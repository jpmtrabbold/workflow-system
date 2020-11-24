// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import Table from '@material-ui/core/Table'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import TableBody from '@material-ui/core/TableBody'
import Paper from '@material-ui/core/Paper'
import { DealViewStoreContext } from '../../../DealViewStore'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { DealWorkflowActionAssignmentList } from '../assignments/DealWorkflowActionAssignmentList'
import { DealWorkflowActionAssignmentPolicy } from '../assignments/DealWorkflowActionAssignmentPolicy'
import { DealWorkflowActionTermOverrideDialog } from '../assignments/DealWorkflowActionTermOverrideDialog'

export const DealWorkflowTradingPolicyAssessmentDialog = observer(() => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore
    
    return (
        <>
            <MessageDialog
                fullWidth
                maxWidth='sm'
                scroll='paper'
                title="Trading Policy Assessment"
                actions={[{
                    name: "Close",
                    color: "primary",
                    callback: store.handleCloseTradingPolicyAssessmentDialog,
                }]}
                onClose={store.handleCloseTradingPolicyAssessmentDialog}
                open={store.tradingPolicyAssessmentDialogVisible}
            >
                <Paper style={{ minHeight: 300 }}>
                    <Table size='small' aria-labelledby="assignmentTableTitle">
                        <TableHead>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>Meets Policy?</TableCell>
                                <TableCell>Trader Authority</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <DealWorkflowActionAssignmentList withSelection={false} showsTrader alternativeAssignments={store.tradingPolicyAssessmentLevels}/>
                        </TableBody>
                    </Table>
                </Paper>
            </MessageDialog>
            <DealWorkflowActionAssignmentPolicy />
            <DealWorkflowActionTermOverrideDialog />
        </>
    )
})