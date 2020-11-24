import React, { useContext } from 'react'
import { MessageDialog } from "shared-do-not-touch/material-ui-modals"
import { observer } from "mobx-react-lite"
import { DealViewStoreContext } from '../../../DealViewStore'
import { InputProps } from 'shared-do-not-touch/input-props'
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'
import Grid from '@material-ui/core/Grid'


export const DealWorkflowActionTermOverrideDialog = observer(() => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    return (
        <MessageDialog
            title='Term Override'
            actions={store.overrideActions}
            open={store.termOverrideDialogOpen}
            onClose={store.closeTermOverrideDialog}
            focusOnFirstAction={false}
        >
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <InputProps stateObject={store} propertyName='termInMonths' variant='numeric'
                        config={{ maxDecimalPlaces: 0, maxIntegerLength: 3, onlyPositives: true }}
                        onValueChanged={store.setTermChanged}
                    >
                        <TextField
                            inputRef={store.termInput}
                            label='New Term'
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position="start">
                                        months
                                    </InputAdornment>
                                )
                            }}
                        />
                    </InputProps>
                </Grid>
                <Grid item xs={12}>
                    <InputProps stateObject={store} propertyName='termNote' errorHandler={store.errorHandler}>
                        <TextField
                            disabled={!store.termChanged}
                            multiline
                            fullWidth
                            label='Term Change Note'
                        />
                    </InputProps>
                </Grid>
            </Grid>
        </MessageDialog>
    )
})