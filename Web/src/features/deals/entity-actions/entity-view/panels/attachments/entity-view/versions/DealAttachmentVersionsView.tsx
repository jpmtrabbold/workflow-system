import React, { useContext, useMemo } from "react"
import { observer } from "mobx-react-lite"
import Dialog from "@material-ui/core/Dialog"
import DialogTitle from "@material-ui/core/DialogTitle"
import DialogContent from "@material-ui/core/DialogContent"

import Grid from "@material-ui/core/Grid"
import DialogActions from "@material-ui/core/DialogActions"
import Button from "@material-ui/core/Button"


import { DealViewStoreContext } from "../../../../DealViewStore"
import { DealAttachmentVersionsGridDefinition } from "./DealAttachmentVersionsGridDefinition"
import { CustomTable } from "shared-do-not-touch/material-ui-table"

export type AttachmentAction = "delete" | "save" | "close" | "save_add_another" | "add_version"

interface DealAttachmentVersionsViewProps {
    onCloseVersionHistory: () => any
    versionHistoryOpen: boolean
}

const DealAttachmentVersionsView = (props: DealAttachmentVersionsViewProps) => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.attachmentStore
    const { versionHistoryOpen: versionHistoryOpened } = props

    const colums = useMemo(() => DealAttachmentVersionsGridDefinition(store), [store])
    
    return (
        <Dialog open={!!versionHistoryOpened} onClose={() => props.onCloseVersionHistory()} fullWidth maxWidth='xl'
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">

            <DialogTitle id="alert-dialog-title">Version History</DialogTitle>
            <DialogContent id="alert-dialog-description">
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <CustomTable
                            columns={colums}
                            rows={store.attachmentVersions}
                            title="Attachment Versions"
                        />
                    </Grid>
                </Grid>
            </DialogContent>

            <DialogActions>
                <Button onClick={() => props.onCloseVersionHistory()} title="Close (ESC)" color="primary">
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    )
}

export default observer(DealAttachmentVersionsView)