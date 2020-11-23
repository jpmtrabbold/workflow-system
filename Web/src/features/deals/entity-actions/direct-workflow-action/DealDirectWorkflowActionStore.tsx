import { publicClient } from "clients/deal-system-rest-clients";
import { DealDirectWorkflowActionRequest } from "clients/deal-system-client-definitions";
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading";
import { messageActions } from "shared-do-not-touch/material-ui-modals";
import FormErrorHandler from "shared-do-not-touch/input-props/form-error-handler";
import { action } from "mobx";

export interface DealDirectWorkflowActionQuery {
    dealId: string
    userId: string
    key: string
    actionId: string
    actionName: string
}

export class DealDirectWorkflowActionStore {
    constructor(sp: DealDirectWorkflowActionQuery) {
        this.sp = sp
    }
    sp: DealDirectWorkflowActionQuery
    errorHandler = new FormErrorHandler<DealDirectWorkflowActionStore>()

    reason = ""
    fillReason = false
    dealNumber = ""

    async carryOutAction() {
        executeLoading(`Confirming ${this.sp.actionName} action...`, action(async () => {
            const request = new DealDirectWorkflowActionRequest()

            request.actionId = parseInt(this.sp.actionId)
            request.actionName = this.sp.actionName
            request.key = this.sp.key
            request.userId = parseInt(this.sp.userId)
            request.dealId = parseInt(this.sp.dealId)
            request.reason = this.reason

            try {
                var response = await publicClient.dealDirectWorkflowAction(request)
                this.dealNumber = response.dealNumber
            } catch (error) {
                return
            }

            if (response.success) {
                await messageActions({
                    title: "Success - Deal " + this.dealNumber,
                    content: response.messages.reduce((prev, curr) => prev + "\n\n" + curr, "") || `'${this.sp.actionName}' action performed successfully. You can close this tab.`,
                    actions: [{ name: "Close Tab" }]
                })
                window.close()
            } else {
                if (response.reasonIsRequired) {
                    this.fillReason = true
                } else {
                    await messageActions({
                        title: "Error - Deal " + this.dealNumber,
                        content: response.messages.reduce((prev, curr) => prev + "\n\n" + curr, "") || `'${this.sp.actionName}' could not be performed.`,
                        actions: [{ name: "Close Tab" }]
                    })
                    window.close()
                }
            }

        }))
    }

    carryOutActionWithReason = () => {
        if (!this.reason) {
            this.errorHandler.reset()
            this.errorHandler.error('reason', "Please provide a reason")
            return
        }
        this.carryOutAction()
    }
}