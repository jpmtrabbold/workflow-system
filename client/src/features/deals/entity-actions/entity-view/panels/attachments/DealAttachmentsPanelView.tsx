// ** Common
import React, { useContext } from "react"
import { observer } from "mobx-react-lite"
import makeStyles from "@material-ui/core/styles/makeStyles"
import DealAttachmentView from "./entity-view/DealAttachmentView"
import { DealViewStoreContext, PanelComponentProps } from "../../DealViewStore"
import Typography from "@material-ui/core/Typography"
import { CustomTable } from "shared-do-not-touch/material-ui-table"
import { theme } from "shared-do-not-touch/theme/theme"
import { LazyLoadedDataStateEnum } from "clients/deal-system-client-definitions"

const useStyles = makeStyles({
    attachmentsTable: {
        flexGrow: 1,
        padding: 1,
        margin: 1,
        overflowX: "auto"
    }
})

const DealAttachmentsPanelView = (props: PanelComponentProps) => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.attachmentStore

    switch (rootStore.deal.attachments.state) {
        case LazyLoadedDataStateEnum.Loading:
            return <Typography variant='inherit'>Loading...</Typography>
        case LazyLoadedDataStateEnum.NotLoaded:
            return <Typography variant='inherit'>Data failed to be loaded</Typography>
    }

    return (
        <div className={classes.attachmentsTable} onDragEnter={store.onDragEnter}>
            <CustomTable
                columns={store.attachmentsGridDefinition}
                rows={store.dealAttachments}
                title={
                    <Typography variant="subtitle1">
                        {`Attachments` + (props.fullscreen && rootStore.deal.dealNumber ? ` - ${rootStore.deal.dealNumber}` : "")}
                    </Typography>
                }
                onRowClick={store.onRowClick}
                actions={store.actions}
                minHeight={theme.spacing(60)}
                maxHeight={theme.spacing(60)}
                fullscreen={props.fullscreen}
                fullscreenable={props.fullscreen}
                fullscreenExited={props.fullscreenExited}
                selectable
                showSelectAllCheckbox
            />
            {!!store.currentAttachment && (
                <DealAttachmentView />
            )}
        </div>
    )
}

export default observer(DealAttachmentsPanelView)
