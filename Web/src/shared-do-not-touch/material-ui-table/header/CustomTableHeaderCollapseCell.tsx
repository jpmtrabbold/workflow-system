import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import TableCell from "@material-ui/core/TableCell"
import { CustomTableStore, CustomTableStoreContext } from '../CustomTableStore'
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown'
import ArrowRightIcon from '@material-ui/icons/ArrowRight'
import { useTheme } from '@material-ui/core'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const CustomTableHeaderCollapseCell = observer(<RowData extends Object>() => {

    useCustomTableRenderLog("CustomTableHeaderCollapseCell")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    const theme = useTheme()

    const store = useStore(sp => ({
        get headerCellStyle() {
            return {
                position: 'sticky',
                top: rootStore.headerCellTopValue,
                zIndex: 1,
                backgroundColor: rootStore.backgroundColor,
                whiteSpace: 'nowrap',
                width: '1%',
                padding: theme.spacing(0, 1, 0, 1),
                borderCollapse: 'separate',
            } as React.CSSProperties
        },
        get open() {
            if (rootStore.sp.showCollapseAllButton === false) {
                return undefined
            }

            for (const localRow of rootStore.localRows) {
                if (localRow.collapsibleConfig.rowHasCollapsibleDetail) {
                    return localRow.collapsibleOpen
                }
            }

            return undefined
        },
        get tooltip() {
            return (store.open ? "Close All" : "Open All")
        },
        toggle() {
            rootStore.loading = true
            setTimeout(() => {
                const open = !store.open
                for (const localRow of rootStore.localRows) {
                    if (localRow.collapsibleConfig.rowHasCollapsibleDetail) {
                        localRow.collapsibleOpen = open
                    }
                }
                rootStore.loading = false
            });
        },
        get buttonStyle(): React.CSSProperties {
            return { margin: '0px' }
        }
    }), {})

    return (rootStore.hasCollapsibleRowDetail ? (
        <TableCell
            variant='head'
            style={store.headerCellStyle}
            align='center'
        >
            {store.open !== undefined && (
                <Tooltip title={store.tooltip}>
                    <IconButton onClick={store.toggle} style={store.buttonStyle} size={rootStore.size}>
                        {store.open ? <ArrowDropDownIcon /> : <ArrowRightIcon />}
                    </IconButton>
                </Tooltip>
            )}
        </TableCell>
    ) : null)
})
