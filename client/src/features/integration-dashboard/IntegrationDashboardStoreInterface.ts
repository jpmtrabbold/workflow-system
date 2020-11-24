import React from 'react'
import { IntegrationTypeEnum } from 'clients/deal-system-client-definitions';

export interface IntegrationDashboardStoreRunIntegrationProps {
    payload?: string
    callbackClose?: (integrationRan?: boolean) => any
}

export interface IntegrationDashboardStoreInterface {
    description: string
    integrationType: IntegrationTypeEnum
    runIntegrationComponent: React.FunctionComponent<IntegrationDashboardStoreRunIntegrationProps>
}