// ** Common
import React, { useContext, useMemo } from 'react'
import { observer } from 'mobx-react-lite'

import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import Grid from '@material-ui/core/Grid'
import makeStyles from "@material-ui/core/styles/makeStyles"

import useButtonWithKeyDownListener from 'shared-do-not-touch/hooks/useButtonWithKeyDownListener'
import Tooltip from '@material-ui/core/Tooltip'
import { DealViewStoreContext } from '../../../DealViewStore'
import { ObjectViewer } from 'features/shared/components/object-viewer/ObjectViewer'
import { DealItemStore } from '../DealItemStore'

export type ItemAction = 'delete' | 'save' | 'close' | 'save_add_another'

const useStyles = makeStyles(theme => ({
    grid: {
        // padLeft: theme.spacing(10),
        // padRight: theme.spacing(10)
    },
}))

const DealItemView = () => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.itemStore
    const { handleClose, isNewItem, deleteHandler, saveAndAddAnother, save } = store
    const dealItem = store.currentItem!
    const classes = useStyles()

    const saveAddAnotherButton = useButtonWithKeyDownListener({
        action: saveAndAddAnother,
        keyboardCondition: e => store.usingKeyboardShortcuts && e.key === "Enter" && e.shiftKey && !e.ctrlKey,
        conditionToApplyListener: () => isNewItem() && rootStore.canEditDealBasicInfo,
        button: (
            <Tooltip title="Confirms this item and open new window to add another right away (SHIFT + ENTER)">
                <Button
                    onClick={saveAndAddAnother}

                    color="primary">
                    Ok +
                </Button>
            </Tooltip>
        )
    })

    const saveButton = useButtonWithKeyDownListener({
        action: save,
        keyboardCondition: e => store.usingKeyboardShortcuts && e.key === "Enter" && !e.shiftKey && e.ctrlKey,
        conditionToApplyListener: () => rootStore.canEditDealBasicInfo,
        button: (
            <Tooltip title="Confirms this item (CTRL + ENTER)">
                <Button onClick={save} color="primary">
                    Ok
                </Button>
            </Tooltip>
        )
    })

    const fields = useMemo(() => store.itemFieldsDefinition.map((item) => {
        let firstField = isNewItem()
        const Comp = item.componentRenderer
        const comp = (
            <Grid item key={item.field}>
                <Comp
                    dealItem={dealItem}
                    fieldLabel={item.inputLabel || ""}
                    store={store}
                    fieldName={item.field as any}
                    autofocus={firstField}
                    disabled={!rootStore.canEditDealBasicInfo}
                    errorHandler={store.errorHandler}
                    updatable={item.updatable(dealItem)}
                />
            </Grid>
        )
        firstField = false
        return comp
    }), [store, isNewItem, dealItem, rootStore.canEditDealBasicInfo])

    return (
        <>
            <Dialog
                open={!!dealItem}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                fullWidth
                maxWidth='md'
                scroll='body'
            >
                <DialogTitle id="form-dialog-title">{isNewItem() ? 'Add Deal Item' : rootStore.canEditDealBasicInfo ? 'Edit Deal Item' : 'View Deal Item'}</DialogTitle>
                <DialogContent className={classes.grid}>
                    <Grid container spacing={2} className={classes.grid}>
                        {fields}
                    </Grid>
                    {store.viewingSourceData.visible && (
                        <ObjectViewer
                            object={dealItem.sourceData}
                            onClose={store.viewingSourceData.close}
                            title='View Integration Source Data'
                            open={true}
                            fields={[
                                'sourceId',
                                { fieldname: 'type', transformer: DealItemStore.getIntegrationSourceTypeDescription },
                                'creationDate'
                            ]}
                        />
                    )}
                </DialogContent>
                <DialogActions>
                    {store.currentItemHasSource && (
                        <Button onClick={store.viewingSourceData.open} title="View Integration Source Data" color="primary">
                            Integration Source Data
                        </Button>
                    )}

                    {!isNewItem() && rootStore.canEditDealBasicInfo &&
                        (
                            <Button onClick={deleteHandler} color="secondary">
                                Delete
                            </Button>
                        )}
                    <Button onClick={handleClose} title="Cancel (ESC)" color="primary">
                        Cancel
                    </Button>

                    {saveAddAnotherButton}

                    {saveButton}

                </DialogActions>

            </Dialog>
        </>
    )
}

export default observer(DealItemView)