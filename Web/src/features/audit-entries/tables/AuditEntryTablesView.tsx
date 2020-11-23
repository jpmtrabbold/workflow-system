import React, { useContext } from 'react'
import { AuditEntryListDto } from 'clients/deal-system-client-definitions'
import { observer } from 'mobx-react-lite'
import Dialog from '@material-ui/core/Dialog'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { auditEntryTablesGridDefinition } from './AuditEntryTablesGridDefinition'
import { AuditEntryFieldsView } from './fields/AuditEntryFieldsView'
import { AuditEntryStoreContext } from '../AuditEntryStore'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'

export interface AuditEntryTablesViewProps {
    auditEntry: AuditEntryListDto
    onClose: () => any
    title: string
}

export const AuditEntryTablesView = observer((props: AuditEntryTablesViewProps) => {
    const store = useContext(AuditEntryStoreContext)
    const windowSize = useWindowSize()
    return (
        <Dialog container={null} open={true} maxWidth='xl' onClose={props.onClose}>
            <DialogTitle>
                {props.title}
            </DialogTitle>
            <DialogContent>
                <CustomTable
                    maxHeight={windowSize.height * 0.6}
                    minHeight={windowSize.height * 0.6}
                    title={`Log entries - Table rows`}
                    columns={auditEntryTablesGridDefinition}
                    rows={props.auditEntry.tables}
                    onRowClick={store.onAuditTableRowClick}
                    customLocalSearch={store.tableSearch}
                    initialSearch={store.entrySearchText}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose} title="Close" color="primary">
                    Close
                </Button>
            </DialogActions>
            {!!store.auditEntryTableSelected && (
                <AuditEntryFieldsView
                    onClose={store.onTableViewClose}
                    auditEntryTable={store.auditEntryTableSelected!}
                    title={props.title + " - table: " + store.auditEntryTableSelected.tableName}
                />
            )}
        </Dialog>
    )
})