import React, { useContext } from "react"
import { Route } from "react-router"
import { observer } from "mobx-react-lite"
import makeStyles from "@material-ui/core/styles/makeStyles"
import { GlobalStoreContext } from "../shared/stores/GlobalStore"

const useStyles = makeStyles(theme =>
    ({
        content: {
            flexGrow: 1
        },
    }),
)

const MainPageRoutes = () => {
    const globalStore = useContext(GlobalStoreContext)
    const classes = useStyles()

    return (
        <>
            {
                globalStore.menuItems?.map((menuItem) => {
                    return (
                        <React.Fragment key={menuItem.divisionName}>
                            {
                                (menuItem.routes.map((route) => {
                                    const RouteComponent = route.component
                                    return (
                                        <React.Fragment key={route.path ?? route.absolutePath}>
                                            <Route
                                                path={route.path}
                                                exact={route.exact}
                                                render={() => (
                                                    <React.Fragment>
                                                        <div className={classes.content}>
                                                            <RouteComponent />
                                                        </div>
                                                    </React.Fragment>
                                                )}
                                            />
                                        </React.Fragment>

                                    )
                                }))
                            }
                        </React.Fragment>
                    )
                })
            }
        </>
    )
}

export default observer(MainPageRoutes)