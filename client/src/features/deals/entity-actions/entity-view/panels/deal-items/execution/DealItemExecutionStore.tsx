import React from 'react'
import { makeAutoObservable } from 'mobx'
import { DealViewStore } from '../../../DealViewStore'
import { DealItemExecutionsListView } from './executions-list/DealItemExecutionsListView'
import { DealItemStore, generateNumbersArray } from '../DealItemStore'
import { DealItemDto } from 'clients/deal-system-client-definitions'
import { CustomTableCollapsibleDetailConfig, FreeActionProp } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { messageActions, messageError } from 'shared-do-not-touch/material-ui-modals'
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading'
import updatable from 'shared-do-not-touch/utils/updatable'
import { itemErrorHandler } from '../deal-items-utils'
import { compareDates, DateRange, areDateRangesClashing } from 'features/shared/helpers/utils'
import { Moment } from 'moment'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import { computedFn } from 'mobx-utils'
import ExecuteIcon from '@material-ui/icons/LocalAtm'
import DeleteExecuteIcon from '@material-ui/icons/MoneyOff'

export class DealItemExecutionStore {
    constructor(rootStore: DealViewStore, itemStore: DealItemStore) {
        this.rootStore = rootStore
        this.itemStore = itemStore
        makeAutoObservable(this)
    }
    rootStore: DealViewStore
    itemStore: DealItemStore

    deleteExecutionAll = async () => {
        executeLoading('Deleting executions...', () => {
            const dealItems = this.rootStore.itemStore.dealItems
            for (const dealItem of dealItems) {
                if (this.hasExecution(dealItem)) {
                    for (const execItem of dealItem.executedItems) {
                        this.deleteExecution(dealItem, execItem)
                    }
                }
            }
        })
    }

    addExecutionAll = async () => {
        const dealItems = this.rootStore.itemStore.dealItems
        let overwrite = undefined as (boolean | undefined)
        if (dealItems.find(t => this.hasExecution(t))) {
            const answer = await messageActions({
                title: 'Adding executions to all rows',
                content: 'Some deal items already have executions added. What would you like to do with those?',
                actions: [
                    {
                        identifier: 'overwrite',
                        name: 'Overwrite',
                        color: 'secondary',
                    },
                    {
                        identifier: 'ignore',
                        name: 'Ignore',
                        color: 'primary',
                    },
                    {
                        identifier: 'cancel',
                        name: 'Cancel',
                        color: 'primary',
                    },
                ]
            })

            switch (answer.identifier) {
                case "cancel":
                    return
                case "ignore":
                    overwrite = false
                    break
                case "overwrite":
                    overwrite = true
                    break
            }
        }

        executeLoading('Adding Executions...', async () => {
            await this.addExecutionAllAction(overwrite)
        })
    }

    hasExecution = (dealItem: DealItemDto) => !!dealItem.executedItems && !!dealItem.executedItems.find(et => !et.deleted)

    addExecutionAllAction = async (overwrite?: boolean) => {
        const dealItems = this.rootStore.itemStore.dealItems
        for (const dealItem of dealItems) {
            if (this.hasExecution(dealItem)) {
                if (!overwrite) {
                    continue
                }
                for (const execItem of dealItem.executedItems) {
                    this.deleteExecution(dealItem, execItem)
                }
            }
            this.addExecution(dealItem, true)
        }
    }

    executedItems = computedFn(function executedItems(dealItem: DealItemDto) {
        return dealItem.executedItems.filter(et => !et.deleted)
    })

    validateExecutionDatesPeriods(dealItem: DealItemDto, executedItem: DealItemDto): string | void | undefined {
        var { slot } = this.searchExecutionSlots({
            dealItem: dealItem,
            minPeriod: executedItem.halfHourTradingPeriodStart.value,
            maxPeriod: executedItem.halfHourTradingPeriodEnd.value,
            minDate: executedItem.startDate.value,
            maxDate: executedItem.endDate.value,
            executionItem: executedItem,
        })
        if (!slot || (
            compareDates(executedItem.startDate.value, slot.proposedMinDate) !== 0
            ||
            compareDates(executedItem.endDate.value, slot.proposedMaxDate) !== 0
            ||
            executedItem.halfHourTradingPeriodStart.value !== slot.proposedMinPeriod
            ||
            executedItem.halfHourTradingPeriodEnd.value !== slot.proposedMaxPeriod
        )) {
            return "Dates/Periods are clashing"
        }
    }

    dealHasExecutionErrors() {
        return !!this.itemStore.dealItems
            .find(t => !!this.executedItems(t)
                .find(e => !!itemErrorHandler(e)?.hasError))
    }

    fullDatesByPeriod(dealItem: DealItemDto, currentExecution?: DealItemDto, productId?: number) {
        const periodsDates = generateNumbersArray(48, 1).map(() => ([] as DateRange[]))

        const execItems = this.executedItems(dealItem)

        for (const execItem of execItems) {
            if (execItem === currentExecution) {
                continue
            }
            if (!!productId && execItem.productId.value !== productId) {
                continue
            }

            const errorHandler = itemErrorHandler(execItem)
            if (errorHandler && errorHandler.hasError) {
                return false
            }

            const minPeriod = execItem.halfHourTradingPeriodStart.value!
            const maxPeriod = execItem.halfHourTradingPeriodEnd.value!
            const minDate = execItem.startDate.value
            const maxDate = execItem.endDate.value

            if (minDate && maxDate && minPeriod && maxPeriod) {
                const range = { minDate, maxDate }

                for (let period = minPeriod; period <= maxPeriod; period++) {
                    const periodDates = periodsDates[period - 1];
                    const found = periodDates.find(pd => compareDates(maxDate, pd.minDate) === -1)
                    if (found) {
                        const index = periodDates.indexOf(found)
                        periodDates.splice(index, 0, range)
                    } else {
                        periodDates.push(range)
                    }
                }
            }
        }

        return periodsDates
    }

    searchExecutionSlots(
        {
            dealItem,
            minPeriod,
            maxPeriod,
            minDate,
            maxDate,
            productId,
            executionItem,
        }: {
            dealItem: DealItemDto,
            minPeriod?: number,
            maxPeriod?: number,
            minDate?: Moment,
            maxDate?: Moment,
            productId?: number
            executionItem?: DealItemDto
        }
    ): {
        messageType?: 'correctBeforeAdding' | "noSlotAvailable"
        slot?: {
            proposedMinDate?: Moment
            proposedMaxDate?: Moment
            proposedMinPeriod?: number
            proposedMaxPeriod?: number
            proposedProduct?: number
        }
    } {

        var proposedMinDate = minDate
        var proposedMaxDate = maxDate
        var proposedMinPeriod = minPeriod
        var proposedMaxPeriod = maxPeriod
        var proposedProduct = productId
        let dateSet = false

        if (proposedMinDate && proposedMaxDate && minPeriod && maxPeriod) {
            const periodsDates = this.fullDatesByPeriod(dealItem, executionItem, productId)

            if (!periodsDates) {
                return { messageType: 'correctBeforeAdding' }
            }

            for (let period = minPeriod; period <= maxPeriod; period++) {
                const periodDates = periodsDates[period - 1];
                if (dateSet) {
                    let valid = true
                    for (const dateRange of periodDates) {
                        if (areDateRangesClashing({ minDate: proposedMinDate, maxDate: proposedMaxDate }, dateRange)) {
                            valid = false
                            break
                        }
                    }
                    if (valid) {
                        proposedMaxPeriod = period
                    } else {
                        break
                    }
                } else {
                    if (periodDates.length === 0) {
                        proposedMinPeriod = period
                        proposedMaxPeriod = period
                        dateSet = true
                    } else {
                        let temporarySet = false
                        for (const dateRange of periodDates) {
                            const compared = compareDates(proposedMinDate, dateRange.minDate)
                            if (compared === -1) {
                                proposedMaxDate = dateRange.minDate?.clone().subtract(1, 'day')
                                proposedMinPeriod = period
                                proposedMaxPeriod = period
                                temporarySet = false
                                dateSet = true
                                break
                            } else {
                                const newMinDate = dateRange.maxDate?.clone().add(1, 'day')
                                if (compareDates(newMinDate, proposedMaxDate) !== 1) {
                                    proposedMinDate = newMinDate
                                    temporarySet = true
                                } else {
                                    temporarySet = false
                                }
                            }
                        }
                        if (temporarySet) {
                            proposedMinPeriod = period
                            proposedMaxPeriod = period
                            temporarySet = false
                            dateSet = true
                        }

                    }
                }
            }

            if (!dateSet) {
                return { messageType: 'noSlotAvailable' }
            }

        }
        return {
            slot: {
                proposedMinDate,
                proposedMaxDate,
                proposedMinPeriod,
                proposedMaxPeriod,
                proposedProduct,
            }
        }
    }

    get productIsAmongExecutionFields() {
        return !!this.itemStore.itemFieldsDefinition.find(d => !!d.execution && d.field === 'productId')
    }

    addExecution = (dealItem: DealItemDto, all = false) => {
        if (!all) {
            var { slot, messageType } = this.searchExecutionSlots({
                dealItem: dealItem,
                minPeriod: dealItem.halfHourTradingPeriodStart.value,
                maxPeriod: dealItem.halfHourTradingPeriodEnd.value,
                minDate: dealItem.startDate.value,
                maxDate: dealItem.endDate.value,
                productId: this.productIsAmongExecutionFields ? dealItem.productId.value : undefined,
            })

            switch (messageType) {
                case 'correctBeforeAdding':
                    messageError({ content: "Please correct other execution errors before adding a new one." })
                    return
                case 'noSlotAvailable':
                    if (this.productIsAmongExecutionFields) {
                        if (!slot) {
                            messageInfo({
                                title: "No Slots",
                                content: "All the products are already taken. To make room for an additional execution, please change the existing products."
                            })
                            return
                        }
                    } else {
                        messageInfo({
                            title: "No Slots",
                            content: "All the dates and periods within the item are already taken. To make room for an additional execution, " +
                                "please change the existing executions' dates/periods."
                        })
                        return
                    }
            }

            if (!slot) {
                return
            }
        }

        const newItem = this.rootStore.itemStore.createNewItem(dealItem, true)
        dealItem.executedItems.push(newItem)
        dealItem.updated = true

        if (slot) {
            updatable(newItem.startDate, slot.proposedMinDate)
            updatable(newItem.endDate, slot.proposedMaxDate)
            updatable(newItem.halfHourTradingPeriodStart, slot.proposedMinPeriod)
            updatable(newItem.halfHourTradingPeriodEnd, slot.proposedMaxPeriod)
            if (slot.proposedProduct) {
                updatable(newItem.productId, slot.proposedProduct)
                newItem.productDescription = this.itemStore.getProductDescription(slot.proposedProduct)
            }
        }
    }

    deleteExecution = (dealItem: DealItemDto, executionItem: DealItemDto) => {
        executionItem.deleted = true
        dealItem.updated = true
    }

    get collapsibleDetailConfig() {
        if (!this.canChangeExecutions && !this.rootStore.deal.executed) {
            return undefined
        }

        return (row: DealItemDto) => (({
            rowHasCollapsibleDetail: true,
            toggleTooltip: 'Execution Details',
            collapsibleDetailStyling: {
                paddingTop: 0, paddingLeft: 0, paddingRight: 0, paddingBottom: 0, // has to be all four because the component sets them individually.
            },
            render: ({ row, opened }) => opened ? <DealItemExecutionsListView dealItem={row} /> : null
        }) as CustomTableCollapsibleDetailConfig<DealItemDto>)
    }

    get executionFields() {
        return this.itemStore.itemFieldsDefinition.filter(fd => fd.execution)
    }
    get itemExecutionImportTemplateType() {
        return this.rootStore.dealTypeConfiguration?.itemExecutionImportTemplateType
    }
    get canChangeExecutions() {
        return this.rootStore.sp.execution || (this.rootStore.deal?.isExecutionStatus && !this.rootStore.sp.readOnly && this.rootStore.deal.assignedToSelf)
    }
    get canImportExecutions() {
        return this.canChangeExecutions && !!this.itemExecutionImportTemplateType
    }

    get executionFreeActions() {
        const actions = [] as FreeActionProp<DealItemDto>[]

        if (this.canChangeExecutions) {
            actions.push({
                callback: this.addExecutionAll,
                icon: () => <ExecuteIcon />,
                title: 'Add execution row to all deal items with the original item values',
            })
            actions.push({
                callback: this.deleteExecutionAll,
                icon: () => <DeleteExecuteIcon />,
                title: 'Delete all executions',
            })
        }
        return actions
    }
}