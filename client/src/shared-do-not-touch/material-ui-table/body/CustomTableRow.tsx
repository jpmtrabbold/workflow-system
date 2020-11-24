import React from 'react'
import { observer } from "mobx-react-lite"
import { CustomTableLocalRow } from "../CustomTableLocalRow"
import TableRow from '@material-ui/core/TableRow'
import { CustomTableRowCell } from './CustomTableRowCell'
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import TableBody from '@material-ui/core/TableBody'
import { CustomTableCollapsibleToggle } from './CustomTableCollapsibleToggle'
import { CustomTableRowSelectionCell } from './CustomTableRowSelectionCell'
import { CustomTableRowActionCell } from './CustomTableRowActionCell'
import { CustomTableCollapsibleDetails } from './CustomTableCollapsibleDetails'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableRowProps<RowData extends Object> {
    localRow: CustomTableLocalRow<RowData>
    rowIndex: number
}

export const CustomTableRow = observer(<RowData extends Object>(props: CustomTableRowProps<RowData>) => {

    useCustomTableRenderLog("CustomTableRow")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        onRowClick: () => {
            if (rootStore.sp.singleRowClickSelection) {
                rootStore.selectedRowByClick = sp.localRow.row
            }
            rootStore.sp.onRowClick &&
                rootStore.sp.onRowClick(sp.localRow.row, !!rootStore.sp.singleRowClickSelection)
        },
        get rowStyle() {
            const styling = rootStore.sp.styling || {}
            let style = {}
            if (styling.rowStyle) {
                if (typeof (styling.rowStyle) === 'function') {
                    style = styling.rowStyle(sp.localRow.row)
                } else {
                    style = styling.rowStyle
                }
            }
            return style
        },
        get selected() {
            return sp.localRow.row === rootStore.selectedRowByClick || sp.localRow.selected
        },
    }), props)

    return (
        <>
            <TableBody>
                <TableRow hover={true} onClick={store.onRowClick} style={store.rowStyle} selected={store.selected}>
                    {rootStore.sp.selectable && <CustomTableRowSelectionCell localRow={props.localRow} />}

                    <CustomTableCollapsibleToggle localRow={props.localRow} />

                    {rootStore.sp.columns.map((column, index) => {
                        return <CustomTableRowCell key={index} column={column} localRow={props.localRow} rowIndex={props.rowIndex} />
                    })}

                    <CustomTableRowActionCell localRow={props.localRow} />
                </TableRow>
            </TableBody>

            <CustomTableCollapsibleDetails localRow={props.localRow} />
        </>
    )
})