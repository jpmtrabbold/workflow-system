import React from 'react'
import { LookupRequest, UserDto, UserInWorkflowRoleDto } from "../../../clients/deal-system-client-definitions"
import { userClient } from "../../../clients/deal-system-rest-clients"
import { createContext, useCallback } from "react"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { snackbar } from "shared-do-not-touch/material-ui-modals"
import { observer } from "mobx-react-lite"
import { InputProps } from "shared-do-not-touch/input-props"
import { YesNoSelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { Theme } from '@material-ui/core/styles'
import { UserViewProps } from './UserView'
import { UserViewWorkflowRoleCell } from './divisions/UserViewWorkflowRoleCell'
import FormErrorHandler from 'shared-do-not-touch/input-props/form-error-handler'
import { theme } from 'shared-do-not-touch/theme/theme'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import { MessageDialogAction } from 'shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog'

export class UserViewStore {
    constructor(sp: UserViewProps, theme: Theme) {
        this.sp = sp
        this.theme = theme
    }

    sp: UserViewProps
    theme: Theme

    workflowRolesErrorHandlers = [] as { workflowRoleInUser: UserInWorkflowRoleDto, errorHandler: FormErrorHandler<any> }[]

    getErrorHandlerForWorkflowRole(workflowRoleInUser: UserInWorkflowRoleDto) {
        return this.workflowRolesErrorHandlers.find(w => w.workflowRoleInUser === workflowRoleInUser)?.errorHandler
    }

    creation = true

    user = new UserDto()
    loaded = false
    workflowRoles: LookupRequest[] = []
    userRoles: LookupRequest[] = []

    onLoad = async () => {
        if (this.sp.entityId) {
            if (!(await this.fetchData(this.sp.entityId))) {
                return this.sp.onEntityClose()
            }
        } else {
            if (!(await this.setupNewUser())) return this.sp.onEntityClose()
        }
        this.loaded = true
    }

    handleClose = () => {
        this.sp.onEntityClose()
    }

    handleSave = async () => {
        if (await this.saveUser()) {
            this.handleClose()
        }
    }

    async saveUser() {
        if (this.validateUser()) {
            return executeLoading('Saving user...', async () => {
                try {
                    const result = await userClient.post(this.user)
                    snackbar({ title: `User ${result.name} saved successfully` })
                } catch {
                    return false
                }
                return true
            })
        }
    }

    validateUser() {
        let ok = true
        for (const workflowRoleInUser of this.workflowRolesInUser) {
            let errorHandler = this.getErrorHandlerForWorkflowRole(workflowRoleInUser)
            if (!errorHandler) {
                errorHandler = new FormErrorHandler<any>()
                this.workflowRolesErrorHandlers.push({ workflowRoleInUser, errorHandler })
            } else {
                errorHandler.reset()
            }

            if (!workflowRoleInUser.workflowRoleId.value) {
                errorHandler.error('workflowRoleId', "Can't be empty")
                ok = false
            }

            if (workflowRoleInUser.active.value === undefined || workflowRoleInUser.active.value === null) {
                errorHandler.error('active', "Can't be empty")
                ok = false
            }
        }
        return ok
    }

    fetchWorkflowRoles = async () => {
        try {
            this.workflowRoles = await userClient.getWorkflowRoles()
        } catch (error) {
            return false
        }
        return true
    }

    fetchUserRoles = async () => {
        try {
            this.userRoles = await userClient.getUserRoles()
        } catch (error) {
            return false
        }
        return true
    }

    fetchSupportingData = async () => {
        const workflowRolesPromise = this.fetchWorkflowRoles()
        const userRolesPromise = this.fetchUserRoles()

        if (!await workflowRolesPromise || !await userRolesPromise)
            return false

        return true
    }

    async fetchData(userId?: number) {
        if (!userId)
            return false

        this.creation = false
        const userPromise = this.fetchUser(userId)
        const supportingDataPromise = this.fetchSupportingData()

        if (!await userPromise || !await supportingDataPromise)
            return false

        this.loaded = true

        return true
    }

    async setupNewUser() {

        this.creation = true
        this.user = new UserDto()
        initializeUpdatable(this.user.name, '')
        initializeUpdatable(this.user.username, '')
        initializeUpdatable(this.user.userRoleId, undefined)
        initializeUpdatable(this.user.active, false)

        if (!await this.fetchSupportingData())
            return false

        return true
    }

    async fetchUser(userId: number): Promise<boolean> {
        try {
            this.user = await userClient.get(userId)
            return true
        } catch {
            return false
        }
    }

    get workflowRolesInUser() {
        return this.user.workflowRolesInUser.filter(wru => !wru.deleted)
    }

    get columns() {
        return [
            {
                title: 'Role', field: 'workflowRoleName', rendererComponent: UserViewWorkflowRoleCell
            },
            {
                title: 'Active', field: 'active', rendererComponent: observer(({ data }) => {
                    const changed = useCallback(() => { data.updated = true }, [data])

                    return (
                        <InputProps stateObject={data} propertyName="active" config={{ isCheckbox: false }} onValueChanged={changed} errorHandler={this.getErrorHandlerForWorkflowRole(data)}>
                            <YesNoSelectWithLabel
                                fullWidth
                                disabled={this.sp.readOnly}
                            />
                        </InputProps>
                    )
                })
            },
        ] as Column<UserInWorkflowRoleDto>[]
    }

    AddWorkflowRole = () => {
        const possibleRoles = this.workflowRoles.filter(r => !this.workflowRolesInUser.find(wru => wru.workflowRoleId.value === r.id))
        if (possibleRoles.length === 0) {
            messageInfo({ title: "All Roles Taken", content: "All the possible workflows are already added to this user." })
            return
        }
        const newWorkflowRoleInUser = new UserInWorkflowRoleDto()
        initializeUpdatable(newWorkflowRoleInUser.active, true)
        initializeUpdatable(newWorkflowRoleInUser.workflowRoleId, possibleRoles[0].id)
        newWorkflowRoleInUser.workflowRoleName = possibleRoles[0].name
        this.user.workflowRolesInUser.push(newWorkflowRoleInUser)
    }

    DeleteWorkflowRoles = (data: UserInWorkflowRoleDto[]) => {
        for (const roleToBeDeleted of data) {
            roleToBeDeleted.deleted = true
        }
    }

    get actions() {
        const actions = [] as MessageDialogAction[]
        if (this.sp.readOnly) {
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
                title: 'Saves the current User'
            })
        }
        return actions
    }

}

export const UserViewStoreContext = createContext(new UserViewStore({ visible: true, onEntityClose: () => null }, theme))