// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import { DealViewStoreContext } from '../../../DealViewStore'
import { DealWorkflowActionAssignmentRow } from './DealWorkflowActionAssignmentRow'
import { WorkflowStatusAssignmentOption } from '../DealWorkflowStore'

interface DealWorkflowActionAssignmentListProps {
    withSelection: boolean
    showsTrader?: boolean
    alternativeAssignments?: WorkflowStatusAssignmentOption[]
}

export const DealWorkflowActionAssignmentList = observer((props: DealWorkflowActionAssignmentListProps) => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    const assignments = props.alternativeAssignments || store.assignments
    return (
        <>
            {assignments.length > 0
                ?
                assignments.map((assignment, index) => <DealWorkflowActionAssignmentRow {...props} key={index} assignment={assignment}/>)
                :
                <TableRow>
                   <TableCell colSpan={10}>There are no possible assignments. (unexpected behaviour)</TableCell>
                </TableRow>
            }
        </>
    )
})