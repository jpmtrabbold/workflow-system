import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import Dialog from "@material-ui/core/Dialog"
import DialogContent from "@material-ui/core/DialogContent"
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import makeStyles from "@material-ui/core/styles/makeStyles"
import Tooltip from '@material-ui/core/Tooltip'
import { DealViewStoreContext } from '../../../DealViewStore'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import useWindowSize from 'shared-do-not-touch/hooks/useWindowSize'
import { useTheme } from '@material-ui/core/styles'

const useStyles = makeStyles(theme =>
    ({
        content: {
            flexGrow: 1,
            padding: 1,
            margin: 1,
            overflowX: 'auto'
        },
    }),
)

export const DealItemImportView = observer(() => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.itemStore
    const importStore = store.itemImportStore
    const windowSize = useWindowSize()
    const theme = useTheme()
    
    if (!importStore.importedGridDefinition || importStore.importedGridDefinition.length === 0) {
        return null
    }
   
    return (
        <Dialog open={true} onClose={importStore.importedItemsReviewModal.close} fullWidth maxWidth='xl'>
            <DialogContent id="alert-dialog-description">
                <div className={classes.content}>
                    <CustomTable
                        maxHeight={windowSize.height - theme.spacing(20)}
                        minHeight={windowSize.height - theme.spacing(20)}
                        columns={importStore.importedGridDefinition}
                        rows={importStore.filteredImportedItems}
                        actions={importStore.actions}
                        fullscreenable
                        title={`${importStore.importType === 'dealItem' ? 'Items' : 'Executions'} to be imported`}
                    />
                </div>
            </DialogContent>
            <DialogActions>
                <Button onClick={importStore.importedItemsReviewModal.close} color="primary">
                    {importStore.hasError ? "Close" : "Cancel"}
                </Button>
                {!importStore.hasError && (
                    <Tooltip title={`Confirms the ${importStore.importType === 'dealItem' ? 'dealItems' : 'executions'} to be imported`}>
                        <Button onClick={importStore.confirmImportedItems} color="primary">
                            Confirm
                        </Button>
                    </Tooltip>
                )}
            </DialogActions>
        </Dialog>
    )
})

