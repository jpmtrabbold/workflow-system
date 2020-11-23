import React, { useContext } from "react"
import { observer } from "mobx-react-lite"
import TextField from "@material-ui/core/TextField"
import Grid from "@material-ui/core/Grid"
import Button from "@material-ui/core/Button"
import { DealViewStoreContext } from "../../../DealViewStore"
import useButtonWithKeyDownListener from "shared-do-not-touch/hooks/useButtonWithKeyDownListener"
import makeStyles from "@material-ui/core/styles/makeStyles"
import { InputProps } from "shared-do-not-touch/input-props"
import { DealNoteViewStore } from "./DealNoteViewStore"
import { MessageDialog } from "shared-do-not-touch/material-ui-modals"
import { SelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label/SelectWithLabel"
import { KeyboardDatePicker } from "@material-ui/pickers/DatePicker"
import Card from "@material-ui/core/Card"
import CardContent from "@material-ui/core/CardContent"
import Typography from "@material-ui/core/Typography"
import Grow from "@material-ui/core/Grow"
import { AutoCompleteField } from "features/shared/components/chip-input-auto-suggest/AutoCompleteField"
import useStore from "shared-do-not-touch/mobx-utils/useStore"
export type NoteAction = "delete" | "save" | "close" | "save_add_another"

const nullChange = () => null

const useStyles = makeStyles(theme => ({
    reminderPaper: {
        padding: theme.spacing(2)
    },
    reminderGrid: {
        maxHeight: '1000px',
        transition: 'max-height 0.5s ease-in',
        overflow: 'hidden',
    },
    reminderGridHidden: {
        maxHeight: 0,
        transition: 'max-height 0.5s ease-out',
        overflow: 'hidden',
    },
}))

export const DealNoteView = observer(() => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = useStore(sp => new DealNoteViewStore(sp), { rootStore })

    const saveAddAnotherButton = useButtonWithKeyDownListener(store.saveAddAnotherButtonProps)
    const saveButton = useButtonWithKeyDownListener(store.saveButtonProps)
    const deleteButton = useButtonWithKeyDownListener(store.deleteButtonProps)

    return (<>
        <MessageDialog
            open={!!store.currentNote}
            onClose={store.handleClose}
            maxWidth='sm'
            title={store.isCurrentNoteNew ? "Create Note" : store.userEditThisNote ? "Edit Note" : "View Note"}
            actionsRender={() => <>
                {store.canAddReminder && (
                    <Button color="primary" onClick={store.addReminder}>
                        Add Reminder
                    </Button>
                )}
                {store.canDeleteReminder && (
                    <Button color="primary" onClick={store.deleteReminder}>
                        Delete Reminder
                    </Button>
                )}
                <Button onClick={store.handleClose} title="Cancel (ESC)" color="primary">
                    Cancel
                </Button>
                {saveAddAnotherButton}
                {saveButton}
                {deleteButton}
            </>}
        >
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <InputProps stateObject={store.currentNote!} propertyName="noteContent">
                        <TextField
                            label="Note content"
                            variant='outlined'
                            multiline
                            fullWidth
                            placeholder="Enter note details here..."
                            autoFocus
                            disabled={!store.userEditThisNote}
                        />
                    </InputProps>
                </Grid>
                <Grid item xs={12}>
                    <TextField
                        label="Created By"
                        value={store.createdBy}
                        variant='outlined'
                        fullWidth
                        disabled={true}
                    />
                </Grid>
                <Grow in={store.hasReminder}>
                    <Grid item xs={12} >
                        <Card className={store.hasReminder ? classes.reminderGrid : classes.reminderGridHidden}>
                            <CardContent>
                                <Grid container spacing={2} className={classes.reminderPaper}>
                                    <Grid item xs={12}>
                                        <Typography variant="h6">
                                            Reminder
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <InputProps stateObject={store.currentNote!} propertyName="reminderType">
                                            <SelectWithLabel
                                                label="Who will be notified?"
                                                fullWidth
                                                disabled={!store.userEditThisNote}
                                                lookups={store.reminderLookups}
                                            />
                                        </InputProps>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <InputProps stateObject={store.currentNote!} propertyName="reminderDateTime" config={{ elementValueForUndefinedOrNull: () => null }}>
                                            <KeyboardDatePicker
                                                label="When?"
                                                format="DD/MM/YYYY"
                                                fullWidth
                                                disabled={!store.userEditThisNote}
                                                onChange={nullChange}
                                                value={null}
                                                autoOk
                                            />
                                        </InputProps>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <AutoCompleteField
                                            onValueAdd={store.addEmail}
                                            onValueRemove={store.removeEmail}
                                            onValuesClear={store.clearEmails}
                                            multipleSelectedValues={store.selected}
                                            dataSource={store.getUsers}
                                            label={store.emailAccountsLabel}
                                            fullWidth
                                            disabled={!store.userEditThisNote}
                                            canAdd
                                            pageSize={10}
                                        />
                                    </Grid>
                                </Grid>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grow>
            </Grid>

        </MessageDialog>
    </>)
})
