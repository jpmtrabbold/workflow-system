import React, { useContext } from 'react'
import { AuditEntryTableListDto } from 'clients/deal-system-client-definitions'
import { observer } from 'mobx-react-lite'
import Dialog from '@material-ui/core/Dialog'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { auditEntryFieldsGridDefinition } from './AuditEntryFieldsGridDefinition'
import { AuditEntryStoreContext } from 'features/audit-entries/AuditEntryStore'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'

export interface AuditEntryFieldsViewProps {
    auditEntryTable: AuditEntryTableListDto
    onClose: () => any
    title: string
}

export const AuditEntryFieldsView = observer((props: AuditEntryFieldsViewProps) => {
    const store = useContext(AuditEntryStoreContext)
    const windowSize = useWindowSize()
    return (
        <Dialog container={null} open={true} maxWidth='xl' onClose={props.onClose}>
            <DialogTitle>
                {props.title}
            </DialogTitle>
            <DialogContent>
                <CustomTable
                    maxHeight={windowSize.height * 0.55}
                    minHeight={windowSize.height * 0.55}
                    title={`Log entries - Fields`}
                    columns={auditEntryFieldsGridDefinition}
                    rows={props.auditEntryTable.fields}
                    initialSearch={store.tableSearchText}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={store.onFieldViewClose} title="Close" color="primary">
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    )
})