import { CustomTableCollapsibleDetailConfig } from "./CustomTable"

export class CustomTableLocalRow<RowData extends Object> {
    constructor(id: number, row: RowData, selected: boolean = false, collapsibleConfig: CustomTableCollapsibleDetailConfig<RowData> = {}) {
        this.tableRowId = id
        this.row = row
        this.selected = selected
        this.collapsibleConfig = collapsibleConfig
        if (collapsibleConfig.rowHasCollapsibleDetail) {
            this.collapsibleOpen = (collapsibleConfig.rowStartsOpened !== undefined ? collapsibleConfig.rowStartsOpened : false)
        }
    }
    tableRowId: number
    row: RowData
    selected: boolean
    collapsibleOpen?: boolean
    collapsibleConfig: CustomTableCollapsibleDetailConfig<RowData>
}