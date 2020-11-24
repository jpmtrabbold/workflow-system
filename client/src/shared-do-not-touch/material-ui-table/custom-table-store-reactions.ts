import DisposableReactionsStore from "shared-do-not-touch/mobx-utils/DisposableReactionsStore";
import { CustomTableStore } from "./CustomTableStore";

export const customTableStoreReactions = <RowData extends Object>(store: CustomTableStore<RowData>, disposable: DisposableReactionsStore) => {

    disposable.registerAutorun(() => {
        if (store.hasScrolling) {
            const divMaxHeight = store.maxHeight - store.pagHeight - store.toolbarHeight
            store.setDivMaxHeight(divMaxHeight)
        }
        store.setPaginationBoxMinHeight(store.pagHeight + 'px')
    })

    disposable.registerAutorunAndRunImmediately(() => {
        store.sp.setReloadFunction && store.sp.setReloadFunction(() => store.dataFetch('forced-reload'))
    })

    disposable.registerAutorunAndRunImmediately(() => {
        store.sp.setStoreReference && store.sp.setStoreReference(store)
    })

    disposable.registerAutorunAndRunImmediately(() => {
        if (store.sp.pageSize) {
            !!store.sp.pageSize && (store.setRowsPerPage(store.sp.pageSize))
        }
    })

    disposable.registerAutorunAndRunImmediately(() => {
        for (let index = 0; index < store.sp.columns.length; index++) {
            const column = store.sp.columns[index];
            if (column.defaultSort) {
                store.setColumnSort(index, column.defaultSort!)
                break
            }
        }
    })

    disposable.registerAutorunAndRunImmediately(() => {
        let updateRows = false
        if (store.lastIsRemoteData !== store.isRemoteData) {
            store.lastIsRemoteData = store.isRemoteData
            store.log('Rows are going to be updated. Reason: isRemoteData changed')
            updateRows = true
        }

        // CustomTable holds reference of the individual objects passed as row items,
        // so the comparison is only on the item reference level
        if (store.sp.rows.length !== store.localRows.length) {
            store.log('Rows are going to be updated. Reason: rows length changed')
            updateRows = true
        } else {
            if (store.sp.rows.some((row, index) => store.localRows[index].row !== row)) {
                store.log('Rows are going to be updated. Reason: one of the row items changed')
                updateRows = true
            }
        }

        if (updateRows) {
            if (!store.isRemoteData) {
                store.loading = true
            }
            store.setLocalRows(store.sp.rows)
            if (!store.isRemoteData) {
                store.dataFetch('data-change')
            }
    }
    })

    disposable.registerAutorunAndRunImmediately(() => {
        store.setSearchString(store.sp.initialSearch)
    })

    disposable.registerAutorunAndRunImmediately(() => {
        let count = 0
        if (store.isRemoteData) {
            count = store.sp.totalCount || 0
        } else {
            count = store.sp.rows.length
        }
        if (count > store.maxCount) {
            store.setMaxCount(count)
        }
    })

    disposable.registerAutorunAndRunImmediately(() => {
        if (store.sp.fullscreen !== undefined) {
            store.toggleFullscreen(store.sp.fullscreen)
        }
    })
}