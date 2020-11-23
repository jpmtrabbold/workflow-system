import React from 'react'
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { IntegrationRunListDto } from "../../clients/deal-system-client-definitions"
import { momentToDateTimeString } from "features/shared/helpers/utils"
import IntegrationDashboardGridViewStore from "./IntegrationDashboardGridViewStore"
import { PayloadClicker } from "features/shared/components/payload-clicker/PayloadClicker"

export const IntegrationDashboardGridViewDefinition = (store: IntegrationDashboardGridViewStore) => [
    {
        title: 'Id',
        field: 'id',
    },
    {
        title: 'Started By',
        field: 'startedBy',
    },
    {
        title: 'Execution Started On',
        field: 'started',
        render: data => momentToDateTimeString(data.started),
    },
    {
        title: 'Execution Ended On',
        field: 'ended',
        render: data => momentToDateTimeString(data.ended),
    },
    {
        title: 'Payload',
        field: 'payload',
        rendererComponent: ({ data }) => <PayloadClicker payload={data.payload} />
    },
    {
        title: 'Status',
        field: 'status',
        render: (data) => store.getIntegrationRunStatusDescription(data.status).visualIndicator
    },
] as Column<IntegrationRunListDto>[]

