import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import TableCell from "@material-ui/core/TableCell"
import TableSortLabel from "@material-ui/core/TableSortLabel"
import { Column } from "../CustomTable"
import { CustomTableStore, CustomTableStoreContext } from '../CustomTableStore'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableHeaderCellProps<RowData extends Object> {
    column: Column<RowData>
    index?: number
}

export const CustomTableHeaderCell = observer(<RowData extends Object>(props: CustomTableHeaderCellProps<RowData>) => {

    useCustomTableRenderLog("CustomTableHeaderCell")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        onHeaderCellClick: () => {
            rootStore.onHeaderCellClick &&
                sp.index !== undefined &&
                sp.index >= 0 &&
                rootStore.onHeaderCellClick(sp.index)
        },
        get headerCellStyle() {
            let style = (sp.column && sp.column.headerStyle && { ...sp.column.headerStyle }) || {}
            style.position = 'sticky'
            style.borderCollapse = 'separate'
            style.top = rootStore.headerCellTopValue
            style.zIndex = 1
            style.backgroundColor = rootStore.backgroundColor
            return style
        },
        get sortActive() {
            return (sp.index === rootStore.columnSortingIndex)
        },
        get sortDirection() {
            return (rootStore.sp.sortable ? rootStore.columnSortingDirection : false)
        },
    }), props)

    if (props.column.hidden) {
        return null
    }
    let title = null
    if (rootStore.sp.sortable && (props.column.sortable === undefined || props.column.sortable === true)) {
        title = (
            <TableSortLabel
                active={store.sortActive}
                direction={store.sortDirection || undefined}
                onClick={store.onHeaderCellClick}
            >
                {props.column.title}
            </TableSortLabel>
        )
    } else {
        title = props.column.title
    }

    return (
        <TableCell
            variant='head'
            sortDirection={store.sortDirection}
            style={store.headerCellStyle}
            align={props.column.align}
        >
            {title}
        </TableCell>
    )
})