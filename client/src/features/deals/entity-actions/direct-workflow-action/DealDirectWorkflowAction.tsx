import React from 'react'
import { observer } from "mobx-react-lite"
import { useParams } from 'react-router-dom';
import useQueryString from 'custom-hooks/useQueryString';
import { DealDirectWorkflowActionQuery, DealDirectWorkflowActionStore } from './DealDirectWorkflowActionStore';
import useInitialMount from 'shared-do-not-touch/hooks/useInitialMount';
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals';
import Grid from '@material-ui/core/Grid';
import { InputProps } from 'shared-do-not-touch/input-props';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';
import useTheme from '@material-ui/core/styles/useTheme';
import useStore from 'shared-do-not-touch/mobx-utils/useStore';

export const DealDirectWorkflowAction = observer(() => {
    const { dealId } = useParams<{ dealId?: string }>();
    const parsedQuery = useQueryString()

    const store = useStore(sp => new DealDirectWorkflowActionStore(sp), { dealId, ...parsedQuery } as DealDirectWorkflowActionQuery)
    const theme = useTheme()

    useInitialMount(() => {
        store.carryOutAction()
    })

    if (store.fillReason) {
        return (
            <MessageDialog
                title={`'${store.sp.actionName}' Action - Deal ${store.dealNumber}`}
                content={(
                    <Grid container>
                        <Grid item>
                            <Typography variant='subtitle2'>
                                Please provide a reason for this action:
                            </Typography>
                            <br />
                            <InputProps stateObject={store} propertyName={'reason'} errorHandler={store.errorHandler}>
                                <TextField fullWidth multiline style={{ width: theme.spacing(50) }} />
                            </InputProps>
                            <br />
                        </Grid>
                    </Grid>
                )}
                actions={[{ name: "Ok", color: 'primary', callback: store.carryOutActionWithReason }]}
            />
        )
    }

    return null
})