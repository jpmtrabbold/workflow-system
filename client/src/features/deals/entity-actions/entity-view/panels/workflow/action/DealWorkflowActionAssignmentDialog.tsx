// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import Table from '@material-ui/core/Table'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import TableBody from '@material-ui/core/TableBody'
import Paper from '@material-ui/core/Paper'
import Toolbar from '@material-ui/core/Toolbar'
import Typography from '@material-ui/core/Typography'
import { DealViewStoreContext } from '../../../DealViewStore'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { DealWorkflowActionAssignmentList } from '../assignments/DealWorkflowActionAssignmentList'
import { DealWorkflowActionAssignmentPolicy } from '../assignments/DealWorkflowActionAssignmentPolicy'
import { DealWorkflowActionTermOverrideDialog } from '../assignments/DealWorkflowActionTermOverrideDialog'

const DealWorkflowAssignmentView = () => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    return (
        <>
            <MessageDialog
                fullWidth
                maxWidth='sm'
                scroll='paper'
                title="Workflow Assignment"
                actions={[{
                    name: "Close",
                    color: "primary",
                    callback: store.handleCloseAssignment,
                }]}
                onClose={store.handleCloseAssignment}
                open={store.assignmentDialogVisible}
            >
                <Paper style={{ minHeight: 300 }}>
                    <Toolbar>
                        <Typography variant="subtitle1" id="assignmentTableTitle">
                            {store.assignmentTableTitle}
                        </Typography>
                    </Toolbar>
                    <Table size='small' aria-labelledby="assignmentTableTitle">
                        <TableHead>
                            <TableRow>
                                <TableCell style={{ width: '30px' }} align='center'>Selected</TableCell>
                                <TableCell>Description</TableCell>
                                <TableCell>Meets Policy?</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <DealWorkflowActionAssignmentList withSelection />
                        </TableBody>
                    </Table>
                </Paper>
            </MessageDialog>
            <DealWorkflowActionAssignmentPolicy />
            <DealWorkflowActionTermOverrideDialog />
        </>
    )
}

export default observer(DealWorkflowAssignmentView)