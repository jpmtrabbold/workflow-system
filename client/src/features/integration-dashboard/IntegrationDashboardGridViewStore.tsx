import React from 'react'
import { IntegrationRunListDto, IntegrationRunsListRequest, IntegrationRunsListResponse, IntegrationRunStatusEnum, FunctionalityEnum, SubFunctionalityEnum } from "../../clients/deal-system-client-definitions"
import { integrationClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { IntegrationDashboardGridViewDefinition } from "./IntegrationDashboardGridViewDefinition"

import CheckCircleIcon from '@material-ui/icons/CheckCircle'
import InfoIcon from '@material-ui/icons/Info'
import CancelIcon from '@material-ui/icons/Cancel'
import RefreshIcon from '@material-ui/icons/Refresh'
import CheckCircleIconOutlined from '@material-ui/icons/CheckCircleOutlined'
import InfoIconOutlined from '@material-ui/icons/InfoOutlined'
import CancelIconOutlined from '@material-ui/icons/CancelOutlined'
import RefreshIconOutlined from '@material-ui/icons/RefreshOutlined'
import Tooltip from "@material-ui/core/Tooltip"
import { IntegrationDashboardGridViewProps } from './IntegrationDashboardGridView'
import PlayCircleFilledIcon from '@material-ui/icons/PlayCircleFilled'
import { GlobalStore } from 'features/shared/stores/GlobalStore'
import { FreeActionProp } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading'
import { History } from 'history'
export type IntegrationRunAction = 'close' | 'reprocess' | 'mark-as-not-pending' | 'mark-as-pending'
type IntegrationDashboardGridViewStoreParams = IntegrationDashboardGridViewProps & {
    globalStore: GlobalStore,
    integrationRunId?: string,
    history: History
}

export default class IntegrationDashboardGridViewStore {

    constructor(sp: IntegrationDashboardGridViewStoreParams) {
        this.gridStore = new GridViewStore({
            gridDefinition: [],
            listRequestType: IntegrationRunsListRequest,
            listMethod: (listRequest) => {
                if (!!sp.integrationRunId) {
                    listRequest.integrationRunId = parseInt(sp.integrationRunId)
                } else {
                    listRequest.integrationType = sp.integrationStore.integrationType
                    listRequest.statuses = this.onlyPending ? [IntegrationRunStatusEnum.Error] : []
                }

                return integrationClient.list(listRequest)
            },
            entitiesFromViewModel: (listResponse) => listResponse.runs,
        })
        this.sp = sp
        this.gridStore.gridDefinition = IntegrationDashboardGridViewDefinition(this)
        this.setActions()
    }
    gridStore: GridViewStore<IntegrationRunListDto, IntegrationRunsListRequest, IntegrationRunsListResponse> 
    sp: IntegrationDashboardGridViewStoreParams

    removeSpecificRun = () => {
        const arr = this.sp.history.location.pathname.split('/')
        arr.pop()
        this.sp.history.replace(arr.join('/'))
    }

    onEntityAction = async (action: IntegrationRunAction, integrationRunId?: number) => {
        const currentStatus = this.gridStore.selectedEntity!.status
        switch (action) {
            case 'reprocess':
                this.runIntegration(this.gridStore.selectedEntity!.payload, async () => {
                    await executeLoading(`Updating Reprocessed Run...`, () =>
                        integrationClient.changeIntegrationRunStatus(
                            integrationRunId,
                            currentStatus,
                            IntegrationRunStatusEnum.ErroredButReprocessed))
                    this.removeSpecificRun()
                })
                break;
            case 'mark-as-not-pending':
                await executeLoading(`Marking as 'Not Pending'...`, () =>
                    integrationClient.changeIntegrationRunStatus(
                        integrationRunId,
                        currentStatus,
                        IntegrationRunStatusEnum.ErroredButNotPending))
                break;
            case 'mark-as-pending':
                await executeLoading(`Marking as 'Pending'...`, () =>
                    integrationClient.changeIntegrationRunStatus(
                        integrationRunId,
                        currentStatus,
                        IntegrationRunStatusEnum.Error))
                break;
        }
        this.gridStore.onEntityClose()
    }

    payload?: string
    callbackAfterRun?: () => any
    runIntegration = (payload?: string, callbackAfterRun?: () => any) => {
        this.payload = payload
        this.callbackAfterRun = callbackAfterRun
        this.runIntegrationScreenVisible = true
    }
    integrationRunCallback = async (integrationRan?: boolean) => {
        this.runIntegrationScreenVisible = false
        if (integrationRan) {
            this.callbackAfterRun && await this.callbackAfterRun()
            this.gridStore.reloadTableData()
        }
    }
    runIntegrationScreenVisible = false
    onlyPending = false
    filterChanged = () => {
        this.onlyPending = !this.onlyPending
        this.setActions()
        this.gridStore.reloadTableData()
    }
    setActions() {
        const hasRun = this.sp.globalStore.functionality(FunctionalityEnum.EmsIntegration).hasSubFunctionality(SubFunctionalityEnum.RunIntegration)
        const freeActions: FreeActionProp<IntegrationRunListDto>[] = []

        if (!this.sp.integrationRunId) {
            if (hasRun) {
                freeActions.push({
                    icon: () => <PlayCircleFilledIcon />,
                    callback: () => this.runIntegration(),
                    title: "Run Integration",
                })
            }

            const desc = this.getIntegrationRunStatusDescription(IntegrationRunStatusEnum.Error, this.onlyPending)
            freeActions.push({
                icon: () => <>{desc.iconComponent}</>,
                callback: this.filterChanged,
                title: this.onlyPending ?
                    `(click to remove filter 'Only Errored - Pending' and show all)` :
                    `(click to add filter 'Only Errored - Pending')`
            })
        }

        this.gridStore.setGridActions({
            hasViewAction: true,
            hasCreateAction: false,
            hasEditAction: false,
            actions: { freeActions }
        })
    }

    getIntegrationRunStatusDescription(status: IntegrationRunStatusEnum, toggled = true) {
        let description = ""
        let Icon = null
        let color = ""
        switch (status) {
            case IntegrationRunStatusEnum.Running:
                description = "Running"
                Icon = toggled ? RefreshIcon : RefreshIconOutlined
                color = this.sp.globalStore.theme.palette.primary.main
                break
            case IntegrationRunStatusEnum.Success:
                description = "Successful"
                Icon = toggled ? CheckCircleIcon : CheckCircleIconOutlined
                color = 'green'
                break
            case IntegrationRunStatusEnum.Warning:
                description = "Successful with Warnings"
                Icon = toggled ? CheckCircleIcon : CheckCircleIconOutlined
                color = 'yellow'
                break
            case IntegrationRunStatusEnum.Error:
                description = "Failed - pending action"
                Icon = toggled ? InfoIcon : InfoIconOutlined
                color = this.sp.globalStore.standardTheme.palette.secondary.main
                break
            case IntegrationRunStatusEnum.ErroredButNotPending:
                description = "Failed but not pending "
                Icon = toggled ? CancelIcon : CancelIconOutlined
                color = this.sp.globalStore.theme.palette.primary.main
                break
            case IntegrationRunStatusEnum.ErroredButReprocessed:
                description = "Failed but Reprocessed"
                Icon = toggled ? CheckCircleIcon : CheckCircleIconOutlined
                color = this.sp.globalStore.theme.palette.primary.main
                break
        }

        const iconComponent = <Icon style={{ color }} />
        return {
            description,
            visualIndicator: (
                <Tooltip title={description}>
                    {iconComponent}
                </Tooltip>
            ),
            iconComponent,
        }
    }
}
