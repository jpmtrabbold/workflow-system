import { IntegrationDashboardEntriesViewProps } from "./IntegrationDashboardEntriesView";
import { observable } from "mobx";
import { MessageDialogAction } from "shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog";
import { IntegrationRunEntryDto, FunctionalityEnum, SubFunctionalityEnum, IntegrationRunStatusEnum } from "clients/deal-system-client-definitions";
import { integrationClient } from "clients/deal-system-rest-clients";
import { GlobalStore } from "features/shared/stores/GlobalStore";

export default class IntegrationDashboardEntriesViewStore {
    constructor(sp: IntegrationDashboardEntriesViewProps & { globalStore: GlobalStore }) {
        this.sp = sp
        this.getRows()
    }
    sp: IntegrationDashboardEntriesViewProps & { globalStore: GlobalStore }
    onClose = () => {
        this.sp.callbackClose('close')
    }
    reprocess = () => {
        this.sp.callbackClose('reprocess', this.sp.integrationRun.id)
    }
    markAsNotPending = () => {
        this.sp.callbackClose('mark-as-not-pending', this.sp.integrationRun.id)
    }
    markAsPending = () => {
        this.sp.callbackClose('mark-as-pending', this.sp.integrationRun.id)
    }
    get actions() {
        const actions = [] as MessageDialogAction[]
        if (this.sp.globalStore.functionality(FunctionalityEnum.EmsIntegration).hasSubFunctionality(SubFunctionalityEnum.ReprocessIntegration)) {
            if (this.sp.integrationRun.status === IntegrationRunStatusEnum.Error) {
                actions.push({
                    name: "Reprocess",
                    callback: this.reprocess,
                    color: 'primary',
                })
                actions.push({
                    name: "Not Pending",
                    title: `Mark this Integration Run as 'Not Pending'`,
                    callback: this.markAsNotPending,
                    color: 'primary',
                })
            } else if (this.sp.integrationRun.status === IntegrationRunStatusEnum.ErroredButNotPending) {
                actions.push({
                    name: "Pending",
                    title: `Mark this Integration Run as 'Pending'`,
                    callback: this.markAsPending,
                    color: 'primary',
                })
            }
        }
        actions.push({
            name: "Cancel",
            callback: this.onClose,
            color: 'primary',
        })
        return actions
    }
    rows: IntegrationRunEntryDto[] = []
    loading = false
    getRows = async () => {
        this.loading = true
        setTimeout(async () => {
            this.rows = await integrationClient.getEntries(this.sp.integrationRun.id)
            this.loading = false
        });
    }

    static getFunctionalityDescription(functionality: FunctionalityEnum) {
        switch (functionality) {
            case FunctionalityEnum.Deals:
                return "Deal"
        }
    }
}
