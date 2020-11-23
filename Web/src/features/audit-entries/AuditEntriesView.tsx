import React from 'react'
import { observer } from 'mobx-react-lite'
import AuditEntryStore, { AuditEntryStoreContext } from './AuditEntryStore'
import Dialog from '@material-ui/core/Dialog'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import { auditEntriesGridDefinition } from './AuditEntryGridDefinition'
import { FunctionalityEnum } from 'clients/deal-system-client-definitions'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { AuditEntryTablesView } from './tables/AuditEntryTablesView'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface AuditEntryGridViewProps {
    onClose: () => any
    entityId: number
    entityName: string
    functionality: FunctionalityEnum
}

export const AuditEntryGridView = observer((props: AuditEntryGridViewProps) => {
    const store = useStore(source => new AuditEntryStore(source), props)
    const windowSize = useWindowSize()
    return (
        <AuditEntryStoreContext.Provider value={store}>
            <Dialog container={null} open={true} maxWidth='xl' onClose={props.onClose}>
                <DialogTitle>
                    {`${props.entityName}`}
                </DialogTitle>
                <DialogContent>
                    <CustomTable
                        maxHeight={windowSize.height * 0.65}
                        minHeight={windowSize.height * 0.65}
                        title={`Log entries`}
                        columns={auditEntriesGridDefinition}
                        rows={store.rows}
                        remoteDataFetchRequest={store.remoteDataFetchRequest}
                        totalCount={store.totalCount}
                        onRowClick={store.onAuditEntryRowClick}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onClose} title="Close" color="primary">
                        Close
                </Button>
                </DialogActions>
                {!!store.auditEntrySelected && (
                    <AuditEntryTablesView onClose={store.onTableViewClose} auditEntry={store.auditEntrySelected!} title={props.entityName} />
                )}
            </Dialog>
        </AuditEntryStoreContext.Provider>
    )
})