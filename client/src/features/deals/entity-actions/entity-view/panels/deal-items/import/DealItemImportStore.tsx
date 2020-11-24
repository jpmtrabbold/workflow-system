import React from 'react'
import Excel from 'exceljs'
import { action } from 'mobx'
import { DealItemStore } from '../DealItemStore'
import { Column, CustomTableActions, CustomTable } from 'shared-do-not-touch/material-ui-table/CustomTable'
import CheckCircleIcon from '@material-ui/icons/CheckCircle'
import CancelIcon from '@material-ui/icons/Cancel'
import CheckCircleOutlinedIcon from '@material-ui/icons/CheckCircleOutlined'
import CancelOutlinedIcon from '@material-ui/icons/CancelOutlined'
import Chip from '@material-ui/core/Chip'
import Tooltip from '@material-ui/core/Tooltip'
import { DealItemFieldDefinition, ILookup } from '../DealItemFieldDefinition'
import { DealViewStore } from '../../../DealViewStore'
import FormErrorHandler from 'shared-do-not-touch/input-props/form-error-handler'
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading'
import { fileToArrayBuffer } from 'shared-do-not-touch/utils/file-operations'
import { messageYesNo, messageError, messageWarning } from 'shared-do-not-touch/material-ui-modals'
import { DealItemDto, DealItemExecutionImportTemplateTypeEnum } from 'clients/deal-system-client-definitions'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import IconButton from '@material-ui/core/IconButton'
import makeStyles from '@material-ui/core/styles/makeStyles'
import { observer } from 'mobx-react-lite'
import ModalState from 'features/shared/helpers/ModalState'
import { momentToDateString } from 'features/shared/helpers/utils'
import { getMonthIfFullMonthItem, addStringWithSeparator } from '../deal-items-utils'
import { messageActions } from 'shared-do-not-touch/material-ui-modals'
import { InfoHover } from 'features/shared/components/info-hover/InfoHover'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export class ImportedItem extends DealItemDto {
    constructor() {
        super()
        ImportedItem.init(this)
    }
    static init(t: ImportedItem) {
        t.valid = true
        t.warnings = []
        t.errorHandler = new FormErrorHandler<ImportedItem>()
    }
    lineNumber?: number
    filename?: string
    filenumber?: string
    valid?: boolean
    warnings?: string[]
    errorHandler?: FormErrorHandler<ImportedItem>
    dayTypeDescription?: string
    positionDescription?: string
    importedInvalidStartDate?: string
    importedInvalidEndDate?: string
    importedInvalidPrice?: string
    parentItem?: DealItemDto
}

const useStyles = makeStyles(theme => ({
    iconButton: {
        margin: theme.spacing(-2)
    }
}))

export class DealItemImportStore {
    constructor(rootStore: DealViewStore, itemStore: DealItemStore) {
        this.rootStore = rootStore
        this.itemStore = itemStore
    }

    rootStore: DealViewStore
    itemStore: DealItemStore

    downloadExcelTemplate = async () => {
        var workbook = new Excel.Workbook()
        workbook.creator = 'WorkflowSystem'
        workbook.lastModifiedBy = 'WorkflowSystem'
        workbook.created = new Date()
        workbook.modified = new Date()

        var mainSheet = workbook.addWorksheet('Items')
        var validationSheet = workbook.addWorksheet('Validations')

        const columns = [] as Partial<Excel.Column>[]
        let columnLetter = 'A'
        let columnValidationLetter = 'A'
        const validationSets = [] as { lookups: ILookup[], columnValidationLetter: string, columnLetters: string[] }[]

        for (const definition of this.itemStore.itemFieldsDefinition) {
            columns.push({ header: definition.title + ' (' + definition.field + ')', key: definition.field })
            if (!!definition.lookups) {
                const lookups = definition.lookups(this.itemStore)
                let set = validationSets.find(s => s.lookups === lookups)
                if (!set) {
                    set = { lookups, columnValidationLetter, columnLetters: [] }
                    validationSets.push(set)
                    columnValidationLetter = String.fromCharCode(columnValidationLetter.charCodeAt(0) + 1)
                }
                set.columnLetters.push(columnLetter)
            }
            columnLetter = String.fromCharCode(columnLetter.charCodeAt(0) + 1)
        }
        mainSheet.columns = columns

        //const { products } = this.itemStore
        let i = 0
        let keepGoing = true
        while (keepGoing) {
            const row = Array(validationSets.length)
            keepGoing = false

            for (let colIndex = 0; colIndex < validationSets.length; colIndex++) {
                const set = validationSets[colIndex]
                if (i < set.lookups.length) {
                    keepGoing = true
                    row[colIndex] = set.lookups[i].name
                }
            }

            if (keepGoing) {
                validationSheet.addRow(row)
            } else {
                break
            }
            i++
        }

        for (const set of validationSets) {
            for (const letter of set.columnLetters) {
                for (let index = 2; index <= 2000; index++) {
                    mainSheet.getCell(letter + index).dataValidation = {
                        type: 'list',
                        allowBlank: false,
                        formulae: ['Validations!$' + set.columnValidationLetter + '$1:$' + set.columnValidationLetter + '$' + set.lookups.length]
                    }
                }
            }
        }

        const buffer = await workbook.xlsx.writeBuffer()
        const blob = new Blob([buffer])

        saveAs(blob, `WorkflowSystem Deal Item Template - ${this.rootStore.infoStore.dealTypeName}.xlsx`)
    }

    handleExcelFile = async (files: File[]) => {
        if (files && files.length > 0) {
            if (files.length >= this.excelFileTotalCount) {
                executeLoading("Processing deal items...", async () => {
                    this.excelDropzone.close()
                    await this.uploadExcelTemplate(files)
                })
            }
        }
    }

    excelFileTotalCount = -1

    onDragEnter = (event: React.DragEvent<HTMLDivElement>) => {
        this.excelFileTotalCount = event.dataTransfer.items.length
        this.excelDropzone.open()
    }

    excelDropzone = new ModalState({
        onOpen: () => {
            if (!this.rootStore.canEditDealBasicInfo &&
                !this.itemStore.executionStore.canChangeExecutions) {
                return false
            }
            if (this.importedItemsReviewModal.visible || this.itemStore.currentItem) {
                return false
            }

            if (this.rootStore.canEditDealBasicInfo) {
                this.importType = 'dealItem'
            }
        }
    })

    importType?: 'dealItem'

    get fileLimit() {
        if (this.itemStore.executionStore.itemExecutionImportTemplateType === DealItemExecutionImportTemplateTypeEnum.FTRs) {
            return 30
        }
        return 1
    }
    uploadExcelTemplate = async (files: File[]) => {
        let ret = false
        if (this.importType === 'dealItem') {
            ret = await this.importItems(files)
        }
        ret && this.importedItemsReviewModal.open()
    }

    importItems = async (files: File[]) => {
        const file = files[0]
        if (!file) {
            return false
        }
        const split = file.name.split('.')
        const ext = split[split.length - 1].toUpperCase()
        var workbook = new Excel.Workbook()
        try {
            if (ext === "XLS" || ext === "XLSX") {
                const buffer = await fileToArrayBuffer(file)
                await workbook.xlsx.load(buffer)
            } else {
                messageError({ content: "Invalid File. Only excel files are accepted" })
                return false
            }

        } catch (error) {
            messageError({ title: 'File Error', content: JSON.stringify(error) })
            return false
        }
        if (!workbook) {
            messageError({ content: "Invalid File" })
            return false
        }
        this.importedGridDefinition = this.generateGridDefinition()

        var worksheet = workbook.getWorksheet('Items')
        if (!worksheet) {
            if (await messageYesNo({
                content: "The template does not have the mandatory 'Items' worksheet. Would you like to download a valid template?",
                title: "Invalid Template"
            })) {
                this.downloadExcelTemplate()
            }
            return false
        }
        var fields: DealItemFieldDefinition[] = []
        var header = worksheet.getRow(1)

        for (let index = 1; index <= header.cellCount; index++) {
            const cell = header.getCell(index)
            if (!!cell.value) {
                const field = cell.value.toString().replace(/^(.{0,})\(/, '').replace(/\)(.{0,})/, '') // this converts "Product (productId)" into "productId"
                const def = this.itemStore.itemFieldsDefinition.find(fd => fd.field === field)
                if (!def) {
                    if (await messageYesNo({
                        content: `The field ${field} is not valid for the current item field set. Would you like to download a valid template?`,
                        title: "Invalid Template"
                    })) {
                        this.downloadExcelTemplate()
                    }
                    return false
                }
                fields.push(def)
            }
        }

        this.resetImportedItems()

        for (let rowIndex = 2; rowIndex <= worksheet.rowCount; rowIndex++) {
            const row = worksheet.getRow(rowIndex)
            const dealItem = new ImportedItem()
            let hasValue = false
            for (let cellIndex = 1; cellIndex <= fields.length; cellIndex++) {
                const cell = row.getCell(cellIndex)
                if (!hasValue && cell.toString()) {
                    hasValue = true
                }
                const field = fields[cellIndex - 1]
                let message = (field.setFieldFromImport(cell.value, dealItem, this.itemStore)) || undefined
                if (!message && !!field.validation) {
                    message = field.validation(dealItem, this.itemStore, true) || undefined
                }
                if (message) {
                    dealItem.valid = false
                    dealItem.errorHandler!.error(field.field as any, message)
                }
            }
            if (hasValue) {
                dealItem.lineNumber = rowIndex
                this.importedItems.push(dealItem)
            }
        }

        return true
    }
    importedGridDefinition = [] as Column<ImportedItem>[]
    importedItems = [] as ImportedItem[]
    importedItemsReviewModal = new ModalState({
        onOpen: () => {
            if (this.hasError) {
                setTimeout(() => {
                    messageError({
                        content: "There are invalid deal items, therefore this action can't be executed. Please correct those errors on the spreadsheed and upload it again."
                    })
                    return
                });
            } else if (this.hasWarning) {
                setTimeout(() => {
                    messageWarning({
                        content: "Although all item are valid, there are warnings that you might want to check out before confirming."
                    })
                    return
                });
            }
        }
    })

    generateGridDefinition({ execution } = { execution: false }) {
        let fileCols: Column<ImportedItem>[] = []
        if (this.fileLimit > 1) {
            fileCols.push({
                title: "File",
                render: (data) => <InfoHover title={data.filenumber} tooltip={data.filename} />
            })
        }
        const lineNumberCol: Column<ImportedItem> = {
            field: 'lineNumber',
            title: 'Line',
        }

        const validCol: Column<ImportedItem> = {
            field: 'valid',
            title: 'Valid',
            rendererComponent: observer(({ data }) => {
                const classes = useStyles()

                if (data.valid && !!data.warnings && data.warnings.length > 0) {
                    const onClick = () => {
                        messageInfo({
                            title: 'Deal Item Warning',
                            content: data.warnings!.length === 1 ? data.warnings![0] : (
                                <>
                                    {data.warnings!.map((warning, i) => (
                                        <>
                                            {i > 0 && <br />}
                                            {"- " + warning}
                                        </>
                                    ))}
                                </>
                            )
                        })
                    }

                    return (
                        <Tooltip title={"Click here to see the warning(s)"}>
                            <IconButton onClick={onClick} className={classes.iconButton}>
                                <CheckCircleIcon style={{ color: 'yellow' }} />
                            </IconButton>
                        </Tooltip>
                    )
                }

                if (data.valid) {
                    return (
                        <Tooltip title={"This item is valid"}>
                            <IconButton className={classes.iconButton}>
                                <CheckCircleIcon style={{ color: 'green' }} />
                            </IconButton>
                        </Tooltip>
                    )
                } else {
                    return (
                        <Tooltip title={"This item is not valid"}>
                            <IconButton className={classes.iconButton}>
                                <CancelIcon style={{ color: 'red' }} />
                            </IconButton>
                        </Tooltip>
                    )
                }
            }),
        }
        const executionCols = [] as Column<ImportedItem>[]
        const that = this
        if (execution) {
            executionCols.push({
                field: 'originalItemId',
                title: 'Parent Deal Item',
                rendererComponent: observer(({ data }) => {
                    const localStore = useStore(sp => ({
                        get title() {
                            let s = ""
                            const pt = sp.parentItem
                            if (pt) {
                                const month = getMonthIfFullMonthItem({ dealItem: pt })
                                if (month) {
                                    s = addStringWithSeparator(s, month.format('MMM-YY'))
                                } else {
                                    s = addStringWithSeparator(s,
                                        `Date Range: ${momentToDateString(pt.startDate?.value)} to ${momentToDateString(pt.endDate?.value)}`)

                                    if (pt.halfHourTradingPeriodStart.value !== 1 || pt.halfHourTradingPeriodEnd.value !== 48) {
                                        s = addStringWithSeparator(s,
                                            `Period Range: ${pt.halfHourTradingPeriodStart?.value} to ${pt.halfHourTradingPeriodEnd?.value}`)
                                    }
                                }
                                for (const field of that.itemStore.itemFieldsDefinition
                                    .filter(d => ![
                                        'startDate',
                                        'endDate',
                                        'halfHourTradingPeriodStart',
                                        'halfHourTradingPeriodEnd',
                                        'position',
                                        'dayType'
                                    ].includes(d.field))) {

                                    const value = field.render && field.render(pt)
                                    if (value) {
                                        s = addStringWithSeparator(s, `${field.title}: ${value}`)
                                    }
                                }
                            }
                            return s + (data.errorHandler?.fieldHasError('parentItem') ? ' (Error)' : '')
                        },
                        showParent() {
                            if (sp.parentItem) {
                                messageActions({
                                    maxWidth: 'xl',
                                    content: (
                                        <CustomTable
                                            title='Parent Deal Item'
                                            columns={that.itemStore.itemFieldsDefinition}
                                            rows={[sp.parentItem]}
                                            searchable={false}
                                            pagination={false}
                                        />
                                    )
                                })
                            }
                            if (sp.errorHandler?.fieldHasError('parentItem')) {
                                messageError({ content: sp.errorHandler?.getFieldError('parentItem').helperText })
                            }
                        }
                    }), data)

                    return (
                        <InfoHover
                            title={localStore.title}
                            tooltip='Click to view item'
                            onClick={localStore.showParent}
                        />
                    )
                }),
            })

            if (this.itemStore.executionStore.itemExecutionImportTemplateType === DealItemExecutionImportTemplateTypeEnum.FTRs) {
                executionCols.push({
                    title: 'FTR ID',
                    render: data => data.sourceData?.sourceId,
                })
            }
        }

        const definitionsWithErrorDisplay = this.itemStore.itemFieldsDefinition
            .filter(d => execution ? !!d.execution : true)
            .map(previousDefinition => {
                const { render, alternativeImportRenderer, ...all } = previousDefinition

                const newRender = (data: ImportedItem) => {
                    const alternativeData = !!alternativeImportRenderer ? alternativeImportRenderer(data) : undefined
                    let standardRender = (alternativeData !== undefined ? alternativeData : (
                        (!!render && render(data)) || null
                    ))

                    const error = data.errorHandler?.getFieldError(all.field as any)?.helperText

                    if (error) {

                        const onErrorClick = () => {
                            messageError({
                                content: error
                            })
                        }

                        return (
                            <Tooltip title={"Click here to see the error"}>
                                <Chip
                                    variant='outlined'
                                    label={standardRender}
                                    icon={<CancelIcon style={{ color: 'red', marginLeft: '5px', marginRight: '5px' }} />}
                                    onClick={onErrorClick}
                                />
                            </Tooltip>
                        )
                    } else {
                        return standardRender
                    }
                }

                return { render: newRender, ...all }
            })

        return [...fileCols, lineNumberCol, validCol, ...executionCols, ...definitionsWithErrorDisplay]
    }

    get hasError() {
        return this.importedItems.some(i => !i.valid)
    }

    get hasWarning() {
        return this.importedItems.some(i => i.warnings && i.warnings.length > 0)
    }

    confirmImportedItems = () => {
        executeLoading("Confirming deal items...", action(async () => {
            if (this.importType === 'dealItem') {
                this.commitImportedItems()
            }
            this.importedItemsReviewModal.close()
        }))
    }

    commitImportedItems = () => {
        for (const importedItem of this.importedItems) {
            this.itemStore.setNewItemAsCurrent(importedItem)
            this.itemStore.saveItem()
            this.itemStore.unsetCurrentItem()
        }
    }

    onlyErrors = false
    onlyWarnings = false
    setOnlyErrors = () => {
        this.onlyErrors = true
        this.onlyWarnings = false
    }
    setOnlyWarnings = () => {
        this.onlyErrors = false
        this.onlyWarnings = true
    }
    clearFilter = () => {
        this.onlyErrors = false
        this.onlyWarnings = false
    }

    resetImportedItems = () => {
        this.importedItems = []
        this.clearFilter()
    }
    get filteredImportedItems() {
        if (this.onlyErrors) {
            return this.importedItems.filter(t => !t.valid)
        }
        if (this.onlyWarnings) {
            return this.importedItems.filter(t => t.warnings && t.warnings.length > 0)
        }
        return this.importedItems
    }

    get actions() {
        const actions: CustomTableActions<ImportedItem> = { freeActions: [] }
        if (this.hasError) {
            actions.freeActions!.push({
                title: (this.onlyErrors ? 'Clear filter' : 'Show only errors'),
                callback: (this.onlyErrors ? this.clearFilter : this.setOnlyErrors),
                icon: () => (this.onlyErrors ?
                    <CancelIcon style={{ color: 'red' }} /> :
                    <CancelOutlinedIcon style={{ color: 'red' }} />),
            })
        }
        if (this.hasWarning) {
            actions.freeActions!.push({
                title: (this.onlyWarnings ? 'Clear filter' : 'Show only warnings'),
                callback: (this.onlyWarnings ? this.clearFilter : this.setOnlyWarnings),
                icon: () => (this.onlyWarnings ?
                    <CheckCircleIcon style={{ color: 'yellow' }} /> :
                    <CheckCircleOutlinedIcon style={{ color: 'yellow' }} />),
            })
        }
        return actions
    }

}