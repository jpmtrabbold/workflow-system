import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { SalesForecastImportDropzone } from './SalesForecastImportDropzone'
import { SalesForecastUploadStoreContext } from './SalesForecastUploadStore'
import { SalesForecastPreview } from './SalesForecastPreview'

export const SalesForecastUploadView = observer(() => {
    const store = useContext(SalesForecastUploadStoreContext)
    return <>
        {store.dropzoneModal.visible && (
            <SalesForecastImportDropzone
                handleClose={store.dropzoneModal.close}
                handleFiles={store.handleFileUpload}
            />
        )}
        <SalesForecastPreview />
    </>
})