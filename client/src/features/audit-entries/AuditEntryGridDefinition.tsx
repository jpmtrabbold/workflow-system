import { AuditEntryListDto, AuditEntryTypeEnum } from "../../clients/deal-system-client-definitions"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { momentToDateTimeString } from "features/shared/helpers/utils"

export const auditEntriesGridDefinition: Column<AuditEntryListDto>[] = [
    {
        title: 'Audit Entry ID',
        field: 'id',
    }, 
    {
        title: 'User',
        field: 'userName',
    }, 
    {
        title: 'Type',
        render: row => row.type === AuditEntryTypeEnum.Added ? "Added" : "Modified"
    }, 
    {
        title: 'Date / Time',
        field: 'dateTime',
        render: row => momentToDateTimeString(row.dateTime)
    }, 
]