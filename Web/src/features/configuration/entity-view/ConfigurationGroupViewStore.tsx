import React, { createContext } from 'react'
import { ConfigurationGroupDto, ConfigurationEntryDto } from "../../../clients/deal-system-client-definitions"
import { configurationClient } from "../../../clients/deal-system-rest-clients"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { snackbar } from "shared-do-not-touch/material-ui-modals"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { MessageDialogAction } from 'shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog'
import { ConfigurationGroupViewProps } from './ConfigurationGroupView'

export class ConfigurationGroupViewStore {
    constructor(sp: ConfigurationGroupViewProps) {
        this.sourceProps = sp
        
    }

    sourceProps: ConfigurationGroupViewProps

    entity = new ConfigurationGroupDto()
    loaded = false

    onLoad = async () => {
        if (this.sourceProps.entityId) {
            if (!(await this.fetchData(this.sourceProps.entityId))) {
                return this.sourceProps.onEntityClose()
            }
        } else {
            throw new Error("There is no 'Create' on this functionality")
        }
        this.loaded = true
    }

    handleClose = () => {
        this.sourceProps.onEntityClose()
    }

    handleSave = async () => {
        if (await this.saveEntity()) {
            this.handleClose()
        }
    }

    async saveEntity() {
        if (this.validate()) {
            return executeLoading('Saving Configuration Data...', async () => {
                try {
                    const result = await configurationClient.post(this.entity)
                    snackbar({ title: `Configuration Group ${result.name} saved successfully` })
                } catch {
                    return false
                }
                return true
            })
        }
    }

    validate() {
        let ok = true
        return ok
    }

    fetchWorkflowRoles = async () => {
        try {
            //    this.workflowRoles = await userClient.getWorkflowRoles()
        } catch (error) {
            return false
        }
        return true
    }
    fetchSupportingData = async () => {
        const workflowRolesPromise = this.fetchWorkflowRoles()

        if (!await workflowRolesPromise)
            return false

        return true
    }

    async fetchData(entityId?: number) {
        if (!entityId)
            return false

        const userPromise = this.fetchEntity(entityId)
        const supportingDataPromise = this.fetchSupportingData()

        if (!await userPromise || !await supportingDataPromise)
            return false

        return true
    }

    async fetchEntity(entityId: number): Promise<boolean> {
        try {
            this.entity = await configurationClient.get(entityId)
            return true
        } catch {
            return false
        }
    }

    get columns() {
        return [
            {
                title: 'Content', field: 'content', rendererComponent: ({ data }) => null
            },
        ] as Column<ConfigurationEntryDto>[]
    }

    get actions() {
        const actions = [] as MessageDialogAction[]
        if (this.sourceProps.readOnly) {
            actions.push({
                name: "Close",
                callback: this.handleClose,
                color: 'primary'
            })
        } else {
            actions.push({
                name: "Cancel",
                callback: this.handleClose,
                color: 'primary'
            })
            actions.push({
                name: "Ok",
                callback: this.handleSave,
                color: 'primary',
                title: 'Saves the Configuration data'
            })
        }
        return actions
    }

}
//export const ConfigurationGroupViewStoreContext = createContext(new ConfigurationGroupViewStore({ visible: true, onEntityClose: () => null }))
export const ConfigurationGroupViewStoreContext = createContext(null) as unknown as React.Context<ConfigurationGroupViewStore>