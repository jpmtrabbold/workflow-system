import React from 'react'
import { IntegrationDashboardGridView } from '../IntegrationDashboardGridView'
import IntegrationDashboardEmsStore from './IntegrationDashboardEmsStore'
import { observer } from 'mobx-react-lite'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const IntegrationDashboardEms = observer(() => {
    const integrationStore = useStore(() => new IntegrationDashboardEmsStore())
    return <IntegrationDashboardGridView integrationStore={integrationStore}/>
})