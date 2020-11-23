import React from 'react'
import { DealViewStore } from "../../DealViewStore"
import { makeAutoObservable } from "mobx"
import { LookupRequest,  DealItemFieldReadDto, DealItemDto, DealItemSourceTypeEnum } from "clients/deal-system-client-definitions"
import { dealClient } from "../../../../../../clients/deal-system-rest-clients"
import updatable, { anyPropertyUpdated, forceUpdatables } from "shared-do-not-touch/utils/updatable"
import FormErrorHandler from "shared-do-not-touch/input-props/form-error-handler"
import { DealItemImportStore } from "./import/DealItemImportStore"
import { ItemAction } from "./entity-view/DealItemView"
import { messageYesNo, snackbar, messageError } from "shared-do-not-touch/material-ui-modals"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import AddIcon from '@material-ui/icons/Add'
import CloudUploadIcon from '@material-ui/icons/CloudUpload'
import CloudDownloadIcon from '@material-ui/icons/CloudDownload'
import LibraryAddIcon from '@material-ui/icons/LibraryAddOutlined'
import DeleteIcon from '@material-ui/icons/DeleteOutlined'

import { CustomTableActions } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { DealItemExecutionStore } from './execution/DealItemExecutionStore'
import moment, { Moment } from 'moment'
import ModalState from 'features/shared/helpers/ModalState'
import { compareDates, momentToDateString } from 'features/shared/helpers/utils'
import { DealItemFieldDefinition, dealDealItemFieldDefinition as dealItemFieldDefinition } from './DealItemFieldDefinition'

export function generateNumbersArray(topNumber: number, starting: number = 1) {
    return Array.from(Array(topNumber - starting + 1).keys()).map(item => item + starting)
}
export class DealItemStore {
    constructor(rootStore: DealViewStore) {
        this.rootStore = rootStore
        this.itemImportStore = new DealItemImportStore(this.rootStore, this)
        this.executionStore = new DealItemExecutionStore(this.rootStore, this)
        makeAutoObservable(this)
    }

    rootStore: DealViewStore
    itemImportStore: DealItemImportStore
    executionStore: DealItemExecutionStore

    errorHandler = new FormErrorHandler<DealItemDto>()

    startPeriodLookup = generateNumbersArray(48).map(item => ({ id: item }))
    get endPeriodLookup() {
        if (!this.currentItem)
            return []
        return generateNumbersArray(48, this.currentItem.halfHourTradingPeriodStart.value).map(item => ({ id: item }))
    }

    get dealItems() {
        const { deal } = this.rootStore
        return deal.items?.data.filter(t => !t.deleted) || [] as DealItemDto[]
    }

    itemFieldsDefinition: DealItemFieldDefinition[] = []
    currentItem?: DealItemDto
    products: LookupRequest[] = []
    get currentItemHasSource() {
        const src = this.currentItem?.sourceData
        if (!src) { return false }

        if (!src.creationDate && !src.sourceId && !src.type) { return false }

        return true
    }
    currentItemIndex = -1

    existingItemsValidation = async (newValue: number) => {
        const promised = await this.rootStore.fetchItemsPromise
        if (!promised.loaded) {
            messageError({ content: "The deal items were not loaded successfully due to an internal error, so the deal type can't be changed." })
            return false
        }
        if (promised.filteredData.length > 0) {
            const confirmed = await messageYesNo(
                {
                    content: `As this deal already contains deal items, if you change the deal or deal type, ` +
                        `they might have to be discarded if the new item fieldset is different. Do you wish to continue?`,
                    title: 'Existing deal items'
                })

            if (!confirmed) {
                return false
            }
        }
        return true
    }

    validateCurrentItem = async () => {
        this.errorHandler.reset()
        for (const definition of this.itemFieldsDefinition) {
            if (definition.validation) {
                const message = definition.validation(this.currentItem!, this)
                if (message) {
                    this.errorHandler.error(definition.field, message)
                }
            }
        }

        if (!this.errorHandler.hasError) {
            if (!this.currentItem!.price.value || this.currentItem!.price.value <= 0) {
                return (await messageYesNo({ content: "This item doesn't have a price. Do you wish to continue?", title: 'No price set' }))
            }
        }

        return !this.errorHandler.hasError
    }

    setItemObservations = () => {
        
        this.rootStore.disposable?.registerReaction(() => this.currentItem?.startDate?.value, this.itemStartDateChanged)
        
        this.rootStore.disposable?.registerReaction(() => this.currentItem?.halfHourTradingPeriodStart?.value, this.itemHalfHourTradingPeriodStartChanged)
    }

    itemStartDateChanged = () => {
        const trans = this.currentItem!
        if (!!trans.startDate.value && !!trans.endDate.value && trans.startDate.value > trans.endDate.value) {
            updatable(trans.endDate, trans.startDate)
        }
    }

    itemHalfHourTradingPeriodStartChanged = () => {
        const trans = this.currentItem!
        if (trans.halfHourTradingPeriodStart.value !== undefined &&
            trans.halfHourTradingPeriodEnd.value !== undefined &&
            trans.halfHourTradingPeriodStart.value > trans.halfHourTradingPeriodEnd.value) {

            updatable(trans.halfHourTradingPeriodEnd, trans.halfHourTradingPeriodStart)
        }
    }

    dealCategoryChanged = () => {
        this.fetchProducts()
    }

    editItem = (rowData: DealItemDto) => {
        this.setExistingItemAsCurrent(rowData)
    }

    closeItem = (action: ItemAction) => {
        switch (action) {
            case 'save':
            case 'save_add_another':
                this.saveItem()
                break
            case 'delete':
                this.deleteCurrentItem()
                break
        }

        this.unsetCurrentItem()

        if (action === 'save_add_another') {
            setTimeout(() => {
                this.setNewItemAsCurrent()
            })
        }
    }

    handleClose = () => this.closeItem('close')
    isNewItem = () => this.currentItemIndex < 0
    deleteHandler = () => this.closeItem('delete')

    usingKeyboardShortcuts = true

    saveAndAddAnother = async () => {
        this.usingKeyboardShortcuts = false
        if (await this.validateCurrentItem()) {
            this.closeItem('save_add_another')
        }
        this.usingKeyboardShortcuts = true
    }

    save = async () => {
        this.usingKeyboardShortcuts = false
        if (await this.validateCurrentItem()) {
            this.closeItem('save')
        }
        this.usingKeyboardShortcuts = true
    }

    deleteItems = async (rows: DealItemDto[]) => {
        executeLoading('Deleting deal items...', async () => {
            await this.deleteItemsAction(rows)
        })

    }
    deleteItemsAction = async (dealItems: DealItemDto[]) => {

        for (const dealItem of dealItems) {
            this.setExistingItemAsCurrent(dealItem)
            this.deleteCurrentItem()
            this.unsetCurrentItem()
        }
        snackbar({ title: 'Items deleted successfully' })
    }

    duplicateItems = async (rows: DealItemDto[]) => {
        executeLoading('Duplicating deal items...', async () => {
            await this.duplicateItemsAction(rows)
        })
    }

    duplicateItemsAction = async (dealItems: DealItemDto[]) => {
        for (const dealItem of dealItems) {
            this.setNewItemAsCurrent(dealItem)
            updatable(this.currentItem!.startDate, moment().startOf('month')) // first day of this month
            updatable(this.currentItem!.endDate, moment().endOf('month').startOf('day')) // last day of this month
            this.saveItem()
            this.unsetCurrentItem()
        }
        snackbar({ title: 'Duplicated deal items added to the end of the list' })
    }

    setNewItemAsCurrentWithoutBase = () => this.setNewItemAsCurrent()

    setNewItemAsCurrent = (baseItem?: DealItemDto) => {
        this.errorHandler.reset()

        this.currentItem = this.createNewItem(baseItem)
        this.setItemObservations()
    }

    createNewItem = (baseItem?: DealItemDto, setOriginFromBase?: boolean) => {
        const dealItems = this.rootStore.deal.items.data
        const newOrder = (dealItems.length > 0 && (dealItems[dealItems.length - 1].order + 1)) || 1
        var entity = new DealItemDto((!!baseItem ? baseItem.clone() : undefined))
        if (baseItem) {
            entity.id = undefined
            forceUpdatables(entity)
        } else {
            this.itemFieldsDefinition.forEach(item => {
                if (!!item.setDefault) {
                    item.setDefault(entity, this)
                }
            })
        }
        entity.order = newOrder
        if (setOriginFromBase) {
            if (!baseItem || !baseItem.id) {
                throw new Error("on the DealItemStore.createNewItem method, with setOriginFromBase = true, the baseItem needs to have an id.")
            }
            entity.originalItemId = baseItem.id
        }
        return entity
    }

    setExistingItemAsCurrent = (rowData: DealItemDto) => {
        this.errorHandler.reset()
        const dealItems = this.rootStore.deal.items.data
        this.currentItemIndex = dealItems.indexOf(rowData)
        if (this.currentItemIndex < 0)
            throw new Error("rowData couldn't be found on store.deal.items")
        this.currentItem = rowData!.clone()
        this.setItemObservations()
    }

    syncUpdatedFlagOnCurrentItem = () => {
        if (!this.currentItem!.updated)
            this.currentItem!.updated = anyPropertyUpdated(this.currentItem)

        return this.currentItem!.updated
    }

    unsetCurrentItem = () => {
        this.currentItem = undefined
        this.currentItemIndex = -1
    }

    saveItem = () => {
        if (this.currentItemIndex >= 0) {
            this.syncUpdatedFlagOnCurrentItem()
            this.rootStore.deal.items.data[this.currentItemIndex] = this.currentItem!
        } else {
            this.rootStore.deal.items.data.push(this.currentItem!)
        }
    }

    deleteCurrentItem = () => {
        if (!this.currentItem || this.currentItemIndex < 0)
            throw new Error("deleteItem function was called but this.currentItem or this.currentItemIndex wasn't set")

        if (this.currentItem.id && this.currentItem.id > 0) {
            // sets as deleted, so it can be deleted on the back-end
            this.rootStore.deal.items.data[this.currentItemIndex].deleted = true
        } else {
            this.rootStore.deal.items.data.splice(this.currentItemIndex, 1)
        }
    }

    loadItemGridDefinition = () => {
        this.itemFieldsDefinition = this.processItemGridDefinition(
            this.rootStore.dealTypeConfiguration!!.dealItemFields,
            this.rootStore.dealTypeConfiguration!!.unitOfMeasure)
    }

    processItemGridDefinition = (itemFields: DealItemFieldReadDto[], unitOfMeasure: string | undefined) => {
        const gridDef = [] as (Omit<DealItemFieldDefinition, 'execution'> & { execution?: boolean })[]
        for (const itemField of itemFields) {
            const gridStandards = dealItemFieldDefinition.filter(item => item.field && item.field.toLowerCase() === itemField.field.toLowerCase())

            for (const gridStandard of gridStandards) {
                if (gridStandard) {

                    let gridTitle = itemField.name
                    if (itemField.field === 'Quantity' && !!unitOfMeasure) {
                        gridTitle = gridTitle + ` (${unitOfMeasure})`
                    }

                    gridDef.push({ ...gridStandard, execution: itemField.execution, title: gridTitle, inputLabel: itemField.name })
                }
            }
        }
        return gridDef
    }

    gotDealTypeConfiguration = async (fromDealTypeChanging?: boolean) => {
        const promised = await this.rootStore.fetchItemsPromise
        if (!promised.loaded) {
            throw new Error("Internal error: deal items were not loaded")
        }

        let fieldSetChanged = false
        if (this.rootStore.dealTypeConfiguration!!.dealItemFieldsetId !== this.rootStore.deal.dealItemFieldsetId.value) {
            fieldSetChanged = true
        } else {
            if (this.rootStore.dealTypeConfiguration!.forcePosition && !!promised.filteredData.find(dt => dt.position.value !== this.rootStore.dealTypeConfiguration!.position)) {
                fieldSetChanged = true
            }
        }
        if (fromDealTypeChanging || !this.rootStore.deal.dealItemFieldsetId.value) {
            updatable(this.rootStore.deal.dealItemFieldsetId, this.rootStore.dealTypeConfiguration!!.dealItemFieldsetId)
        }
        this.loadItemGridDefinition()
        if (fromDealTypeChanging) {
            this.cleanUpItemsNewFieldset(fieldSetChanged)
        }
    }

    cleanUpItemsNewFieldset = async (fieldSetChanged: boolean) => {
        const promised = await this.rootStore.fetchItemsPromise
        if (!promised.loaded) {
            return
        }

        if (promised.filteredData.length === 0) {
            return
        }

        let cleanUp = false

        if (fieldSetChanged) {
            cleanUp = true
        } else {
            cleanUp = !(await messageYesNo({
                content: "The item fieldset for the new deal type hasn't changed. Would you like to keep the existing deal items?",
                title: "Keep deal items?"
            }))
        }

        if (cleanUp) {
            promised.filteredData.forEach(item => item.deleted = true)
        }
    }

    fetchProducts = async () => {
        const { deal } = this.rootStore
        try {
            this.products = await dealClient.getProducts(deal.dealCategoryId.value!)
            return true
        } catch (error) {

        }
        return false
    }

    getProductDescription(id?: number) {
        if (!id) {
            return ""
        }
        const product = this.products.find(n => n.id === id)
        if (!product) {
            return ""
        }
        return product.name
    }

    cleanUpGridDefinition() {
        this.itemFieldsDefinition = []
    }

    get itemActions() {
        const actions: CustomTableActions<DealItemDto> = {
            freeActions: [],
            multipleRowActions: [],
            rowActions: [],
        }

        if (this.rootStore.canEditDealBasicInfo) {
            actions.freeActions!.push({
                callback: this.itemImportStore.downloadExcelTemplate,
                icon: () => <CloudDownloadIcon />,
                title: 'Download template for bulk upload',
            })
            actions.freeActions!.push({
                callback: this.itemImportStore.excelDropzone.open,
                icon: () => <CloudUploadIcon />,
                title: 'Upload deal items from template',
            })
            actions.freeActions!.push({
                callback: this.setNewItemAsCurrentWithoutBase,
                icon: () => <AddIcon />,
                title: 'New Deal Item',
            })
            actions.multipleRowActions!.push({
                callback: this.duplicateItems,
                icon: () => <LibraryAddIcon />,
                title: 'Duplicate Deal Item(s)',
            })
            actions.multipleRowActions!.push({
                callback: this.deleteItems,
                icon: () => <DeleteIcon />,
                title: 'Delete Deal Item(s)',
            })
        }
        actions.freeActions! = actions.freeActions!.concat(this.executionStore.executionFreeActions)

        return actions
    }
    viewingSourceData = new ModalState()

    static getIntegrationSourceTypeDescription = (sourceType: DealItemSourceTypeEnum) => {
        switch (sourceType) {
            case DealItemSourceTypeEnum.Ems:
                return "emsTradepoint"
            case DealItemSourceTypeEnum.Ftr:
                return "FTR"
        }
    }
    get minimumItemStartDate() {
        return this.rootStore.dealCreationDate.clone().startOf('month')
    }

    itemDateErrorMessage(date?: Moment) {
        // if the date is less than the minimum start date
        if (compareDates(date, this.minimumItemStartDate) === -1) {
            return `Date can't be less than ${momentToDateString(this.minimumItemStartDate)}`
        }
        return
    }
}
