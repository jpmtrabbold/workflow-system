// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import makeStyles from '@material-ui/core/styles/makeStyles'
import createStyles from '@material-ui/core/styles/createStyles'
import { Theme } from '@material-ui/core/styles'
import DealItemView from './entity-view/DealItemView'
import { DealViewStoreContext, PanelComponentProps } from '../../DealViewStore'
import DealItemDropzone from './import/DealItemImportDropzone'
import { DealItemImportView } from './import/DealItemImportView'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import Typography from '@material-ui/core/Typography'
import { LazyLoadedDataStateEnum } from 'clients/deal-system-client-definitions'
import { ConditionalRenderer } from 'shared-do-not-touch/mobx-rendering/ConditionalRenderer'

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        content: {
            flexGrow: 1,
            padding: 1,
            margin: 1,
            overflowX: 'auto'
        },
    }),
)

const DealItemsPanelView = (props: PanelComponentProps) => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.itemStore
    const importStore = store.itemImportStore

    switch (rootStore.deal.items.state) {
        case LazyLoadedDataStateEnum.Loading:
            return <Typography variant='inherit'>Loading...</Typography>
        case LazyLoadedDataStateEnum.NotLoaded:
            return <Typography variant='inherit'>Data failed to be loaded</Typography>
    }

    if (!store.itemFieldsDefinition || store.itemFieldsDefinition.length === 0)
        return null
    console.log(`importStore.excelDropzone.visible: ${importStore.excelDropzone.visible}`)
    return (
        <div className={classes.content} onDragEnter={importStore.onDragEnter}>
            <CustomTable
                maxHeight={550}
                columns={store.itemFieldsDefinition}
                rows={store.dealItems}
                onRowClick={store.editItem}
                fullscreen={props.fullscreen}
                fullscreenable={props.fullscreen}
                fullscreenExited={props.fullscreenExited}
                userCanToggleSize
                title={
                    <Typography variant="subtitle1">
                        {`Items` + (props.fullscreen && rootStore.deal.dealNumber ? ` - ${rootStore.deal.dealNumber}` : "")}
                    </Typography>
                }
                size='medium'
                actions={store.itemActions}
                showSelectAllCheckbox={true}
                selectable={true}
                collapsibleDetailConfig={store.executionStore.collapsibleDetailConfig}
            />

            <ConditionalRenderer stateObject={store} propertyName='currentItem'>
                <DealItemView />
            </ConditionalRenderer>

            <ConditionalRenderer stateObject={importStore.excelDropzone} propertyName='visible'>
                <DealItemDropzone/>
            </ConditionalRenderer>

            <ConditionalRenderer stateObject={importStore.importedItemsReviewModal} propertyName='visible'>
                <DealItemImportView />
            </ConditionalRenderer>
        </div>
    )
}

export default observer(DealItemsPanelView)