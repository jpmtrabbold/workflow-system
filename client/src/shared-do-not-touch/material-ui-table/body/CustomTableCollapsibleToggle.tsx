import React from 'react'
import { observer } from "mobx-react-lite"
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown'
import ArrowRightIcon from '@material-ui/icons/ArrowRight'
import { action } from 'mobx'
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import { CustomTableLocalRow } from '../CustomTableLocalRow'
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
import TableCell from '@material-ui/core/TableCell'
import { useStylesRowOpenedNoBorder } from './shared-styles'
import { useTheme } from '@material-ui/core'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableCollapsibleToggleProps<RowData extends Object> {
    localRow: CustomTableLocalRow<RowData>
}

export const CustomTableCollapsibleToggle = observer(<RowData extends Object>(props: CustomTableCollapsibleToggleProps<RowData>) => {
    useCustomTableRenderLog("CustomTableCollapsibleToggle")
    const classes = useStylesRowOpenedNoBorder()
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    const theme = useTheme()

    const store = useStore(sp => ({
        toggleRowCollapsible: action((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
            if (!sp.localRow.collapsibleOpen) {
                sp.localRow.collapsibleOpen = true
            } else {
                sp.localRow.collapsibleOpen = false
            }
            event.stopPropagation()
            event.preventDefault()
        }),
        get tooltip() {
            return sp.localRow.collapsibleConfig.toggleTooltip || "Details"
        },
        get has() {
            return sp.localRow.collapsibleConfig.rowHasCollapsibleDetail || false
        },
        get cellStyle(): React.CSSProperties {
            return { whiteSpace: 'nowrap', width: '1%', padding: theme.spacing(0, 1, 0, 1) }
        },
        get buttonStyle(): React.CSSProperties {
            return { margin: '0px' }
        },
    }), { ...props })

    return !rootStore.hasCollapsibleRowDetail ? null : (
        <TableCell
            className={props.localRow.collapsibleOpen ? classes.cellOpened : classes.cellClosed}
            style={store.cellStyle}
            align='center'
        >
            {store.has && (
                <Tooltip title={store.tooltip}>
                    <IconButton onClick={store.toggleRowCollapsible} style={store.buttonStyle} size={rootStore.size}>
                        {props.localRow.collapsibleOpen ? <ArrowDropDownIcon /> : <ArrowRightIcon />}
                    </IconButton>
                </Tooltip>
            )}
        </TableCell>
    )
})