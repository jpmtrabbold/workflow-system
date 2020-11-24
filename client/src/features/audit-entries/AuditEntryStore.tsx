import { createContext } from "react"
import { AuditEntryGridViewProps } from "./AuditEntriesView"
import { AuditEntryListDto, AuditEntriesListRequest, FunctionalityEnum, AuditEntryTableListDto } from "clients/deal-system-client-definitions"
import { RemoteDataFetchProps } from "shared-do-not-touch/material-ui-table/CustomTable"
import { auditClient } from "clients/deal-system-rest-clients"
import { auditEntryFieldsGridDefinition } from "./tables/fields/AuditEntryFieldsGridDefinition"

export default class AuditEntryStore {
    constructor(sourceProps: AuditEntryGridViewProps) {
        this.sourceProps = sourceProps
        
        
    }
    sourceProps: AuditEntryGridViewProps
    
    rows: AuditEntryListDto[] = []
    totalCount = 0
    auditEntrySelected?: AuditEntryListDto
    
    onTableViewClose = () => {
        this.auditEntrySelected = undefined
        this.auditEntryTableSelected = undefined
    }
    onAuditEntryRowClick = (rowData: AuditEntryListDto) => {
        this.auditEntrySelected = rowData
    }

    auditEntryTableSelected?: AuditEntryTableListDto
    onFieldViewClose = () => this.auditEntryTableSelected = undefined
    onAuditTableRowClick = (rowData: AuditEntryTableListDto) => {
        this.auditEntryTableSelected = rowData
    }

    remoteDataFetchRequest = async (props: RemoteDataFetchProps<AuditEntryListDto>) => {
        this.entrySearchText = props.searchString || ""
        this.tableSearchText = props.searchString || ""
        const ret = await auditClient.list(AuditEntriesListRequest.fromJS({
            entityId: this.sourceProps.entityId,
            pageNumber: props.page,
            pageSize: props.pageSize,
            searchString: props.searchString,
            sortField: props.sortField || "",
            sortOrderAscending: props.sortDirection === 'asc',
            functionalityEnum: this.sourceProps.functionality,
        }))

        this.rows = ret.auditEntries
        this.totalCount = ret.totalRecords || 0
    }

    entrySearchText = ""
    tableSearchText = ""
    
    tableSearch = (row: AuditEntryTableListDto, searchText: string) => {
        this.tableSearchText = searchText || ""
        if (!searchText) {
            return true
        }
        const search = searchText.trim().toUpperCase()
        for (const tableField of row.fields) {
            for (const col of auditEntryFieldsGridDefinition) {
                if (col.field) {
                    let raw = tableField[col.field]
                    if (raw) {
                        raw = raw.toString().trim().toUpperCase()
                        if (raw.includes(search)) {
                            return true
                        }
                    }
                }
                
            }    
        }
        return false
    }
}

export const AuditEntryStoreContext = createContext(new AuditEntryStore({
    onClose: () => null,
    entityId: 0,
    entityName: "",
    functionality: FunctionalityEnum.Deals,
}))