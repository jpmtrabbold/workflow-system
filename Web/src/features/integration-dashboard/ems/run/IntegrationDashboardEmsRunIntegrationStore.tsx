import { computed } from "mobx";
import moment from "moment"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { EmsFetchRequest } from "clients/deal-system-client-definitions";
import { dealIntegrationClient } from "clients/deal-system-rest-clients";
import { IntegrationDashboardStoreRunIntegrationProps } from "features/integration-dashboard/IntegrationDashboardStoreInterface";
import { MessageDialogAction } from "shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog";
import FormErrorHandler from "shared-do-not-touch/input-props/form-error-handler";

export default class IntegrationDashboardEmsRunIntegrationStore {
    constructor(sp: IntegrationDashboardStoreRunIntegrationProps) {
        this.sp = sp
        if (this.sp.payload) {
            const fetchRequest = JSON.parse(this.sp.payload)
            if (fetchRequest.StartCreationDateTime) this.startCreationDate = moment(fetchRequest.StartCreationDateTime)
            if (fetchRequest.EndCreationDateTime) this.endCreationDate = moment(fetchRequest.EndCreationDateTime)
        }
    }
    sp: IntegrationDashboardStoreRunIntegrationProps

    get actions(): MessageDialogAction[] {
        return [
            {
                name: "Run Integration",
                callback: this.run,
                color: 'primary',
            },
            {
                name: "Cancel",
                callback: this.sp.callbackClose,
                color: 'primary',
            },
        ]
    }

    get DisabledParams() {
        return !!this.sp.payload
    }

    errorHandler = new FormErrorHandler<IntegrationDashboardEmsRunIntegrationStore>()
    startCreationDate? = moment()
    endCreationDate? = moment()
    run = () => {
        this.errorHandler.reset()
        if (!this.startCreationDate) {
            this.errorHandler.error('startCreationDate', 'Mandatory')
        }
        if (!this.endCreationDate) {
            this.errorHandler.error('endCreationDate', 'Mandatory')
        }
        if (this.errorHandler.hasError) {
            return
        }
        executeLoading('Executing Integration...', async () => {
            const request = new EmsFetchRequest({
                startCreationDateTime: this.startCreationDate!.startOf('day'),
                endCreationDateTime: this.endCreationDate!.endOf('day')
            })
            await dealIntegrationClient.emsFetch(request)
            this.sp.callbackClose && this.sp.callbackClose(true)
        })
    }
}