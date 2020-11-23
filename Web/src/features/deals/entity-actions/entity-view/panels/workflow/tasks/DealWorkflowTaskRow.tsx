// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import { DealViewStoreContext } from '../../../DealViewStore'
import { DealWorkflowTaskWithChildren } from '../DealWorkflowStore'
import makeStyles from "@material-ui/core/styles/makeStyles"
import DealWorkflowTaskDoneCheck from './DealWorkflowTaskDoneCheck'
import DealWorkflowTaskField from './DealWorkflowTaskField'
import SubdirectoryArrowRightIcon from '@material-ui/icons/SubdirectoryArrowRight'
import Grid from '@material-ui/core/Grid'
import DealWorkflowTaskRowChildren from './DealWorkflowTaskRowChildren'

const useStyles = makeStyles(theme => ({
    checkboxCell: {
        width: theme.spacing(3)
    },
    icon: {
        marginRight: theme.spacing(2)
    }
}))

interface DealWorkflowTaskRowProps {
    taskWithChildren: DealWorkflowTaskWithChildren
    level?: number
    readOnly?: boolean
}

const DealWorkflowTaskRow = (props: DealWorkflowTaskRowProps) => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore
    const classes = useStyles()

    const { checkChanged } = store
    const { taskWithChildren, readOnly } = props
    const { task } = taskWithChildren
    const level = !!props.level ? props.level : 0

    return (
        <>
            <TableRow>
                <TableCell>
                    <Grid container>
                        {level > 0 && (
                            <Grid item xs={level as any} style={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center' }} >
                                <SubdirectoryArrowRightIcon fontSize='small' className={classes.icon} />
                            </Grid>
                        )}
                        <Grid item xs={(12 - level) as any}>
                            <DealWorkflowTaskField readOnly={readOnly} taskWithChildren={taskWithChildren} />
                        </Grid>
                    </Grid>
                </TableCell>
                <TableCell className={classes.checkboxCell} align='center'>
                    <DealWorkflowTaskDoneCheck readOnly={readOnly} task={task} onChange={checkChanged} />
                </TableCell>
            </TableRow>
            <DealWorkflowTaskRowChildren readOnly={readOnly} level={level} taskWithChildren={taskWithChildren} />
        </>
    )
}

export default observer(DealWorkflowTaskRow)