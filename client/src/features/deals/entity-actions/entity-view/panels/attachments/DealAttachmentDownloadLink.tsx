import React, { useContext } from 'react'
import { DealAttachmentDtoWithLastVersion } from "./DealAttachmentStore"
import Link from "@material-ui/core/Link"
import { observer } from 'mobx-react-lite'
import { DealViewStoreContext } from '../../DealViewStore'

export const DealAttachmentDownloadLink = observer(({ data }: { data: DealAttachmentDtoWithLastVersion }) => {
    const store = useContext(DealViewStoreContext).attachmentStore
    return (

        <Link
            onClick={(event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
                store.downloadVersion(data.lastVersion!)
                event.stopPropagation()
            }}
            style={{ cursor: 'pointer' }}
        >
            {data.lastVersion!.fileName + "." + data.lastVersion!.fileExtension}
        </Link>
    )
})