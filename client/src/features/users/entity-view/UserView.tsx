// ** Common
import React from "react"
import { observer } from "mobx-react-lite"
import Grid from "@material-ui/core/Grid"
import makeStyles from "@material-ui/core/styles/makeStyles"
import TextField from "@material-ui/core/TextField"
import useInitialMount from "shared-do-not-touch/hooks/useInitialMount"
import { UserViewStoreContext, UserViewStore } from "./UserViewStore"
import { InputProps } from "shared-do-not-touch/input-props"
import { LoadingModal, MessageDialog } from "shared-do-not-touch/material-ui-modals"
import { AutoCompleteField } from "shared-do-not-touch/material-ui-auto-complete/AutoCompleteField"
import { CheckBoxWithLabel } from "shared-do-not-touch/material-ui-checkbox-with-label"
import { useTheme } from "@material-ui/core/styles"
import { UserIntegrationDataView } from "./divisions/UserIntegrationDataView"
import { UserWorkflowRolesView } from "./divisions/UserWorkflowRolesView"
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

export interface UserViewProps {
    entityId?: number
    entityName?: string
    visible: boolean
    onEntityClose: () => any
    readOnly?: boolean
}

const UserView = (props: UserViewProps) => {
    const classes = useStyles()
    const theme = useTheme()

    const store = useStore(sp => new UserViewStore(sp.props, sp.theme), { props, theme })

    useInitialMount(() => {
        store.onLoad()
    })

    const { user } = store
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
        <UserViewStoreContext.Provider value={store}>
            <MessageDialog
                open={visible}
                title={props.readOnly ? "View User" : store.creation ? "Add User" : "Edit User"}
                onClose={store.handleClose}
                maxWidth="md"
                scroll="body"
                actions={store.actions}
            >
                <div className={classes.container}>
                    <Grid container spacing={2} className={classes.grid}>
                        <Grid item sm={6} xs={12}>
                            <InputProps stateObject={user} propertyName="name">

                                <TextField
                                    disabled={props.readOnly}
                                    label="Name"
                                    required
                                    fullWidth
                                    autoFocus
                                />
                            </InputProps>
                        </Grid>
                        <Grid item sm={6} xs={12}>
                            <InputProps stateObject={user} propertyName="username">

                                <TextField
                                    disabled={props.readOnly}
                                    label="Username / E-mail"
                                    required
                                    fullWidth
                                />
                            </InputProps>
                        </Grid>
                        <Grid item sm={6} xs={12}>
                            <InputProps stateObject={user} propertyName="userRoleId">
                                <AutoCompleteField
                                    disabled={props.readOnly}
                                    fullWidth
                                    label="User Role"
                                    dataSource={store.userRoles}
                                />
                            </InputProps>
                        </Grid>
                        <Grid item sm={6} xs={6}>
                            <InputProps stateObject={user} propertyName="active">
                                <CheckBoxWithLabel
                                    disabled={props.readOnly}
                                    label="Is Active?"
                                />
                            </InputProps>
                        </Grid>
                        <Grid item xs={12}>
                            <UserWorkflowRolesView />
                        </Grid>
                        <Grid item xs={12}>
                            <UserIntegrationDataView />
                        </Grid>
                    </Grid>
                </div>
            </MessageDialog>
        </UserViewStoreContext.Provider>
    )
}

export default observer(UserView)
