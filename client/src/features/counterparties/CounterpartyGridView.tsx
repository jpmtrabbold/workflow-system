import React from 'react'
import { observer } from 'mobx-react-lite'
import CounterpartyGridViewStore from './CounterpartyGridViewStore'
import { GridView } from 'shared-do-not-touch/grid-view'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import CounterpartyView from './entity-view/CounterpartyView'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import { useParams, useHistory } from 'react-router-dom'
import Typography from '@material-ui/core/Typography/Typography'
import { useTheme } from '@material-ui/core/styles'
import Chip from '@material-ui/core/Chip'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'


const CounterpartyGridView = () => {
    const { id } = useParams<{id?: string}>()
    const history = useHistory()
    const store = useStore(sp => new CounterpartyGridViewStore(sp), { id, history })
    const theme = useTheme()
    
    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.Counterparties,
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
                title={<>
                    <Typography variant='h6'>
                        Counterparties
                    </Typography>
                    {store.isSpecific && (
                        <Chip
                            style={{ marginLeft: theme.spacing(3) }}
                            label={`${store.specificCounterpartyIdentifier}`}
                            onDelete={store.removeSpecific}
                            color="primary"
                        />
                    )}
                </>}
            />
            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.name!}
                        functionality={FunctionalityEnum.Counterparties}
                    />
                    :
                    <CounterpartyView
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

export default observer(CounterpartyGridView)