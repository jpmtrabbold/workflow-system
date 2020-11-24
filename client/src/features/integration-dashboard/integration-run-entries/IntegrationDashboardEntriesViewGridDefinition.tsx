import React from 'react'
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { momentToDateTimeString } from "features/shared/helpers/utils"
import { IntegrationRunEntryDto } from "clients/deal-system-client-definitions"
import { htmlSpace } from "shared-do-not-touch/utils/utils"
import Link from "@material-ui/core/Link"
import { messageInfo } from "shared-do-not-touch/material-ui-modals/message-info"
import IntegrationDashboardEntriesViewStore from './IntegrationDashboardEntriesViewStore'
import { PayloadClicker } from 'features/shared/components/payload-clicker/PayloadClicker'

export const IntegrationDashboardEntriesViewGridDefinition: Column<IntegrationRunEntryDto>[] = [
    {
        title: 'Id',
        field: 'id',
    },
    {
        title: 'Date / Time',
        field: 'dateTime',
        render: data => momentToDateTimeString(data.dateTime)
    },
    {
        title: 'Message',
        field: 'message',
        render: data => <>
            {data.message}{htmlSpace}
            {!!data.details && (
                <Link onClick={() => messageInfo({ title: "Message Details", content: data.details })} style={{ cursor: 'pointer' }}>
                    Details
                </Link>
            )}
        </>
    },
    {
        title: 'Payload',
        field: 'payload',
        rendererComponent: ({ data }) => <PayloadClicker payload={data.payload} />
    },

    {
        title: 'Affected Entity',
        render: data => (!!data.functionalityOfAffectedId && (
            <>
                {`${IntegrationDashboardEntriesViewStore.getFunctionalityDescription(data.functionalityOfAffectedId)} Id:`}
                {htmlSpace}
                <Link href={`/all-deals/${data.affectedId}`} target='_blank' style={{ cursor: 'pointer' }}>
                    {data.affectedId}
                </Link>
            </>
        )) || null
    },

]