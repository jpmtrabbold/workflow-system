import React from "react"
import { useObserver, } from "mobx-react-lite"
import useWindowSize from "../hooks/useWindowSize"
import { GridViewStore } from "."
import { GridViewListRequest, GridViewListResponse } from "./GridViewStore"
import { CustomTable } from "../material-ui-table"
import useTheme from "@material-ui/core/styles/useTheme"

interface GridViewProps<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse> {
    store: GridViewStore<ListType, ListRequestType, ListResponseType>
    title: string | React.ReactElement
    searchable?: boolean
    height?: number
}

export function GridView<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse>
    (props: GridViewProps<ListType, ListRequestType, ListResponseType>) {
    const theme = useTheme()
    const [store, title, searchable] = useObserver(() => [props.store, props.title, props.searchable])
    const windowSize = useWindowSize()
    const height = props.height || windowSize.height - theme.spacing(7)

    return useObserver(() =>
        <CustomTable
            maxHeight={height}
            minHeight={height}
            columns={store.gridDefinition}
            rows={store.rows}
            totalCount={store.totalCount}
            remoteDataFetchRequest={store.fetchRequest}
            title={title}
            actions={store.tableActions}
            pageSize={store.pageSize}
            searchDebounceInterval={500}
            sortable={true}
            setReloadFunction={store.setReloadFunction}
            searchable={searchable}
            containerPaperSquare={true}
            additionalToolbarControls={store.additionalToolbarElements}
        />
    )
}