import React from 'react'
import Excel from 'exceljs'
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { fileToArrayBuffer } from "shared-do-not-touch/utils/file-operations"
import { createContext } from 'react'
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { isDate } from 'util'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import { convertMonthYearForDisplay } from 'shared-do-not-touch/utils/helper-functions'
import CheckCircleIcon from '@material-ui/icons/CheckCircle'
import CancelIcon from '@material-ui/icons/Cancel'
import { salesForecastClient } from 'clients/deal-system-rest-clients'
import { SalesForecastDto } from 'clients/deal-system-client-definitions'
import { initializeUpdatable } from 'shared-do-not-touch/utils/updatable'
import { snackbar } from 'shared-do-not-touch/material-ui-modals'
import SalesForecastGridViewStore from '../SalesForecastGridViewStore'
import moment, { Moment } from 'moment'
import ModalState from 'features/shared/helpers/ModalState'

const isMonth = (month: any) => {
    if (isNaN(month)) {
        return false
    }
    try {
        month = parseInt(month)
    } catch (error) {
        return false
    }

    if (month < 1 || month > 12) {
        return false
    }
    if (Math.trunc(month) !== month) {
        return false
    }
    return true
}

const isYear = (year: any) => {
    if (isNaN(year)) {
        return false
    }
    try {
        year = parseInt(year)
    } catch (error) {
        return false
    }

    if (year < 1900 || year > 3000) {
        return false
    }
    if (Math.trunc(year) !== year) {
        return false
    }
    return true
}

export class ImportedForecast {
    constructor(monthYear: Excel.Cell, volume: Excel.Cell) {
        this.volumeText = volume.toString()
        if (typeof (volume.value) === 'number') {
            this.volume = volume.value
        } else {
            if (isNaN(this.volumeText as any)) {
                this.errors.push("Volume must be a valid number.")
            }
            this.volume = parseInt(this.volumeText)
        }

        this.monthYearText = monthYear.toString()
        let splitMonthYear = this.monthYearText.split('/')
        if (isDate(monthYear.value)) {
            splitMonthYear = [(monthYear.value.getMonth() + 1).toString(), monthYear.value.getFullYear().toString()]
            this.monthYearText = convertMonthYearForDisplay(monthYear.value)
        } else {
            if (splitMonthYear.length !== 2) {
                this.errors.push("Month/Year must be in the following format: MM/YYYY")
            } else {
                if (!isMonth(splitMonthYear[0])) {
                    this.errors.push(`${splitMonthYear[0]} is not a valid month`)
                }
                if (!isYear(splitMonthYear[1])) {
                    this.errors.push(`${splitMonthYear[1]} is not a valid year`)
                }
            }

        }
        this.monthYear = moment(new Date(parseInt(splitMonthYear[1]), parseInt(splitMonthYear[0]) - 1))
    }
    monthYearText: string
    monthYear?: Moment
    volumeText: string
    volume?: number
    errors = [] as string[]
}

export class SalesForecastUploadStore {
    constructor(rootStore: SalesForecastGridViewStore) {
        this.rootStore = rootStore
    }
    rootStore: SalesForecastGridViewStore

    downloadTemplate = async () => {
        var workbook = new Excel.Workbook()
        workbook.creator = 'DealSystem'
        workbook.lastModifiedBy = 'DealSystem'
        workbook.created = new Date()
        workbook.modified = new Date()

        var mainSheet = workbook.addWorksheet('Sales Forecast')

        const columns = [] as Partial<Excel.Column>[]
        columns.push({ header: "Month/Year", key: 'monthyear' })
        columns.push({ header: "Volume (MW)", key: 'volume' })
        mainSheet.columns = columns

        const date = new Date()
        mainSheet.addRow([
            (date.getMonth() + 2).toString().padStart(2, '0') + '/' + date.getFullYear(),
            1000,
            '(Just an example)'
        ])

        const buffer = await workbook.xlsx.writeBuffer()
        const blob = new Blob([buffer])
        saveAs(blob, `Sales Forecast Template (DealSystem).xlsx`)
    }

    handleFileUpload = (files: File[]) => {
        executeLoading("Processing forecast...", async () => {
            const buffer = await fileToArrayBuffer(files[0])
            this.dropzoneModal.close()
            await this.uploadSalesForecastFile(buffer)
        })
    }

    uploadSalesForecastFile = async (buffer: ArrayBuffer) => {
        var workbook = new Excel.Workbook()
        await workbook.xlsx.load(buffer)

        this.importedForecasts = []
        this.hasErrors = false
        var worksheet = workbook.getWorksheet(1)
        for (let index = 2; index <= worksheet.rowCount; index++) {
            const row = worksheet.getRow(index)
            const monthYear = row.getCell(1)
            const volume = row.getCell(2)
            // ignore blank cells
            if (!monthYear.toString() && !volume.toString()) {
                continue
            }
            const imported = new ImportedForecast(monthYear, volume)

            this.importedForecasts.push(imported)
            if (imported.errors.length > 0) {
                this.hasErrors = true
            }
        }

        this.previewModal.open()

        if (this.hasErrors) {
            setTimeout(() => {
                messageInfo({
                    title: 'Inconsistent Data',
                    content: "The data being imported is inconsistent and cannot be imported into DealSystem."
                })
            });
        }
    }
    onDragEnter = (event: React.DragEvent<HTMLDivElement>) => {
        this.dropzoneModal.open()
    }
    dropzoneModal = new ModalState({
        onOpen: () => {
            return this.rootStore.canAddForecast
        }
    })

    previewModal = new ModalState()

    confirmUpload = async () => {
        executeLoading("Uploading Sales Forecasts...", async () => {
            const bulkData = [] as SalesForecastDto[]
            for (const imported of this.importedForecasts) {
                const forecast = new SalesForecastDto()
                initializeUpdatable(forecast.monthYear, imported.monthYear)
                initializeUpdatable(forecast.volume, imported.volume)
                bulkData.push(forecast)
            }

            await salesForecastClient.bulkImport(bulkData)
            snackbar({ title: 'Sales Forecast data imported successfully', variant: 'success', anchorOrigin: { vertical: 'top', horizontal: 'center' } })
            this.previewModal.close()
            this.rootStore.gridStore.reloadTableData()
        })
    }

    hasErrors = false
    importedForecasts = [] as ImportedForecast[]

    previewColumns: Column<ImportedForecast>[] = [
        {
            title: 'Valid',
            render: (data) => (data.errors.length === 0 ? <CheckCircleIcon style={{ color: 'green' }} /> : <CancelIcon style={{ color: 'red' }} />),
        },
        { title: 'Month/Year', field: 'monthYearText', },
        { title: 'Volume (MW)', field: 'volumeText', },
        {
            title: 'Errors',
            render: (data) => <>
                {data.errors.map((error, index) => <>{index > 0 && <br />} - {error}</>)}
            </>,
        },
    ]
}
export const SalesForecastUploadStoreContext = createContext(new SalesForecastUploadStore(new SalesForecastGridViewStore()))