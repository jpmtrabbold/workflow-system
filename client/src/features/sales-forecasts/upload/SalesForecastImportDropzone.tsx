import React from 'react'
import makeStyles from "@material-ui/styles/makeStyles"
import { Theme } from "@material-ui/core/styles"
import { observer } from "mobx-react-lite"
import Dialog from "@material-ui/core/Dialog"
import DialogTitle from "@material-ui/core/DialogTitle"
import DialogContent from "@material-ui/core/DialogContent"
import Grid from "@material-ui/core/Grid"
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { DropzoneArea } from 'material-ui-dropzone'

interface SalesForecastImportDropzoneProps {
    handleFiles: (files: File[]) => any
    handleClose: () => any
}

const useStyles = makeStyles((theme: Theme) => ({
    dropzoneParagraph: {
        marginLeft: theme.spacing(4),
        marginRight: theme.spacing(4),
    },
}))

export const SalesForecastImportDropzone = observer((props: SalesForecastImportDropzoneProps) => {
    const classes = useStyles()

    return (
        <Dialog open={true} onClose={props.handleClose} fullWidth maxWidth='sm'
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">

            <DialogTitle id="alert-dialog-title">
                Upload Sales Forecast File
            </DialogTitle>
            <DialogContent id="alert-dialog-description">
                <Grid container spacing={2}>
                    <DropzoneArea
                        onChange={props.handleFiles}
                        acceptedFiles={['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet']}
                        filesLimit={1}
                        dropzoneText='Drop your file here or click to explore'
                        dropzoneParagraphClass={classes.dropzoneParagraph}
                        showPreviews={false}
                        showPreviewsInDropzone={false}
                        clearOnUnmount={true}
                    />
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.handleClose} title="Cancel (ESC)" color="primary">
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    )
})