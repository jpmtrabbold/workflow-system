import React from 'react'
import { observer } from "mobx-react-lite"
import TableCell from "@material-ui/core/TableCell"
import { CustomTableLocalRow } from "../CustomTableLocalRow"
import { CustomTableCheck } from './CustomTableCheck'
import { useStylesRowOpenedNoBorder } from './shared-styles'
import { useTheme } from '@material-ui/core/styles'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableRowSelectionCellProps<RowData extends Object> {
    localRow: CustomTableLocalRow<RowData>
}

export const CustomTableRowSelectionCell = observer(<RowData extends Object>(props: CustomTableRowSelectionCellProps<RowData>) => {
    useCustomTableRenderLog("CustomTableRowSelectionCell")
    const classes = useStylesRowOpenedNoBorder()
    const theme = useTheme()

    const store = useStore(() => ({
        get cellStyle(): React.CSSProperties {
            return { whiteSpace: 'nowrap', maxWidth: '1%', paddingLeft: theme.spacing(2), paddingRight: theme.spacing(2) }
        }
    }))

    return (
        <TableCell
            className={props.localRow.collapsibleOpen ? classes.cellOpened : classes.cellClosed}
            style={store.cellStyle}
            align='center'
        >
            <CustomTableCheck row={props.localRow} />
        </TableCell>
    )
})