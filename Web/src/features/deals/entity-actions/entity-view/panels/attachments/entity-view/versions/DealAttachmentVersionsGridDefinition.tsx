import React from 'react'
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import {  DealAttachmentStore, DealAttachmentVersionDtoWithVersionNumber } from "../../DealAttachmentStore"
import Link from "@material-ui/core/Link"
import { momentToDateTimeString } from 'features/shared/helpers/utils'

export function DealAttachmentVersionsGridDefinition(store: DealAttachmentStore): Column<DealAttachmentVersionDtoWithVersionNumber>[] {

    return [
        {
            title: 'Name',
            render: (data) => (
                <Link onClick={(event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
                    store.downloadVersion(data)
                    event.stopPropagation()
                }}>
                    {data.fileName + "." + data.fileExtension}
                </Link>
            ),
            rawDataForSearchingAndSorting: (data) => data.fileName + "." + data.fileExtension,
        },
        {
            title: 'Version',
            render: (data) => data.versionNumber,
        },
        {
            title: 'Uploaded By',
            render: (data) => data.creationUserName,
        },
        {
            title: 'Date',
            render: (data) => momentToDateTimeString(data.createdDate),
            rawDataForSorting: data => data.createdDate,
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
            rawDataForSearchingAndSorting: (data) => data.isLocked ? 'Yes' : 'No'
        },
    ]
}