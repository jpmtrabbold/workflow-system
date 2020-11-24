import clsx from 'clsx'
import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import { RowActionType } from '../CustomTable'
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import TableCell from '@material-ui/core/TableCell'
import { useStylesRowOpenedNoBorder } from './shared-styles'
import { CustomTableLocalRow } from '../CustomTableLocalRow'
import { SplitButton } from '../../material-ui-split-button'
import { makeStyles } from '@material-ui/core/styles'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableRowActionCellProps<RowData extends Object> {
    localRow: CustomTableLocalRow<RowData>
}

const useStyles = makeStyles(theme => {
    return {
        actionCell: {
            width: '1px',
            paddingLeft: theme.spacing(4),
            paddingRight: theme.spacing(4),
        },
    }
})

export const CustomTableRowActionCell = observer(<RowData extends Object>(props: CustomTableRowActionCellProps<RowData>) => {
    useCustomTableRenderLog("CustomTableRowActionCell")
    const sharedClasses = useStylesRowOpenedNoBorder()
    const classes = useStyles()

    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        get hasRowActions() {
            return sp.actions.length > 0
        },
        get actionCellContent() {
            const options = []
            if (sp.actions) {
                for (const action of sp.actions) {
                    let realAction: RowActionType<RowData> | undefined
                    if (typeof (action) !== "function") {
                        realAction = action
                    } else {
                        realAction = action(sp.localRow.row)
                    }
                    if (!!realAction) {
                        options.push({
                            title: realAction.title || "",
                            callback: (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
                                realAction!.callback(sp.localRow.row)
                                event.stopPropagation()
                            },
                            hasPriority: realAction.hasPriority,
                        })
                    }
                }
            }
            if (options.length > 0) {
                return (
                    <SplitButton options={options} />
                )
            }
            return null
        },
        get classes() {
            return clsx(sp.localRow.collapsibleOpen ? sharedClasses.cellOpened : sharedClasses.cellClosed, classes.actionCell)
        }
    }), { actions: rootStore.rowActions, ...props })

    if (!store.hasRowActions) {
        return null
    }
    return (
        <TableCell className={store.classes} >
            {store.actionCellContent}
        </TableCell>
    )
})
