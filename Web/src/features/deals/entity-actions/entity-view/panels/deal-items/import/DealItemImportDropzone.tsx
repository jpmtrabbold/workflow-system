import React, { useContext } from 'react'
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
import { DealViewStoreContext } from '../../../DealViewStore'

const useStyles = makeStyles((theme: Theme) => ({
    dropzoneParagraph: {
        marginLeft: theme.spacing(4),
        marginRight: theme.spacing(4),
    },
}))

const DealItemImportDropzone = () => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const importStore = rootStore.itemStore.itemImportStore

    return (
        <Dialog open={true} onClose={importStore.excelDropzone.close} fullWidth maxWidth='sm'
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">

            <DialogTitle id="alert-dialog-title">
                {`Import ${importStore.importType === 'dealItem' ? 'Items' : 'Executions'} Template`}
            </DialogTitle>
            <DialogContent id="alert-dialog-description">
                <Grid container spacing={2}>
                    <DropzoneArea
                        onChange={importStore.handleExcelFile}
                        acceptedFiles={['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'text/csv', 'application/vnd.ms-excel']}
                        filesLimit={importStore.fileLimit}
                        dropzoneText='Drop your file here or click to explore'
                        dropzoneParagraphClass={classes.dropzoneParagraph}
                        showPreviews={false}
                        showPreviewsInDropzone={false}
                        clearOnUnmount={true}
                    />
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button onClick={importStore.excelDropzone.close} title="Cancel (ESC)" color="primary">
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    )
}

export default observer(DealItemImportDropzone)