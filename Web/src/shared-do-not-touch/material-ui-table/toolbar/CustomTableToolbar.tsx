import React from 'react'
import Toolbar from "@material-ui/core/Toolbar"
import IconButton from '@material-ui/core/IconButton'
import SearchIcon from '@material-ui/icons/Search'
import ClearIcon from '@material-ui/icons/Clear'
import Input from "@material-ui/core/Input"
import InputAdornment from "@material-ui/core/InputAdornment"
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import Typography from '@material-ui/core/Typography'
import Box from '@material-ui/core/Box'
import CircularProgress from '@material-ui/core/CircularProgress'
import Tooltip from '@material-ui/core/Tooltip'
import { InputProps } from '../../input-props'
import FullscreenIcon from '@material-ui/icons/Fullscreen'
import FullscreenExitIcon from '@material-ui/icons/FullscreenExit'
import UnfoldMoreIcon from '@material-ui/icons/UnfoldMore'
import UnfoldLessIcon from '@material-ui/icons/UnfoldLess'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const CustomTableToolbar = observer(<RowData extends Object>() => {
    useCustomTableRenderLog("CustomTableToolbar")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(() => ({
        get toolbarContainerStyle(): React.CSSProperties {
            return { position: 'sticky', top: '0px', zIndex: 1, backgroundColor: rootStore.backgroundColor }
        },
        get boxStyle(): React.CSSProperties {
            return { marginLeft: 'auto', display: 'flex', alignItems: 'center' }
        },
        get progressButtonStyle(): React.CSSProperties {
            return { visibility: (rootStore.isLoading && !rootStore.hasRefreshButton ? "visible" : "hidden") }
        },
        get progressTooltipStyle(): React.CSSProperties {
            return store.progressButtonStyle
        },
        get loadingTitle() {
            return rootStore.isLoading && !rootStore.hasRefreshButton ? "Loading..." : ""
        },
        get searchInputProps() {
            return { style: rootStore.sp.styling?.searchFieldInputStyle }
        },
        get toolbarTitle() {
            const title = rootStore.sp.title
            return typeof (title) === 'string'
                ?
                <Typography variant='h6' color='secondary'>
                    {title}
                </Typography>
                :
                title
        },
    }))

    const styling = rootStore.sp.styling || {}
    const ToolbarOverride = rootStore.sp.overrides?.toolbar !== undefined ? rootStore.sp.overrides?.toolbar! : undefined

    return (
        <div ref={rootStore.toolbarDivRef} style={store.toolbarContainerStyle}>
            {ToolbarOverride
                ?
                <ToolbarOverride store={rootStore} />
                :
                (
                    <Toolbar >
                        {store.toolbarTitle}
                        <Box style={store.boxStyle}>
                            <Tooltip title={store.loadingTitle} style={store.progressTooltipStyle} >
                                <IconButton style={store.progressButtonStyle}>
                                    <CircularProgress disableShrink size={24} style={store.progressButtonStyle} />
                                </IconButton>
                            </Tooltip>

                            {rootStore.searchable && (
                                <InputProps stateObject={rootStore} propertyName='searchString' onValueChanged={rootStore.onSearchChanged}>
                                    <Input
                                        style={styling.searchFieldStyle}
                                        inputProps={store.searchInputProps}
                                        startAdornment={<InputAdornment position="start"><SearchIcon /></InputAdornment>}
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <IconButton
                                                    disabled={!rootStore.searchString}
                                                    aria-label="clear"
                                                    onClick={rootStore.clearSearchString}
                                                >
                                                    <ClearIcon />
                                                </IconButton>
                                            </InputAdornment>
                                        }
                                    />
                                </InputProps>
                            )}
                            {rootStore.fullscreenable && (
                                <Tooltip title="Toggle fullscreen" >
                                    <IconButton onClick={rootStore.toggleFullscreenByUser}>
                                        {(rootStore.fullscreen ? <FullscreenExitIcon /> : <FullscreenIcon />)}
                                    </IconButton>
                                </Tooltip>
                            )}
                            {rootStore.sp.userCanToggleSize && (
                                <Tooltip title="Toggle table density" >
                                    <IconButton onClick={rootStore.toggleSize}>
                                        {(rootStore.size === 'small' ? <UnfoldMoreIcon /> : <UnfoldLessIcon />)}
                                    </IconButton>
                                </Tooltip>
                            )}
                            {rootStore.buttons}
                        </Box>
                    </Toolbar>
                )
            }
            { rootStore.sp.renderBetweenToolbarAndTable}
        </div >
    )
})