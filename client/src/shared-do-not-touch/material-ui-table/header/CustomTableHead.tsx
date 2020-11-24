import React from 'react'
import TableHead from "@material-ui/core/TableHead"
import TableRow from "@material-ui/core/TableRow"
import { CustomTableHeaderCell } from './CustomTableHeaderCell'
import { CustomTableHeaderSelectCell } from './CustomTableHeaderSelectCell'
import { CustomTableHeaderCollapseCell } from './CustomTableHeaderCollapseCell'
import TableCell from '@material-ui/core/TableCell'
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'

export const CustomTableHead = observer(<RowData extends Object>() => {

    useCustomTableRenderLog("CustomTableHead")
    const store = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const styling = store.sp.styling || {}
    const hasActions = store.rowActions.length > 0
    return (
        <TableHead style={styling.headerStyle}>
            <TableRow style={styling.headerRowStyle}>

                <CustomTableHeaderSelectCell />
                <CustomTableHeaderCollapseCell />

                {store.sp.columns.map((column, index) => (
                    <CustomTableHeaderCell
                        key={index}
                        column={column}
                        index={index}
                    />
                ))}

                {hasActions && <TableCell style={store.actionColumnHeaderStyle}>Actions</TableCell>}

            </TableRow>
        </TableHead>
    )
})