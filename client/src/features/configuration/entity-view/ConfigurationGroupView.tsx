// ** Common
import React from "react"
import { observer } from "mobx-react-lite"
import Grid from "@material-ui/core/Grid"
import makeStyles from "@material-ui/core/styles/makeStyles"
import TextField from "@material-ui/core/TextField"
import useInitialMount from "shared-do-not-touch/hooks/useInitialMount"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal, MessageDialog } from "shared-do-not-touch/material-ui-modals"
import { ConfigurationGroupViewStore, ConfigurationGroupViewStoreContext } from "./ConfigurationGroupViewStore"
import { ConfigurationEntriesView } from "./entries/ConfigurationEntriesView"
import useStore from "shared-do-not-touch/mobx-utils/useStore"

const useStyles = makeStyles(theme => ({
    grid: {
        padLeft: theme.spacing(10),
        padRight: theme.spacing(10),
        justifyContent: "space-evenly"
    },
    container: {
        display: "flex"
    },
    clearButtonContainer: {
        display: "flex",
        alignItems: "center"
    }
}))

export interface ConfigurationGroupViewProps {
    entityId?: number
    entityName?: string
    visible: boolean
    onEntityClose: () => any
    readOnly?: boolean
}

export const ConfigurationGroupView = observer((props: ConfigurationGroupViewProps) => {
    const classes = useStyles()
    const store = useStore(sp => new ConfigurationGroupViewStore(sp), props)

    useInitialMount(() => {
        store.onLoad()
    })

    const { entity } = store
    const { visible } = props

    if (!store.loaded) {
        return (
            <LoadingModal
                title={
                    !props.entityName
                        ? "Loading..."
                        : `Loading ${props.entityName}`
                }
                visible={visible}
            />
        )
    }

    return (
        <ConfigurationGroupViewStoreContext.Provider value={store}>
            <MessageDialog
                open={visible}
                title={props.readOnly ? "View Configuration Group" : "Edit Configuration Group"}
                onClose={store.handleClose}
                fullWidth={true}
                maxWidth="md"
                scroll="body"
                actions={store.actions}
            >
                <div className={classes.container}>
                    <Grid container spacing={2} className={classes.grid}>
                        <Grid item sm={3} xs={12}>
                            <InputProps stateObject={entity} propertyName="name">
                                <TextField
                                    disabled={true}
                                    label="Name"
                                    fullWidth
                                />
                            </InputProps>
                        </Grid>
                        <Grid item sm={9} xs={12}>
                            <InputProps stateObject={entity} propertyName="description">
                                <TextField
                                    disabled={true}
                                    label="Description"
                                    fullWidth
                                />
                            </InputProps>
                        </Grid>
                        <Grid item xs={12}>
                            <ConfigurationEntriesView />
                        </Grid>
                    </Grid>
                </div>
            </MessageDialog>
        </ConfigurationGroupViewStoreContext.Provider>
    )
})
