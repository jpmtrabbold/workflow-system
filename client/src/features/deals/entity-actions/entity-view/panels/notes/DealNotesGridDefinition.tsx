import React from 'react'
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { DealNoteDto } from "../../../../../../clients/deal-system-client-definitions"
import { momentToDateTimeString, momentToDateString } from 'features/shared/helpers/utils'
import Tooltip from '@material-ui/core/Tooltip'
import Typography from '@material-ui/core/Typography'

export const dealNotesGridDefinition: Column<DealNoteDto>[] = [
    {
        title: 'Created',
        render: (data) => momentToDateTimeString(data.createdDate.value),
        rawDataForSorting: data => data.createdDate.value,
    }, {
        title: 'Content',
        render: (data) => data.noteContent.value,
        cellStyle: {
            maxWidth: "600px",
            whiteSpace: "nowrap",
            overflow: "hidden",
            textOverflow: "ellipsis"
        }
    },
    {
        title: 'User',
        field: 'noteCreatorName',
    },
    {
        title: 'Locked',
        render: (data) => (
            <div title={data.isLocked
                ? "This was locked when the deal changed statuses, so it can't be modified/deleted. For any changes on this attachment, please add a new version."
                : 'Not locked, therefore can be deleted/modified.'}>
                {data.isLocked ? 'Yes' : 'No'}
            </div>
        ),
        rawDataForSearchingAndSorting: data => data.isLocked ? 'Yes' : 'No'
    },
    {
        title: 'Reminder',
        render: data => data.reminderType.value ? (
            <Tooltip title={`Reminder on ${momentToDateString(data.reminderDateTime.value)}`}>
                <Typography variant='inherit'>
                    {momentToDateString(data.reminderDateTime.value)}
                </Typography>
            </Tooltip>
        ) : "",
        rawDataForSearchingAndSorting: data => momentToDateString(data.reminderDateTime.value),
    }
]