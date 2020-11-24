import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import { InputProps } from "shared-do-not-touch/input-props"
import { AutoCompleteField } from "shared-do-not-touch/material-ui-auto-complete"
import { UserViewStoreContext } from '../UserViewStore'
import { UserInWorkflowRoleDto } from 'clients/deal-system-client-definitions'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const UserViewWorkflowRoleCell = observer(({ data } : { data: UserInWorkflowRoleDto }) => {
    
    const rootStore = useContext(UserViewStoreContext)

    const store = useStore(sp => ({
        changed: () => { sp.data.updated = true },
        get workflowRoles() {
            return rootStore.workflowRoles.filter(wr => !rootStore.workflowRolesInUser.find(awr => awr.workflowRoleId.value === wr.id && awr !== sp.data))
        },
        get errorHandler() {
            return rootStore.getErrorHandlerForWorkflowRole(sp.data)
        }
    }), { data })

    return (
        <InputProps stateObject={data} propertyName="workflowRoleId" onValueChanged={store.changed} errorHandler={store.errorHandler}>
            <AutoCompleteField
                disabled={rootStore.sp.readOnly || !!data.id}
                fullWidth
                initialInputValue={data.workflowRoleName}
                dataSource={store.workflowRoles}
            />
        </InputProps>
    )
})