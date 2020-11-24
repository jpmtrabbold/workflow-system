import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import IntegrationDashboardEntriesViewStore from './IntegrationDashboardEntriesViewStore'
import { IntegrationRunListDto } from 'clients/deal-system-client-definitions'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import { IntegrationDashboardEntriesViewGridDefinition } from './IntegrationDashboardEntriesViewGridDefinition'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import useTheme from '@material-ui/core/styles/useTheme'
import { GlobalStoreContext } from 'features/shared/stores/GlobalStore'
import { IntegrationRunAction } from '../IntegrationDashboardGridViewStore'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface IntegrationDashboardEntriesViewProps {
    integrationRun: IntegrationRunListDto
    callbackClose: (action: IntegrationRunAction, integrationRunId?: number) => any
}

export const IntegrationDashboardEntriesView = observer((props: IntegrationDashboardEntriesViewProps) => {
    const globalStore = useContext(GlobalStoreContext)
    const store = useStore(sp => new IntegrationDashboardEntriesViewStore(sp), {...props, globalStore})
    const windowSize = useWindowSize()
    const theme = useTheme()
    const height = windowSize.height - theme.spacing(25)

    return (
        <>
            <MessageDialog open={true} maxWidth='xl' onClose={store.onClose} actions={store.actions}>
                <CustomTable
                    title={`Integration Run ${props.integrationRun.id} - Log Entries`}
                    columns={IntegrationDashboardEntriesViewGridDefinition}
                    rows={store.rows}
                    forceIsLoading={store.loading}
                    pageSizeOptions={[10, 50, 100, 2000]}
                    maxHeight={height}
                    minHeight={height}
                />
            </MessageDialog>
        </>
    )
})