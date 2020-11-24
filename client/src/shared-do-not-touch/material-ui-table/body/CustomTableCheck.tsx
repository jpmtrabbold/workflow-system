import React from 'react'
import { observer } from "mobx-react-lite"
import Checkbox from '@material-ui/core/Checkbox'
import { action } from 'mobx'
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import { CustomTableLocalRow } from '../CustomTableLocalRow'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface CustomTableCheckProps<RowData extends Object> {
    row: CustomTableLocalRow<RowData>
}

export const CustomTableCheck = observer(<RowData extends Object>(props: CustomTableCheckProps<RowData>) => {

    useCustomTableRenderLog("CustomTableCheck")
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)

    const store = useStore(sp => ({
        onClick: action((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
            rootStore.toggleRowSelection(sp.row)
            event.stopPropagation()
            event.preventDefault()
        }),
        get checkboxStyle(): React.CSSProperties {
            return { margin: '0px', padding: '0px', zIndex: 0 }
        },
    }), props)

    return (
        <Checkbox style={store.checkboxStyle} value={props.row.selected} checked={props.row.selected} onClick={store.onClick} />
    )
})
