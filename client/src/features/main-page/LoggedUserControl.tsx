import React, { useContext } from "react"
import { authContext } from "../../clients/azure-authentication"

import { observer } from "mobx-react-lite"
import { GlobalStoreContext } from "../shared/stores/GlobalStore"

import IconButton from "@material-ui/core/IconButton"
import Paper from "@material-ui/core/Paper"
import AccountCircle from '@material-ui/icons/AccountCircle'
import Popper from '@material-ui/core/Popper'
import ClickAwayListener from '@material-ui/core/ClickAwayListener'
import MenuItem from '@material-ui/core/MenuItem'
import MenuList from '@material-ui/core/MenuList'
import makeStyles from "@material-ui/core/styles/makeStyles"

const useStyles = makeStyles(theme => ({
    popper: {
        zIndex: 9999999,
    },
}))

const LoggedUserControl = () => {
    const classes = useStyles()
    const globalStore = useContext(GlobalStoreContext)
    const anchorRef = React.useRef<HTMLButtonElement>(null)
    
    return (
        <>
            <IconButton
                ref={anchorRef}
                color="inherit"
                aria-label="Login"
                onClick={globalStore.openUserPopper} >

                <AccountCircle />
            </IconButton>

            <Popper open={globalStore.userPopperOpen} anchorEl={anchorRef.current} className={classes.popper}>
                <Paper>
                    <ClickAwayListener onClickAway={globalStore.closeUserPopper}>
                        <MenuList>
                            <MenuItem key={0} onClick={globalStore.closeUserPopper}>{globalStore.username}</MenuItem>
                            <MenuItem key={1} onClick={() => authContext.logOut()}>Logout</MenuItem>
                            <MenuItem key={2} onClick={globalStore.toggleTheme}>{globalStore.darkTheme ? "Turn lights on" : "Turn lights off"}</MenuItem>
                        </MenuList>
                    </ClickAwayListener>
                </Paper>

            </Popper>
        </>
    )
}
export default observer(LoggedUserControl)