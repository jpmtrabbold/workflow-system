import React, { useContext } from "react"
import { Link } from "react-router-dom"
import { observer } from "mobx-react-lite"

import Drawer from '@material-ui/core/Drawer'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import Divider from '@material-ui/core/Divider'
import ListSubheader from '@material-ui/core/ListSubheader'

import makeStyles from "@material-ui/core/styles/makeStyles"
import { GlobalStoreContext } from "../shared/stores/GlobalStore"
import MenuIcon from '@material-ui/icons/Menu'
import useStore from "shared-do-not-touch/mobx-utils/useStore"

const getPath = () => (window.location.pathname === "/" ? "/deals/" : window.location.pathname)
class NavigationMenuStore {
    path = getPath()
}

const useStyles = makeStyles(theme => ({
    list: {
        width: 250,
    },
    listItem: {
        color: theme.palette.text.primary,
    }
}))

const NavigationMenu = () => {
    const store = useStore(() => new NavigationMenuStore())
    const globalStore = useContext(GlobalStoreContext)

    const classes = useStyles()

    function click() {
        store.path = getPath()
    }
    function activeRoute(routeName?: string) {
        if (routeName === undefined) {
            return false
        }
        return window.location.pathname.indexOf(routeName) > -1 ? true : false
    }

    let first = true
    const menuItems = globalStore.menuItems?.map((menuItem) => {
        let subMenu = (
            <React.Fragment key={menuItem.divisionName}>
                {!first && <Divider />}

                <List
                    subheader={
                        <ListSubheader component="div" id="nested-list-subheader">
                            {menuItem.divisionName}
                        </ListSubheader>
                    }>
                    {menuItem.routes.map((route, index) => {
                        if (route.displaysInMenu) {
                            const IconComponent = route.icon
                            const item = (
                                <ListItem button selected={activeRoute(route.path)}>
                                    <ListItemIcon>
                                        <IconComponent />
                                    </ListItemIcon>

                                    <ListItemText color='inherit' className={classes.listItem} primary={route.name} />
                                </ListItem>
                            )
                            if (!!route.path) {
                                return (
                                    <Link to={route.path} key={route.name} >
                                        {item}
                                    </Link>
                                )
                            } else if (!!route.absolutePath) {
                                return (
                                    <a href={route.absolutePath!} key={route.name}>{item}</a>
                                )
                            }
                            return null
                        }
                        return null
                    })}
                </List>
            </React.Fragment>
        )

        if (first) {
            first = false
        }

        return subMenu
    })

    return (
        <Drawer open={globalStore.drawerOpen} onClose={() => globalStore.drawerOpen = false} onClick={() => globalStore.drawerOpen = false} >
            <div
                className={classes.list}
                role="presentation"
                onClick={click}
                onKeyDown={() => globalStore.drawerOpen = false}
            >
                <List>
                    <ListItem>
                        <ListItemIcon>
                            <MenuIcon onClick={() => globalStore.drawerOpen = false} />
                        </ListItemIcon>
                        <ListItemText primary={"Menu"} />
                    </ListItem>
                </List>
                <Divider />
                {menuItems}
            </div>
        </Drawer>
    )
}
export default observer(NavigationMenu)