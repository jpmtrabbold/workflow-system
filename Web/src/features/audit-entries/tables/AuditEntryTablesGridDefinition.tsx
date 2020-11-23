import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { AuditEntryTableListDto } from "clients/deal-system-client-definitions"

export const auditEntryTablesGridDefinition: Column<AuditEntryTableListDto>[] = [
    {
        title: 'Table Name',
        field: 'tableName',
    }, 
    {
        title: 'Action',
        field: 'action',
    }, 
    {
        title: 'Key(s)',
        field: 'keyValues',
    }, 
]