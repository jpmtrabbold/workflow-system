// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import Grid from '@material-ui/core/Grid'
import { DealViewStoreContext, PanelComponentProps } from '../../DealViewStore'
import Typography from '@material-ui/core/Typography'
import DealWorkflowTasksView from './tasks/DealWorkflowTasksView'
import { CustomTable } from 'shared-do-not-touch/material-ui-table/CustomTable'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import useTheme from '@material-ui/core/styles/useTheme'

const DealWorkflowPanelView = (props: PanelComponentProps) => {

    const windowSize = useWindowSize()
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore
    const theme = useTheme()

    const { tasks } = store
    const statuses = rootStore.deal.dealWorkflowStatuses
    if (statuses.length === 0)
        return <Typography variant="subtitle1" id="tableTitle">There is no status history yet.</Typography>

    return (
        <>
            <Grid container spacing={2}>
                <Grid item lg={6} md={6} sm={12} xs={12}>
                    <CustomTable
                        maxHeight={props.fullscreen ? windowSize.height - theme.spacing(20) : 550}
                        minHeight={props.fullscreen ? windowSize.height - theme.spacing(20) : undefined}
                        columns={store.statusColumns}
                        rows={statuses}
                        onRowClick={store.setCurrentStatusHistory}
                        title={<Typography variant="subtitle1">Status History</Typography>}
                        singleRowClickSelection
                        pagination={props.fullscreen ? false : true}
                    />
                </Grid>
                <Grid item lg={6} md={6} sm={12} xs={12}>
                    <DealWorkflowTasksView readOnly={true} tasks={tasks} title={"Tasks" + (!!store.currentStatusHistory ? " - " + store.currentStatusHistory.workflowStatusName : "")} />
                </Grid>
            </Grid>
        </>
    )
}

export default observer(DealWorkflowPanelView)