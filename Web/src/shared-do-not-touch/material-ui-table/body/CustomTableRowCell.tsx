import React from 'react'
import { observer } from "mobx-react-lite"
import TableCell from "@material-ui/core/TableCell"
import { Column } from "../CustomTable"
import { useStylesRowOpenedNoBorder } from './shared-styles'
import { CustomTableLocalRow } from '../CustomTableLocalRow'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableRowCellProps<RowData extends Object> {
    column: Column<RowData>
    localRow: CustomTableLocalRow<RowData>
    rowIndex: number
}

export const CustomTableRowCell = observer(<RowData extends Object>(props: CustomTableRowCellProps<RowData>) => {
    useCustomTableRenderLog("CustomTableRowCell")
    const classes = useStylesRowOpenedNoBorder()

    const store = useStore(sp => ({
        get cellStyle() {
            let style = {}
            if (sp.column && sp.column.cellStyle) {
                if (typeof (sp.column.cellStyle) === 'function') {
                    style = sp.column.cellStyle(sp.localRow.row)
                } else {
                    style = sp.column.cellStyle
                }
            }
            return style
        },
    }), props)

    if (props.column.hidden) {
        return null
    }

    let render = null
    if (props.column.rendererComponent) {
        const Component = props.column.rendererComponent
        render = <Component data={props.localRow.row} />
    } else if (props.column.render) {
        render = props.column.render(props.localRow.row) || null
    } else if (props.column.field) {
        render = props.localRow.row[props.column.field]
    }

    return (
        <TableCell className={props.localRow.collapsibleOpen ? classes.cellOpened : classes.cellClosed} align={props.column.align} style={store.cellStyle}>
            {render}
        </TableCell>
    )
})