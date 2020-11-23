import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import Dialog from "@material-ui/core/Dialog"
import DialogContent from "@material-ui/core/DialogContent"
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { SalesForecastUploadStoreContext } from './SalesForecastUploadStore'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'

export const SalesForecastPreview = observer(() => {
    const store = useContext(SalesForecastUploadStoreContext)
    const windowSize = useWindowSize()
    return (
        <Dialog open={store.previewModal.visible} onClose={store.previewModal.close} fullWidth maxWidth='sm'
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">

            <DialogContent id="alert-dialog-description">
                <CustomTable
                    title='Sales Forecast - Upload Preview'
                    rows={store.importedForecasts}
                    columns={store.previewColumns}
                    searchable={false}
                    minHeight={windowSize.height - 150}
                    maxHeight={windowSize.height - 150}
                    pagination={false}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={store.previewModal.close} title="Cancel (ESC)" color="primary">
                    Cancel
                </Button>
                {!store.hasErrors && (
                    <Button onClick={store.confirmUpload} title="Confirm Upload" color="primary">
                        Confirm
                    </Button>
                )}
            </DialogActions>
        </Dialog>
    )
})