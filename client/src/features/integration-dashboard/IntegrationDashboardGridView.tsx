import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { GridView } from 'shared-do-not-touch/grid-view'
import IntegrationDashboardGridViewStore from './IntegrationDashboardGridViewStore'
import { IntegrationDashboardStoreInterface } from './IntegrationDashboardStoreInterface'
import { IntegrationDashboardEntriesView } from './integration-run-entries/IntegrationDashboardEntriesView'
import { GlobalStoreContext } from 'features/shared/stores/GlobalStore'
import { useParams, useHistory } from 'react-router-dom'
import Typography from '@material-ui/core/Typography'
import Chip from '@material-ui/core/Chip'
import { useTheme } from '@material-ui/core/styles'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface IntegrationDashboardGridViewProps {
    integrationStore: IntegrationDashboardStoreInterface
}
export const IntegrationDashboardGridView = observer((props: IntegrationDashboardGridViewProps) => {
    const { integrationRunId } = useParams<{ integrationRunId?: string }>()
    const history = useHistory()
    const globalStore = useContext(GlobalStoreContext)
    const store = useStore(sp => new IntegrationDashboardGridViewStore(sp), { ...props, globalStore, integrationRunId, history })
    const theme = useTheme()
    const RunIntegration = store.sp.integrationStore.runIntegrationComponent

    return (
        <>
            <GridView
                store={store.gridStore}
                title={<>
                    <Typography variant='h6'>
                        {props.integrationStore.description}
                    </Typography>
                    {!!integrationRunId && (
                        <Chip
                            style={{ marginLeft: theme.spacing(3) }}
                            label={`Integration Run Id ${integrationRunId}`}
                            onDelete={store.removeSpecificRun}
                            color="primary"
                        />
                    )}
                </>}
                searchable={false}
            />

            {!!store.runIntegrationScreenVisible && (
                <RunIntegration callbackClose={store.integrationRunCallback} payload={store.payload} />
            )}

            {!!store.gridStore.entityActionActive && (
                <IntegrationDashboardEntriesView integrationRun={store.gridStore.selectedEntity!} callbackClose={store.onEntityAction} />
            )}
        </>
    )
})