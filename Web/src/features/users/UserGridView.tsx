import React from 'react'
import { observer } from 'mobx-react-lite'
import { GridView } from 'shared-do-not-touch/grid-view'
import UserGridViewStore from './UserGridViewStore'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum, UserListDto } from '../../clients/deal-system-client-definitions'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import UserView from './entity-view/UserView'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import useTheme from '@material-ui/core/styles/useTheme'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface UserGridViewProps {
    selectionOnly?: boolean
    selectionCallback?: (entity: UserListDto) => any
    title?: string
    onlyUsersWithLevel?: boolean
}
const UserGridView = (props: UserGridViewProps) => {

    const store = useStore(sp => new UserGridViewStore(sp), props)
    const windowSize = useWindowSize()
    const theme = useTheme()

    useSetUserPermissions({
        selectionOnly: props.selectionOnly,
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.Users,
        setActionsProps: (actionProps, functionality) => {
            actionProps.actions = { rowActions: [] }
            if (props.selectionOnly && props.selectionCallback) {
                actionProps.actions!.rowActions!.push({
                    title: 'Select',
                    callback: (data) => props.selectionCallback!(data),
                    hasPriority: true
                })
            }
            if (functionality.hasSubFunctionality(SubFunctionalityEnum.AuditLogsView)) {
                actionProps.actions!.rowActions!.push({
                    title: 'Audit Log',
                    callback: (data) => store.gridStore.onActionClick("audit-log", data)
                })
            }
        }
    })


    return (
        <>
            <GridView
                store={store.gridStore}
                title={props.title ?? 'Users'}
                height={(props.selectionOnly && (windowSize.height - theme.spacing(25))) || undefined}
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.name!}
                        functionality={FunctionalityEnum.Users}
                    />
                    :
                    <UserView
                        readOnly={store.gridStore.entityActionType === 'view'}
                        visible={store.gridStore.entityActionActive}
                        onEntityClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity && store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity && store.gridStore.selectedEntity!.name!}
                    />
            )}
        </>
    )
}
export default observer(UserGridView)