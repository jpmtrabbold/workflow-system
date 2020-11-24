import React, { useContext } from 'react'

import { observer } from 'mobx-react-lite'
import NavigationMenu from './NavigationMenu'
import MainPageRoutes from './MainPageRoutes'
import MenuIcon from '@material-ui/icons/Menu'
import LoggedUserControl from './LoggedUserControl'
import { GlobalStoreContext } from '../shared/stores/GlobalStore'
import makeStyles from "@material-ui/core/styles/makeStyles"
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { AppBarContainer } from 'shared-do-not-touch/material-ui-app-bar-container'
import useInitialMount from 'shared-do-not-touch/hooks/useInitialMount'

const useStyles = makeStyles(theme => ({
    image: {
        width: theme.spacing(3.5),
        height: theme.spacing(3.5),
        marginRight: theme.spacing(1),
    },
}))

const MainPage = () => {
    const classes = useStyles()
    const globalStore = useContext(GlobalStoreContext)
    console.log('globalStore2', globalStore)
    useInitialMount(() => {
        globalStore.getMenuItems()
    })
    
    if (globalStore.invalidLoginMessage) {
        return <MessageDialog
            title='Access Restricted'
            content={globalStore.invalidLoginMessage}
            actions={[{ name: 'Reload Page', callback: () => window.location.reload() }]}
        />
    }
    
    return (
        <AppBarContainer
            title={<>
                <img className={classes.image} src={process.env.PUBLIC_URL + '/favicon.ico'} alt="Logo" />
                {globalStore.appBarTitle}
            </>}
            leftButtonOnClick={() => globalStore.drawerOpen = true}
            leftButtonIcon={<MenuIcon />}
            rightButtons={[<LoggedUserControl key={1} />]}
        >
            <NavigationMenu key={2} />
            <MainPageRoutes key={3} />
        </AppBarContainer>
    )
}
export default observer(MainPage)