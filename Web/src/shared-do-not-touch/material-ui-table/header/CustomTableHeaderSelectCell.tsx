import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import TableCell from "@material-ui/core/TableCell"
import Checkbox from '@material-ui/core/Checkbox'
import { CustomTableStore, CustomTableStoreContext } from '../CustomTableStore'
import Tooltip from '@material-ui/core/Tooltip'
import { useTheme } from '@material-ui/core/styles'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const CustomTableHeaderSelectCell = observer(<RowData extends Object>() => {

    useCustomTableRenderLog("CustomTableHeaderSelectCell")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    const theme = useTheme()

    const store = useStore(sp => ({
        get checkboxHeaderCellStyle() {
            return {
                position: 'sticky',
                top: rootStore.headerCellTopValue,
                zIndex: 1,
                backgroundColor: rootStore.backgroundColor,
                whiteSpace: 'nowrap',
                maxWidth: '1%',
                borderCollapse: 'separate',
                paddingLeft: theme.spacing(2),
                paddingRight: theme.spacing(2),
            } as React.CSSProperties
        },
        get checkboxData() {
            let checkboxProps = undefined
            let title = ""
            if (rootStore.sp.showSelectAllCheckbox) {
                const selected = rootStore.rowsSelectedCount
                const totalRowCount = rootStore.localRows.length
                const selectedPage = rootStore.rowsSelectedCountPage
                const totalRowCountPage = rootStore.filteredSortedAndPagedRows.length
                const pagination = rootStore.sp.pagination

                if (rootStore.isRemoteData) {
                    if (selected === 0) {
                        title = 'No items selected. Click here to select all' + (pagination ? ' from this page' : "")
                        checkboxProps = { checked: false, onClick: rootStore.selectAll }
                    } else if (selected < totalRowCount) {
                        title = 'Some items are selected. Click here to select all' + (pagination ? ' from this page' : "")
                        checkboxProps = { indeterminate: true, checked: true, onClick: rootStore.selectAll }
                    } else {
                        checkboxProps = { checked: true, onClick: rootStore.deselectAll }
                        title = "All items are selected. Click here to deselect all."
                    }
                } else {
                    if (selected === 0) {
                        title = 'No items selected. Click here to select all' + (pagination ? ' from this page' : "")
                        checkboxProps = { checked: false, onClick: rootStore.selectPageAll }
                    } else if (selectedPage < totalRowCountPage) {
                        title = "Some items are already selected. Click here to select all items" + (pagination ? " from this page." : ".")
                        checkboxProps = { indeterminate: true, checked: true, onClick: rootStore.selectPageAll }
                    } else if (selectedPage === totalRowCountPage && selected < totalRowCount) {
                        if (pagination) {
                            title = "Some items are already selected. Click here to select all pages' items."
                        } else {
                            title = "Some items are already selected. Click here to select all items."
                        }
                        checkboxProps = { indeterminate: true, checked: true, onClick: rootStore.selectAll }
                    } else {
                        checkboxProps = { checked: true, onClick: rootStore.deselectAll }
                        title = "All items are selected. Click here to deselect all."
                    }
                }
            }
            return { checkboxProps, title }
        },
        get checkBoxStyle(): React.CSSProperties {
            return { margin: '0px', padding: '0px', zIndex: 1 }
        }
    }), {})

    return (rootStore.sp.selectable ? (
        <TableCell
            variant='head'
            style={store.checkboxHeaderCellStyle}
            align='center'
        >
            {store.checkboxData.checkboxProps ? (
                <Tooltip title={store.checkboxData.title}>
                    <Checkbox
                        style={store.checkBoxStyle}
                        {...store.checkboxData.checkboxProps}
                    />
                </Tooltip>
            ) : "Selected"}
        </TableCell>
    ) : null)
})