import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { UserViewStoreContext } from '../UserViewStore'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'

import AddIcon from '@material-ui/icons/Add'
import DeleteIcon from '@material-ui/icons/DeleteOutlined'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const UserWorkflowRolesView = observer(() => {
    const store = useContext(UserViewStoreContext)
    const localStore = useStore(() => ({
        get actions() {
            return (!store.sp.readOnly && {
                freeActions: [{ title: 'Add', callback: store.AddWorkflowRole, icon: () => <AddIcon /> }],
                multipleRowActions: [{ title: 'Delete', callback: store.DeleteWorkflowRoles, icon: () => <DeleteIcon /> }],
            }) || undefined
        }
    }))

    return (
        <CustomTable
            title='Workflow Roles'
            rows={store.workflowRolesInUser}
            columns={store.columns}
            searchable={false}
            pagination={false}
            actions={localStore.actions}
            selectable={!store.sp.readOnly}
            showSelectAllCheckbox={!store.sp.readOnly}
        />
    )
})