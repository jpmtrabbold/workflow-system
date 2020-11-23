import React, { useCallback, useContext } from 'react'
import { DealPanel, DealViewStoreContext } from '../DealViewStore'
import { observer } from 'mobx-react-lite'
import Typography from '@material-ui/core/Typography'
import Accordion from '@material-ui/core/Accordion'
import AccordionDetails from '@material-ui/core/AccordionDetails'
import AccordionSummary from '@material-ui/core/AccordionSummary'
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'
import makeStyles from "@material-ui/core/styles/makeStyles"
import IconButton from '@material-ui/core/IconButton'
import FullscreenIcon from '@material-ui/icons/Fullscreen'
import FullscreenExitIcon from '@material-ui/icons/FullscreenExit'
import Box from '@material-ui/core/Box'
import Dialog from "@material-ui/core/Dialog"
import DialogContent from "@material-ui/core/DialogContent"
import Tooltip from '@material-ui/core/Tooltip'

const useStyles = makeStyles(theme => ({
    expansionPanel: {
        boxShadow: theme.shadows[2]
    },
    panelSummaryRoot: {
        marginBottom: 0
        // padding: theme.spacing(0, 1, 0, 0),
        // margin: theme.spacing(0, 0, 0, 0),
        // marginTop: 0,
        // minHeight: 0
    },
    panelSummaryContent: {
        margin: theme.spacing(0.5, 0.5, 0.5, 0.5)
    },
    typography: {
        marginTop: theme.spacing(1),
        marginBottom: theme.spacing(1),
    }
}))

interface DealViewPanelProps {
    panel: DealPanel
}

export const DealViewPanel = observer(({ panel }: DealViewPanelProps) => {
    const store = useContext(DealViewStoreContext)
    const classes = useStyles()
    const onPanelClick = useCallback(() => {
        panel.expanded = !panel.expanded
    }, [panel])

    const onFullscreenClick = useCallback((event?: any, reason?: "backdropClick" | "escapeKeyDown") => {
        panel.fullscreen = !panel.fullscreen
        !!event && !!event.stopPropagation && event.stopPropagation()
    }, [panel])

    if (panel.visible) {
        const Component = panel.component
        const localFullscreen = !!(panel.fullscreen && !panel.hasOwnFullscreenMethod)
        const Adornment = panel.titleAdornment
        const expansionPanel = (
            <Accordion className={classes.expansionPanel} expanded={panel.expanded || localFullscreen}>
                <AccordionSummary onClick={onPanelClick}
                    expandIcon={(localFullscreen ? null : <ExpandMoreIcon />)}
                    classes={{
                        root: classes.panelSummaryRoot,
                        expanded: classes.panelSummaryRoot,
                        content: classes.panelSummaryContent,
                    }}
                >
                    <Typography variant='h6' classes={{ root: classes.typography }}>
                        {panel.title + (localFullscreen && store.deal.dealNumber ? ` - ${store.deal.dealNumber}` : "")}
                    </Typography>
                    {!!Adornment && (
                        <Box style={{ marginRight: 'auto', display: 'flex', alignItems: 'center' }}>
                            <Adornment />
                        </Box>
                    )}
                    <Box style={{ marginLeft: 'auto', display: 'flex', alignItems: 'center' }}>
                        <Tooltip title={localFullscreen ? "Exit full screen" : "Maximize this panel to full screen"}>
                            <IconButton onClick={onFullscreenClick}>
                                {localFullscreen ? <FullscreenExitIcon /> : <FullscreenIcon />}
                            </IconButton>
                        </Tooltip>
                    </Box>

                </AccordionSummary>
                <AccordionDetails >
                    <Component fullscreen={panel.fullscreen} fullscreenExited={onFullscreenClick} />
                </AccordionDetails>
            </Accordion>
        )
        if (localFullscreen) {
            return (
                <Dialog open={true} fullScreen onClose={onFullscreenClick}>
                    <DialogContent>
                        {expansionPanel}
                    </DialogContent>
                </Dialog>
            )
        } else {
            return <>
                {expansionPanel}
            </>
        }
    }
    return null
})