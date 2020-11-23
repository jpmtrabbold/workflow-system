import React from 'react'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { observer } from 'mobx-react-lite'
import UserGridView from './UserGridView'
import { UserListDto } from 'clients/deal-system-client-definitions'
import { MessageDialogAction } from 'shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface UserGridViewForSelectionProps {
    selectionCallback: (entity?: UserListDto) => any,
    title?: string,
    onlyUsersWithLevel?: boolean
}
export const UserGridViewForSelection = observer((props: UserGridViewForSelectionProps) => {

    const store = useStore(sp => ({
        get actions(): MessageDialogAction[] {
            return [
                {
                    name: "Close",
                    callback: () => sp.selectionCallback()
                }
            ]
        }
    }), props)

    return (
        <MessageDialog actions={store.actions} open={true} maxWidth='xl' onClose={props.selectionCallback}>
            <UserGridView
                selectionOnly
                selectionCallback={props.selectionCallback}
                title={props.title ?? "User Selection"}
                onlyUsersWithLevel={props.onlyUsersWithLevel}
            />
        </MessageDialog>
    )
})