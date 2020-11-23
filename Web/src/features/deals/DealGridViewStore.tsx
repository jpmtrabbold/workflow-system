import { DealListDto, DealsListRequest, DealsListResponse, FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import { dealClient } from '../../clients/deal-system-rest-clients'
import { dealGridDefinition } from './DealGridDefinition'
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { messageConfirm, messageActions, messageNoYes, snackbar, messageYesNo, messageError } from 'shared-do-not-touch/material-ui-modals'
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import { GlobalStore } from 'features/shared/stores/GlobalStore'
import { History } from 'history'
import React from 'react'
import Typography from '@material-ui/core/Typography'
import Tooltip from '@material-ui/core/Tooltip'
import InfoIcon from '@material-ui/icons/Info'
import Chip from '@material-ui/core/Chip'
import { Theme } from '@material-ui/core/styles'

export type DealListParams = {
    page: number
    pageSize: number
    sortField: string | undefined
    sortDirection: "asc" | "desc"
    searchString: string
    assignedToMeMode?: boolean
}

export interface DealGridViewStoreProps {
    assignedToMe?: boolean
    dealId: string
    subFunctionalityId: string
    globalStore: GlobalStore
    history: History
    theme: Theme
}

export type DealEntityActionType = ''

export default class DealGridViewStore {
    sp: DealGridViewStoreProps
    constructor(sp: DealGridViewStoreProps) {
        this.gridStore = new GridViewStore({
            gridDefinition: dealGridDefinition,
            listRequestType: DealsListRequest,
            listMethod: (listRequest) => dealClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => {
                this.processSpecificDeal(listResponse.deals)
                return listResponse.deals
            },
            editListRequest: (listRequest, params) => {
                if (!!sp.assignedToMe) {
                    listRequest.onlyDealsAssignedToMeOrMyRole = true
                    listRequest.includeFinalizedDeals = false
                } else {
                    listRequest.onlyDealsAssignedToMeOrMyRole = false
                    listRequest.includeFinalizedDeals = true
                }

                if (this.specificDeal) {
                    listRequest.dealId = parseInt(this.sp.dealId)
                }
            },
            canEditEntity: async (rowData: DealListDto) => {
                try {
                    var assignInfo = await executeLoading("Retrieving Assignment Info...", () => dealClient.dealAssignInfo(rowData.id))
                } catch (error) {
                    return false
                }

                if (!assignInfo) return false

                if (assignInfo.assignedToSelf) {
                    return true
                } else if (assignInfo.assignedToUsersRole) {
                    const assignedToAnotherUser = !!assignInfo.assignedToUserId

                    if (assignedToAnotherUser && this.firstLoad && this.specificDeal) {
                        messageInfo({ title: "Deal Already Taken", content: `The user ${assignInfo.assignedToUserDescription} is already working on this deal.` })
                        return false
                    }

                    const msg = (assignedToAnotherUser ?
                        `This deal is assigned to another user (${assignInfo.assignedToUserDescription}) from the same workflow role as you. ` :
                        "This deal is assigned to your workflow role. ")

                    const actionTaken = await messageActions({
                        title: "Assign to self?",
                        content: msg + 'What would you like to do?',
                        variant: 'bigger actions',
                        actions: [
                            { identifier: 'assign', name: `Assign it specifically to yourself so you can work on it`, color: 'primary' },
                            { identifier: 'open', name: `Open the deal with limited functionality (as it is not assigned to you)`, color: 'secondary' },
                            { identifier: 'nothing', name: `Cancel` },
                        ]
                    })

                    if (actionTaken.identifier === 'assign') {
                        try {
                            await executeLoading("Assigning deal to self...", () => dealClient.assignDealToSelf(rowData.id, (!!assignInfo.assignedToUserId ? assignInfo.assignedToUserId : undefined)))
                            return true
                        } catch (error) {
                            return false
                        }
                    } else if (actionTaken.identifier === 'open') {
                        return true
                    } else {
                        return false
                    }
                } else if (assignInfo.revertBackInfo.canRevertStatusBack) {
                    const revert = assignInfo.revertBackInfo
                    const actionTaken = await messageActions({
                        title: "Can Revert Status Back",
                        content: `This deal moved to the '${revert.currentDealWorkflowStatusName}' status ` +
                            `when you took the '${revert.precedingWorkflowActionName}' action, ` +
                            `but no one from ${revert.currentWorkflowRoleName} have picked it up to work on it yet. ` +
                            `What would you like to do?`,
                        variant: 'bigger actions',
                        actions: [
                            { identifier: 'open', name: `Open the deal with limited functionality (as it is not assigned to you)`, color: 'primary' },
                            { identifier: 'revert', name: `Revert the deal status to '${revert.previousDealWorkflowStatusName}' and assign it back to yourself so you can work on it again`, color: 'secondary' },
                            { identifier: 'nothing', name: `Cancel` },
                        ]
                    })
                    if (actionTaken.identifier === 'open') {
                        return true
                    } else if (actionTaken.identifier === 'revert') {
                        try {
                            await executeLoading("Reverting deal status back...", () =>
                                dealClient.revertDealStatusBack(
                                    rowData.id,
                                    revert.previousWorkflowStatusId,
                                    revert.currentDealWorkflowStatusId,
                                    (!!assignInfo.assignedToUserId ? assignInfo.assignedToUserId : undefined)))

                            return true
                        } catch (error) {
                            return false
                        }
                    } else {
                        return false
                    }
                }
                return await messageConfirm({
                    content: "As this deal is not assigned to you or your workflow role, you will have limited functionality. Would you like to continue?",
                    title: "Not your deal"
                })
            },
        })
        this.sp = sp

    }
    gridStore: GridViewStore<DealListDto, DealsListRequest, DealsListResponse>

    get gridTitle() {
        const title = this.sp.assignedToMe ? 'Deals Assigned To Me' : 'All Deals'
        const tooltip = this.sp.assignedToMe && (
            <Tooltip title='Here you can see all deals that are assigned to you or your role, that are still not completed - deals on which you can take action.'>
                <InfoIcon style={{ marginLeft: 5, fontSize: '0.8em' }} />
            </Tooltip>
        )
        const chip = this.specificDeal && (
            <Chip
                style={{ marginLeft: this.sp.theme.spacing(3) }}
                label={`Deal ${this.specificDealIdentifier}`}
                onDelete={this.removeSpecificDeal}
                color="primary"
            />
        )
        
        return <>
            <Typography variant='h6'>
                {title}
                {tooltip}
            </Typography>
            {chip}
        </>
    }

    removeSpecificDeal = () => {
        const arr = this.sp.history.location.pathname.split('/')
        arr.pop()
        this.sp.history.replace(arr.join('/'))
    }

    firstLoad = true
    processSpecificDeal(deals: DealListDto[]) {
        if (this.specificDeal) {
            if (deals.length === 1) {
                const row = deals[0]

                this.specificDealNumber = row.dealNumber

                if (this.firstLoad) {
                    const subFunc: SubFunctionalityEnum = parseInt(this.sp.subFunctionalityId)
                    if (subFunc) {
                        const { functionality } = this.sp.globalStore
                        const funcHelper = functionality(FunctionalityEnum.Deals)
                        if (funcHelper.hasSubFunctionality(subFunc)) {
                            if (subFunc === SubFunctionalityEnum.Edit) {
                                this.gridStore.onActionClick("edit", row)
                            } else if (subFunc === SubFunctionalityEnum.ExecuteDeal) {
                                this.execute(row)
                            } else {
                                messageError({ title: "Access Restricted", content: `SubFunctionality ${subFunc} was not expected in this context.` })
                            }
                        } else {
                            const action = subFunc === SubFunctionalityEnum.Edit ? "edit" : subFunc === SubFunctionalityEnum.ExecuteDeal ? "execute" : null
                            if (action) {
                                messageError({ title: "Access Restricted", content: `You do not have enough permissions to ${action} a deal.` })
                            } else {
                                messageError({ title: "Access Restricted", content: `SubFunctionality ${subFunc} was not expected in this context.` })
                            }
                        }
                    }

                    this.firstLoad = false
                }
            }
        }
    }

    specificDealNumber = ""

    get specificDealIdentifier() {
        return this.specificDealNumber || this.sp.dealId
    }

    get specificDeal() {
        return !!this.sp.dealId
    }

    execute = async (deal: DealListDto) => {
        try {
            var assignInfo = await executeLoading("Retrieving Assignment Info...", () => dealClient.dealExecutionInfo(deal.id))
        } catch (error) {
            return false
        }

        if (!assignInfo) return false

        const execute = () => this.gridStore.onActionClick("execute", deal)

        if (assignInfo.executed) {
            messageInfo({ title: "Deal already executed", content: "Meanwhile, this deal was executed from another session." })
            this.gridStore.reloadTableData()

        } else if (assignInfo.assignedToSelf) {
            execute()
        } else {
            if (!!assignInfo.assignedToUserId) {
                if (!await (messageNoYes({
                    title: "Assign Execution to self?",
                    content: `The execution of this deal is assigned to another user (${assignInfo.assignedToUserName}). Would you like to assign its execution to yourself?`,
                }))) {
                    return false
                }
            }

            try {
                await executeLoading("Assigning deal's execution to self...", () =>
                    dealClient.assignDealExecutionToSelf(deal.id, (!!assignInfo.assignedToUserId ? assignInfo.assignedToUserId : undefined), undefined))
            } catch (error) {
                return false
            }
            execute()
        }
    }

    reverseDealExecution = async (deal: DealListDto) => {
        try {
            var assignInfo = await executeLoading("Retrieving Assignment Info...", () => dealClient.dealExecutionInfo(deal.id))
        } catch (error) {
            return false
        }

        if (!assignInfo) return false

        if (!assignInfo.executed) {
            messageInfo({ title: "Deal not executed anymore", content: "Meanwhile, this deal's execution was reversed from another session." })
        } else {
            if (await messageYesNo({ title: "Reverse deal execution", content: "Do you confirm the reversal of this deal's execution?" })) {
                try {
                    await executeLoading("Reversing deal execution...", () =>
                        dealClient.reverseDealExecution(deal.id, (!!assignInfo.assignedToUserId ? assignInfo.assignedToUserId : undefined), assignInfo.executionDate))
                } catch (error) {
                    return false
                }
                snackbar({ title: `Execution of deal ${deal.dealNumber} was reversed successfully.` })
            }
        }
        this.gridStore.reloadTableData()
    }
}