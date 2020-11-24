import React from 'react'
import { observer } from 'mobx-react-lite'
import DealTypeGridViewStore from './DealTypeGridViewStore'
import { GridView } from 'shared-do-not-touch/grid-view'
import DealTypeViewLoader from './entity-view/DealTypeViewLoader'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const DealTypeGridView = () => {

    const store = useStore(() => new DealTypeGridViewStore())

    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.DealTypes,
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
                title='Deal Types'
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.name!}
                        functionality={FunctionalityEnum.DealTypes}
                    />
                    :
                    <DealTypeViewLoader
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
export default observer(DealTypeGridView)