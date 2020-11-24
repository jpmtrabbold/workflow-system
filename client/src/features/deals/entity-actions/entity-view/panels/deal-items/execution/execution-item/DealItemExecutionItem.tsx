import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import Typography from '@material-ui/core/Typography'
import { DealItemDto } from 'clients/deal-system-client-definitions'
import { makeStyles } from '@material-ui/core/styles'
import { DealViewStoreContext } from 'features/deals/entity-actions/entity-view/DealViewStore'
import { DealItemExecutionItemStore } from './DealItemExecutionItemStore'
import { DealItemExecutionsListViewStoreContext } from '../executions-list/DealItemExecutionsListViewStore'
import TableRow from '@material-ui/core/TableRow'
import TableCell from '@material-ui/core/TableCell'
import { DealItemExecutionItemField } from './DealItemExecutionItemField'
import { ObjectViewer } from 'features/shared/components/object-viewer/ObjectViewer'
import { DealItemStore } from '../../DealItemStore'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface DealItemExecutionItemProps {
    executionItem?: DealItemDto
    first: boolean
    last: boolean
}

const useStyles = makeStyles(theme => ({
    alignCenter: {
        display: 'flex', alignItems: 'center', justifyContent: 'center', flexDirection: 'column'
    },
    grid: {
        margin: theme.spacing(1),
    },
    firstGrid: {
        paddingTop: theme.spacing(1),
    },
    executedCell: {
        width: '1%', whiteSpace: 'nowrap', borderBottom: '0px', borderCollapse: 'separate',
        paddingBottom: theme.spacing(1),
        paddingTop: theme.spacing(1),
    }
}))

export const DealItemExecutionItem = observer((props: DealItemExecutionItemProps) => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const executionsStore = useContext(DealItemExecutionsListViewStoreContext)

    const store = useStore(sp => new DealItemExecutionItemStore(sp), { classes, rootStore, executionsStore, ...props })

    return (
        <>
            <TableRow >
                <TableCell className={classes.executedCell}>
                    {props.first && (
                        <Typography variant='subtitle1'>
                            Execution:
                        </Typography>
                    )}
                </TableCell>
                {props.executionItem &&
                    rootStore.itemStore.executionStore.executionFields.map((ef, index) => (
                        <TableCell key={index} className={classes.executedCell}>
                            <DealItemExecutionItemField
                                executionField={ef}
                                executionItem={props.executionItem!}
                            />
                        </TableCell>
                    ))}
                {executionsStore.editable && (
                    <>
                        <TableCell style={{ whiteSpace: 'nowrap' }}>
                            {store.sourceDataButton}
                            
                            {!!props.executionItem && store.deleteButton}

                            {props.last && executionsStore.addButton}

                        </TableCell>
                    </>
                )}
                {store.sourceDataModal.visible && (
                    <ObjectViewer
                        object={props.executionItem?.sourceData ?? {}}
                        onClose={store.sourceDataModal.close}
                        title='View Integration Source Data'
                        open={true}
                        fields={[
                            'sourceId',
                            { fieldname: 'type', transformer: DealItemStore.getIntegrationSourceTypeDescription },
                            'creationDate'
                        ]}
                    />
                )}
            </TableRow>
        </>
    )
})