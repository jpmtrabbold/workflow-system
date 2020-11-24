// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealViewStoreContext } from '../../../DealViewStore'
import Grid from '@material-ui/core/Grid'
import DealWorkflowTasksView from '../tasks/DealWorkflowTasksView'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { htmlSpace } from 'shared-do-not-touch/utils/utils'

const DealWorkflowActionView = () => {

    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    if (!store.action) {
        return null
    }

    return (
        <MessageDialog
            fullWidth
            maxWidth={'md'}
            scroll='paper'
            title={(!!store.action ? (store.action.name + " - ") : "") + 'Tasks'}
            actions={store.actions}
            onClose={store.handleCloseAction}
            open={store.actionDialogVisible}
        >
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    {store.action.description}{htmlSpace}{store.assignmentDescription}
                </Grid>
                <Grid item xs={12}>
                    <DealWorkflowTasksView tasks={store.tasks} />
                </Grid>
            </Grid>
        </MessageDialog>
    )
}

export default observer(DealWorkflowActionView)