import React from 'react'
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "./CustomTableStore"
import Dialog from '@material-ui/core/Dialog'
import DialogContent from '@material-ui/core/DialogContent'
import Paper from '@material-ui/core/Paper'
import makeStyles from '@material-ui/core/styles/makeStyles'
import useCustomTableRenderLog from './hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableContainerProps {
    children: React.ReactElement
}
const useStyles = makeStyles(theme => ({
    content: {
        display: 'flex',
        flexDirection: 'column',
        flexGrow: 1,
        overflowX: 'auto',
    },
}))

export const CustomTableContainer = observer(<RowData extends Object>(props: CustomTableContainerProps) => {
    useCustomTableRenderLog("CustomTableContainer")
    const classes = useStyles()
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    const store = useStore(() => ({
        get containerStyle() {
            return { minHeight: rootStore.minHeight }
        }
    }))

    const hasContainer = rootStore.fullscreen || (rootStore.sp.hasContainer === undefined || rootStore.sp.hasContainer === true)

    const component = (hasContainer ? (
        <Paper className={classes.content} style={store.containerStyle} square={rootStore.sp.containerPaperSquare} >
            {props.children}
        </Paper>
    ) : (
            <div className={classes.content} style={store.containerStyle} >
                {props.children}
            </div>
        ))

    return (
        rootStore.fullscreen ? (
            <Dialog open={true} fullScreen onClose={rootStore.toggleFullscreenByUser}>
                <DialogContent>
                    {component}
                </DialogContent >
            </Dialog >
        ) : component
    )
})