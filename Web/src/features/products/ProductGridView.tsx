import React from 'react'
import { observer } from 'mobx-react-lite'
import { GridView } from 'shared-do-not-touch/grid-view'
import ProductGridViewStore from './ProductGridViewStore'
import ProductViewLoader from './entity-view/ProductViewLoader'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const ProductGridView = () => {

    const store = useStore(() => new ProductGridViewStore())

    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.Products,
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
                title='Products'
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.name!}
                        functionality={FunctionalityEnum.Products}
                    />
                    :
                    <ProductViewLoader
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
export default observer(ProductGridView)