// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import Checkbox from '@material-ui/core/Checkbox'
import { DealViewStoreContext } from '../../../DealViewStore'
import Link from '@material-ui/core/Link'
import { WorkflowStatusAssignmentOption } from '../DealWorkflowStore'
import CheckIcon from '@material-ui/icons/Check'
import CancelIcon from '@material-ui/icons/Cancel'
import Tooltip from '@material-ui/core/Tooltip'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface DealWorkflowActionAssignmentRowProps {
    assignment: WorkflowStatusAssignmentOption
    withSelection: boolean
    showsTrader?: boolean
}

export const DealWorkflowActionAssignmentRow = observer(({ assignment, withSelection, showsTrader }: DealWorkflowActionAssignmentRowProps) => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    const localStore = useStore(source => ({
        onChange(event: React.ChangeEvent<HTMLInputElement>, checked: boolean) {
            store.handleAssignmentCheck(source.assignment, checked)
        },
        onClick() {
            store.showTradingPolicyForAssignment(source.assignment)
        }
    }), { assignment })

    return (
        <TableRow>
            {!!withSelection && (
                <TableCell style={{ width: '30px' }} align='center'>
                    <Checkbox
                        disabled={!assignment.enabledForSelection}
                        checked={assignment.checked}
                        onChange={localStore.onChange}
                    />
                </TableCell>
            )}

            <TableCell>
                {assignment.name}
            </TableCell>
            <TableCell>
                {!assignment.enabledForSelection ? "N/A" : (
                    <Link
                        onClick={localStore.onClick}
                        style={{ cursor: 'pointer' }}>
                        {assignment.meetsTradingPolicy ? "Yes" : "No"}
                    </Link>
                )}
            </TableCell>
            {!!showsTrader && (
                <TableCell>
                    {assignment.isTraderWorkflowLevel && (
                        <Tooltip title={`This is the trader authority role, and it ${assignment.meetsTradingPolicy ? 'meets': 'does not meet'} the trading policy.`}>
                            {assignment.meetsTradingPolicy ? <CheckIcon style={{ color: 'green' }} /> : <CancelIcon style={{ color: 'red' }} />}
                        </Tooltip>
                    )}
                </TableCell>
            )}
        </TableRow>
    )
})