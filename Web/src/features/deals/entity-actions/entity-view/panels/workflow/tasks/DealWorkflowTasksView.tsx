// ** Common
import React from 'react'
import { observer } from 'mobx-react-lite'
import Table from '@material-ui/core/Table'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import TableBody from '@material-ui/core/TableBody'
import Checkbox from '@material-ui/core/Checkbox'
import Paper from '@material-ui/core/Paper'
import Toolbar from '@material-ui/core/Toolbar'
import Typography from '@material-ui/core/Typography'
import makeStyles from "@material-ui/core/styles/makeStyles"
import DealWorkflowTaskRow from './DealWorkflowTaskRow'
import { DealWorkflowTaskWithChildren } from '../DealWorkflowStore'

const useStyles = makeStyles((theme) => ({
    checkboxCell: {
        width: theme.spacing(3)
    },
    paper: {
        minHeight: theme.spacing(37)
    }
}))
interface DealWorkflowTasksViewProps {
    tasks?: DealWorkflowTaskWithChildren[]
    title?: string
    readOnly?: boolean
}
const DealWorkflowTasksView = (props: DealWorkflowTasksViewProps) => {
    const classes = useStyles()
    const { tasks, title, readOnly } = props

    return (
        <Paper className={classes.paper}>
            {!!title && (
                <Toolbar>
                    <Typography variant="subtitle1" id="tableTitle">{title}</Typography>
                </Toolbar>
            )}
            <Table size='small' aria-labelledby="tableTitle">
                <TableHead>
                    <TableRow>
                        <TableCell>Task</TableCell>
                        <TableCell className={classes.checkboxCell} align='center'>Done</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        (!tasks || tasks.length === 0)
                            ?
                            <TableRow>
                                <TableCell>{(!tasks
                                    ? "Please click on a status history entry to see its tasks"
                                    : "There are no tasks for this action.")}
                                </TableCell>
                                <TableCell className={classes.checkboxCell} align='center'>
                                    <Checkbox checked={true} disabled={true} />
                                </TableCell>
                            </TableRow>
                            :
                            tasks.map(item => <DealWorkflowTaskRow readOnly={readOnly} key={item.task.id} taskWithChildren={item} />)
                    }
                </TableBody>
            </Table>
        </Paper>
    )
}

export default observer(DealWorkflowTasksView)