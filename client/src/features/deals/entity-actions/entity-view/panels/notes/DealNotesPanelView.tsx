// ** Common
import React, { useContext } from "react"
import { observer } from "mobx-react-lite"
import { dealNotesGridDefinition } from "./DealNotesGridDefinition"
import makeStyles from "@material-ui/core/styles/makeStyles"
import { DealNoteView } from "./entity-view/DealNoteView"
import { LazyLoadedDataStateEnum } from "../../../../../../clients/deal-system-client-definitions"
import { DealViewStoreContext, PanelComponentProps } from "../../DealViewStore"
import Typography from "@material-ui/core/Typography"
import { CustomTable } from "shared-do-not-touch/material-ui-table"

const useStyles = makeStyles({
    notesTable: {
        flexGrow: 1,
        padding: 1,
        margin: 1,
        overflowX: "auto"
    }
})

const DealNotesPanelView = (props: PanelComponentProps) => {
    const classes = useStyles()
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.noteStore

    switch (rootStore.deal.notes.state) {
        case LazyLoadedDataStateEnum.Loading:
            return <Typography variant='inherit'>Loading...</Typography>
        case LazyLoadedDataStateEnum.NotLoaded:
            return <Typography variant='inherit'>Data failed to be loaded</Typography>
    }

    return (
        <div className={classes.notesTable}>
            <CustomTable
                columns={dealNotesGridDefinition}
                rows={store.dealNotes}
                title={
                    <Typography variant="subtitle1">
                        {`Notes` + (props.fullscreen && rootStore.deal.dealNumber ? ` - ${rootStore.deal.dealNumber}` : "")}
                    </Typography>
                }
                onRowClick={store.setCurrentNote}
                actions={store.actions}
                fullscreen={props.fullscreen}
                fullscreenable={props.fullscreen}
                fullscreenExited={props.fullscreenExited}
            />
            {!!store.currentNote ? (
                <DealNoteView />
            ) : null}
        </div>
    )
}

export default observer(DealNotesPanelView)
