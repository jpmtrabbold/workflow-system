import React from 'react'
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import { CustomTableRow } from './CustomTableRow'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'

export const CustomTableBody = observer(<RowData extends Object>() => {
    const rootStore = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    useCustomTableRenderLog("CustomTableBody")

    return (
        <>
            {rootStore.rows.map((row, index) => {
                return <CustomTableRow
                    key={row.tableRowId}
                    localRow={row}
                    rowIndex={index}
                />
            })}
        </>
    )
})