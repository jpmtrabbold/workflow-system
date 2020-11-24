import React, { useContext } from "react"
import { observer } from "mobx-react-lite"
import Dialog from "@material-ui/core/Dialog"
import DialogTitle from "@material-ui/core/DialogTitle"
import DialogContent from "@material-ui/core/DialogContent"

import TextField from "@material-ui/core/TextField"
import Grid from "@material-ui/core/Grid"
import DialogActions from "@material-ui/core/DialogActions"
import Button from "@material-ui/core/Button"
import Tooltip from "@material-ui/core/Tooltip"

import { DealViewStoreContext } from "../../../DealViewStore"
import useButtonWithKeyDownListener from "shared-do-not-touch/hooks/useButtonWithKeyDownListener"
import DealAttachmentVersionsView from "./versions/DealAttachmentVersionsView"
import DealAttachmentDropzone from "./DealAttachmentDropzone"
import { AutoCompleteField } from 'shared-do-not-touch/material-ui-auto-complete/AutoCompleteField'
import { InputProps } from "shared-do-not-touch/input-props"
import { momentToDateTimeString } from "features/shared/helpers/utils"
import { DealAttachmentDownloadLink } from "../DealAttachmentDownloadLink"

const DealAttachmentView = () => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.attachmentStore

    const currentAttachment = store.currentAttachment!
    const currentVersion = store.currentAttachment!.lastVersion!

    const hasFile = !!currentVersion?.createdDate

    const saveAddAnotherButton = useButtonWithKeyDownListener({
        action: store.saveAttachmentAndAddAnother,
        keyboardCondition: e => e.key === "Enter" && e.shiftKey && !e.ctrlKey,
        conditionToApplyListener: () => store.hasSaveAndAddAnotherButton,
        button: (
            <Tooltip title="Confirms this attachment and open new window to add another right away (SHIFT + ENTER)">
                <Button
                    onClick={store.saveAttachmentAndAddAnother}
                    color="primary"
                >
                    Ok +
                </Button>
            </Tooltip>
        )
    })

    const saveButton = useButtonWithKeyDownListener({
        action: store.saveAttachmentAndClose,
        keyboardCondition: e => e.key === "Enter" && !e.shiftKey && e.ctrlKey,
        conditionToApplyListener: () => store.hasSaveButton,
        button: (
            <Tooltip title="Confirms this attachment (CTRL + ENTER)">
                <Button
                    onClick={store.saveAttachmentAndClose}
                    color="primary"
                >
                    Ok
                </Button>
            </Tooltip>
        )
    })

    if (!currentVersion) {
        return null
    }

    const dropzoneArea = !hasFile && (
        <Grid item xs={12}>
            <DealAttachmentDropzone handleFiles={store.handleFiles} />
        </Grid>
    )

    const versionFields = hasFile && (
        <>
            <Grid item xs={12}>
                {store.nameFieldEnabled ? (
                    <InputProps stateObject={currentVersion} propertyName='fileName'>
                        <TextField label='Name' disabled={!store.nameFieldEnabled} autoFocus fullWidth />
                    </InputProps>
                ) : (
                        <>File: <DealAttachmentDownloadLink data={store.currentAttachment!} /></>
                    )
                }
            </Grid>
            <Grid item xs={6}>
                <TextField label='Extension' value={currentVersion.fileExtension} disabled={true} fullWidth />
            </Grid>
            <Grid item xs={6}>
                <TextField label='Size' value={store.fileSizeDescription} disabled={true} fullWidth />
            </Grid>
            <Grid item xs={12}>
                <TextField label='Uploaded By' value={`${currentVersion.creationUserName} at ${momentToDateTimeString(currentVersion.createdDate)}`}
                    disabled={true} fullWidth />
            </Grid>
            <Grid item xs={12}>
                <InputProps stateObject={currentAttachment.dealAttachment} propertyName="attachmentTypeId">
                    <AutoCompleteField label='Attachment Type'
                        disabled={!store.attachmentTypeEnabled} fullWidth dataSource={store.attachmentTypes} />
                </InputProps>
            </Grid>
            {store.hasOtherAttachmentType && (
                <Grid item xs={12}>
                    <InputProps stateObject={currentAttachment.dealAttachment} propertyName="attachmentTypeOtherText">
                        <TextField label='Attachment Type Description' disabled={!store.attachmentTypeEnabled} fullWidth />
                    </InputProps>
                </Grid>
            )}
        </>
    )

    return (
        <Dialog open={!!currentAttachment} onClose={store.closeAttachment} fullWidth maxWidth='sm'
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">

            <DialogTitle id="alert-dialog-title">
                {store.actionType === 'add-version' ? "Add Version" :
                    store.actionType === 'add-attachment' ? "Add Attachment" :
                        store.actionType === 'edit-attachment' ? "Edit Attachment" : "View Attachment"}
            </DialogTitle>
            <DialogContent id="alert-dialog-description">
                <Grid container spacing={2}>
                    {dropzoneArea}
                    {versionFields}
                </Grid>
                <DealAttachmentVersionsView
                    versionHistoryOpen={store.versionHistoryOpen}
                    onCloseVersionHistory={store.closeVersionHistory}
                />
            </DialogContent>

            <DialogActions>

                {!!store.canDeleteAttachment && (
                    <Tooltip title="Deletes the current version of the document.">
                        <Button onClick={store.deleteAttachmentAndClose} color="secondary">
                            Delete
                        </Button>
                    </Tooltip>
                )}

                <Tooltip title="Cancel (ESC)">
                    <Button onClick={store.closeAttachment} color="primary">
                        Cancel
                    </Button>
                </Tooltip>

                {store.shouldShowVersionHistory && (
                    <Tooltip title="Opens the version history" >
                        <Button onClick={store.openVersionHistory} color="primary">
                            Version History
                        </Button>
                    </Tooltip>
                )}

                {store.canAddVersion && (
                    <Tooltip title="Add a new version to this attachment"  >
                        <Button onClick={store.addAttachmentVersion} color="primary">
                            Add Version
                        </Button>
                    </Tooltip>
                )}

                {saveAddAnotherButton}

                {saveButton}

            </DialogActions>
        </Dialog>
    )
}

export default observer(DealAttachmentView)