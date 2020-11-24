import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealItemDto } from 'clients/deal-system-client-definitions'
import { makeStyles } from '@material-ui/core/styles'
import { DealViewStoreContext } from '../../../../DealViewStore'
import Paper from '@material-ui/core/Paper'
import { DealItemExecutionsListViewStore, DealItemExecutionsListViewStoreContext as DealItemExecutionsStoreContext } from './DealItemExecutionsListViewStore'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import { DealItemExecutionItem } from '../execution-item/DealItemExecutionItem'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface DealItemExecutionsListViewProps {
    dealItem: DealItemDto
}

const useStyles = makeStyles(theme => ({
    paper: {
        margin: theme.spacing(0, 1, 1, 1),     
    },
}))

export const DealItemExecutionsListView = observer((props: DealItemExecutionsListViewProps) => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = useStore(sp => new DealItemExecutionsListViewStore(sp), { rootStore, ...props })

    return (
        <DealItemExecutionsStoreContext.Provider value={store}>
            <Paper className={classes.paper} elevation={4}>
                <Table>
                    <TableBody>
                        {store.hasExecutedItems
                            ?
                            store.rows
                            : <DealItemExecutionItem first last />
                        }
                    </TableBody>
                </Table>
            </Paper>
        </DealItemExecutionsStoreContext.Provider>
    )
})