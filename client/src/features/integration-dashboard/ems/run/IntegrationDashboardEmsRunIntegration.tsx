import React from 'react'
import { observer } from 'mobx-react-lite'
import Grid from '@material-ui/core/Grid'
import { InputProps } from 'shared-do-not-touch/input-props'
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import Paper from '@material-ui/core/Paper'
import { IntegrationDashboardStoreRunIntegrationProps } from 'features/integration-dashboard/IntegrationDashboardStoreInterface'
import IntegrationDashboardEmsRunIntegrationStore from './IntegrationDashboardEmsRunIntegrationStore'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const nullChange = () => null

export const IntegrationDashboardEmsRunIntegration = observer((props: IntegrationDashboardStoreRunIntegrationProps) => {
    const store = useStore(sp => new IntegrationDashboardEmsRunIntegrationStore(sp), props)
    return (
        <MessageDialog title="Run EMS Integration"
            actions={store.actions}
            onClose={props.callbackClose}
            open={true}
        >
            <Paper>
                <Grid container spacing={3}>
                    <Grid item>
                        <InputProps stateObject={store} propertyName='startCreationDate' errorHandler={store.errorHandler} config={{ elementValueForUndefinedOrNull: () => null }}>
                            <KeyboardDatePicker label="Start Creation Date" autoOk format="DD/MM/YYYY" disabled={store.DisabledParams}
                                value={null} onChange={nullChange}
                            />
                        </InputProps>
                    </Grid>
                    <Grid item>
                        <InputProps stateObject={store} propertyName='endCreationDate' errorHandler={store.errorHandler} config={{ elementValueForUndefinedOrNull: () => null }}>
                            <KeyboardDatePicker label="End Creation Date" autoOk format="DD/MM/YYYY" disabled={store.DisabledParams}
                                value={null} onChange={nullChange}
                            />
                        </InputProps>
                    </Grid>
                </Grid>
            </Paper>
        </MessageDialog>
    )
})