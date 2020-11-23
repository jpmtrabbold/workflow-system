import React from 'react'
import { DealAttachmentDtoWithLastVersion, DealAttachmentStore } from "./DealAttachmentStore"
import { TextWithTooltip } from 'shared-do-not-touch/material-ui-text-with-tooltip'
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { momentToDateTimeString } from 'features/shared/helpers/utils'
import { DealAttachmentDownloadLink } from './DealAttachmentDownloadLink'

export function DealAttachmentsGridDefinition(store: DealAttachmentStore): Column<DealAttachmentDtoWithLastVersion>[] {

    return [
        {
            title: 'Name',
            rendererComponent: DealAttachmentDownloadLink,
            rawDataForSearchingAndSorting: (data) => data.lastVersion!.fileName + "." + data.lastVersion!.fileExtension,
        },
        {
            title: 'Type',
            render: (data) => store.isAttachmentTypeOther(data.dealAttachment.attachmentTypeId.value) ?
                data.dealAttachment.attachmentTypeOtherText.value :
                store.getAttachmentTypeDescription(data.dealAttachment.attachmentTypeId.value),
        },
        {
            title: 'Version',
            render: (data) => data.lastVersionNumber,
        },
        {
            title: 'Uploaded By',
            render: (data: DealAttachmentDtoWithLastVersion) => data.lastVersion!.creationUserName,
        },
        {
            title: 'Date',
            render: (data: DealAttachmentDtoWithLastVersion) => momentToDateTimeString(data.lastVersion!.createdDate),
            rawDataForSorting: data => data.lastVersion!.createdDate,
        },
        {
            title: 'Locked',
            render: (data: DealAttachmentDtoWithLastVersion) => (
                <TextWithTooltip text={data.lastVersion!.isLocked ? 'Yes' : 'No'} tooltip={data.lastVersion!.isLocked
                    ? "This was locked when the deal changed statuses, so it can't be modified/deleted. For any changes on this attachment, please add a new version."
                    : 'Not locked, therefore can be deleted/modified.'} />
            ),
            rawDataForSearchingAndSorting: (data) => data.lastVersion!.isLocked ? 'Yes' : 'No',
        },
    ]
}
