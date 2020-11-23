import React from 'react'
import { observer } from "mobx-react-lite"
import { useContext } from "react"
import { CustomTableStoreContext, CustomTableStore } from "../CustomTableStore"
import Typography from '@material-ui/core/Typography'
import Box from '@material-ui/core/Box'
import useCustomTableRenderLog from '../hooks/useCustomTableRenderLog'

export const CustomTableBodyNoResults = observer(<RowData extends Object>() => {
    const store = useContext<CustomTableStore<RowData>>(CustomTableStoreContext as any)
    useCustomTableRenderLog("CustomTableBodyNoResults")

    if (store.rows.length === 0) {
        return (
            <Box textAlign="center" margin={3}>
                <Typography variant='subtitle2'>
                    {store.isLoading || store.waitingForRowsToCome ? "Loading..." : "No results to be displayed"}
                </Typography>
            </Box>
        )
    }
    return null
})