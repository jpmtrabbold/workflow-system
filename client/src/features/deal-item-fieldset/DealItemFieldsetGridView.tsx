import React from 'react'
import { observer } from 'mobx-react-lite'
import DealItemFieldsetGridViewStore from './DealItemFieldsetGridViewStore'
import { GridView } from 'shared-do-not-touch/grid-view'
import DealItemFieldsetViewLoader from './entity-view/DealItemFieldsetViewLoader'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const DealItemFieldsetGridView = () => {

    const store = useStore(() => new DealItemFieldsetGridViewStore())

    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.DealItemFieldsets,
        setActionsProps: (actionProps, functionality) => {
            actionProps.actions = { rowActions: [] }
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
                title='Deal Item Fieldsets'
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.name!}
                        functionality={FunctionalityEnum.DealItemFieldsets}
                    />
                    :
                    <DealItemFieldsetViewLoader
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
export default observer(DealItemFieldsetGridView)