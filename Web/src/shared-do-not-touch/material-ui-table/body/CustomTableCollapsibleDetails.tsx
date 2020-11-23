import React from 'react'
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import { CustomTableLocalRow } from '../CustomTableLocalRow'
import TableCell from '@material-ui/core/TableCell'
import TableBody from '@material-ui/core/TableBody'
import TableRow from '@material-ui/core/TableRow'
import { useStylesRowClosedNoBorder } from './shared-styles'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'


interface CustomTableCollapsibleDetailsProps<RowData extends Object> {
    localRow: CustomTableLocalRow<RowData>
}

export const CustomTableCollapsibleDetails = observer(<RowData extends Object>(props: CustomTableCollapsibleDetailsProps<RowData>) => {
    useCustomTableRenderLog("CustomTableCollapsibleDetails")
    const classes = useStylesRowClosedNoBorder()

    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        get collapsibleDetailStyling() {
            const style = { ...((sp.localRow.collapsibleConfig || {}).collapsibleDetailStyling) } as React.CSSProperties
            if (!sp.localRow.collapsibleOpen) {
                style.paddingTop = '0px'
                style.paddingBottom = '0px'
            }
            style.transitionProperty = 'padding-top, padding-bottom'
            style.transitionDuration = '0.2s'
            return style
        },
        get trStyling() {
            if (sp.localRow.collapsibleOpen) {
                return {}
            } else {
                return {
                    height: '0px'
                }
            }
        },
        get collapsibleRender() {
            if (sp.localRow.collapsibleConfig.render) {
                const CollapsibleConfigRender = sp.localRow.collapsibleConfig.render
                return <CollapsibleConfigRender opened={!!sp.localRow.collapsibleOpen} row={sp.localRow.row} />
            } else {
                return null
            }
        },
        get cellDivStyle(): React.CSSProperties {
            return (sp.localRow.collapsibleOpen ? {
                maxHeight: 10000,
                transition: 'max-height 0.1s',
            } : {
                    maxHeight: 0,
                    boxSizing: 'border-box',
                    overflow: 'hidden',
                    transition: 'max-height 0.1s',
                })
        }
    }), { rootStore, ...props })

    if (!rootStore.hasCollapsibleRowDetail) {
        return null
    }

    const cellClass = (props.localRow.collapsibleOpen ? classes.cellOpened : classes.cellClosed)

    return (
        <TableBody>
            <TableRow>
                <TableCell className={cellClass} style={store.collapsibleDetailStyling} colSpan={rootStore.sp.columns.length + 2}>
                    <div style={store.cellDivStyle}>
                        {store.collapsibleRender}
                    </div>
                </TableCell>
            </TableRow>
        </TableBody>
    )
})