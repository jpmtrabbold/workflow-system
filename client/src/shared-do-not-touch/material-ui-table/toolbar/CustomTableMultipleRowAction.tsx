import React, { useContext } from 'react'
import { observer } from "mobx-react-lite"
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableActionProps<RowData extends Object> {
    title: string
    callback: (data: RowData[]) => any
    icon: (() => React.ReactElement<any>)
}

export const CustomTableMultipleRowAction = observer(<RowData extends Object>(props: CustomTableActionProps<RowData>) => {

    useCustomTableRenderLog("CustomTableMultipleRowAction")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        callback: () => {
            const selectedRows = rootStore.localRows.filter(l => l.selected).map(l => l.row)
            sp.callback(selectedRows)
        },
        get icon() {
            return sp.icon()
        },
    }), props)

    return (
        <Tooltip title={props.title}>
            <IconButton onClick={store.callback} >
                {store.icon}
            </IconButton>
        </Tooltip>
    )
})
