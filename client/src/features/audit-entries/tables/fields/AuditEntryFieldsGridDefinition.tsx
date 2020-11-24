import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { AuditEntryFieldListDto } from "clients/deal-system-client-definitions"

export const auditEntryFieldsGridDefinition: Column<AuditEntryFieldListDto>[] = [
    {
        title: 'Field Name',
        field: 'fieldName',
    }, 
    {
        title: 'Old Value',
        field: 'oldValue',
    }, 
    {
        title: 'New Value',
        field: 'newValue',
    }, 
]