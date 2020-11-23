import React, { useContext } from 'react'
import DealView from './entity-actions/entity-view/DealView'
import { observer } from 'mobx-react-lite'
import DealGridViewStore, { DealGridViewStoreProps } from './DealGridViewStore'
import { GridView } from 'shared-do-not-touch/grid-view'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import dealSheetPrint from './entity-actions/deal-sheet-print/deal-sheet-print'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import { useParams, useHistory } from 'react-router-dom'
import { GlobalStoreContext } from 'features/shared/stores/GlobalStore'
import useTheme from '@material-ui/core/styles/useTheme'
import useQueryString from 'custom-hooks/useQueryString'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'


interface DealGridViewProps {
    assignedToMe?: boolean
}

const DealGridView = (props: DealGridViewProps) => {
    
    const { dealId } = useParams<{ dealId?: string }>()
    const parsedQuery = useQueryString()

    const history = useHistory()
    const globalStore = useContext(GlobalStoreContext)
    const theme = useTheme()

    const store = useStore(sp => new DealGridViewStore(sp), {
        assignedToMe: !!props.assignedToMe,
        dealId,
        subFunctionalityId: parsedQuery.subFunctionalityId,
        globalStore,
        history,
        theme,
    } as DealGridViewStoreProps)

    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.Deals,
        setActionsProps: (actionProps, functionality) => {

            actionProps.actions = { rowActions: [] }
            if (functionality.hasSubFunctionality(SubFunctionalityEnum.PDFExport)) {
                actionProps.actions!.rowActions!.push({
                    title: 'Export PDF',
                    callback: (data) => dealSheetPrint(data.id, data.dealNumber),
                })
            }

            if (functionality.hasSubFunctionality(SubFunctionalityEnum.AuditLogsView)) {
                actionProps.actions!.rowActions!.push({
                    title: 'Audit Log',
                    callback: (data) => store.gridStore.onActionClick("audit-log", data)
                })
            }

            if (functionality.hasSubFunctionality(SubFunctionalityEnum.ExecuteDeal)) {
                actionProps.actions!.rowActions!.push(deal => {
                    if (deal.isExecutionStatus) {
                        if (deal.executed) {
                            return {
                                title: 'Reverse Execution',
                                callback: (data) => store.reverseDealExecution(data)
                            }
                        } else {
                            return {
                                title: 'Execute',
                                callback: (data) => store.execute(data)
                            }
                        }
                    }
                })
            }
        }
    })

    return (
        <>
            <GridView
                store={store.gridStore}
                title={store.gridTitle}
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity!.dealNumber!}
                        functionality={FunctionalityEnum.Deals}
                    />
                    :
                    <DealView
                        visible={store.gridStore.entityActionActive}
                        onEntityClose={store.gridStore.onEntityClose}
                        dealId={store.gridStore.selectedEntity && store.gridStore.selectedEntity.id}
                        dealNumber={store.gridStore.selectedEntity && store.gridStore.selectedEntity.dealNumber}
                        readOnly={store.gridStore.entityActionType === 'view'}
                        execution={store.gridStore.entityActionType === 'execute'}
                    />

            )}
        </>
    )
}

export default observer(DealGridView)