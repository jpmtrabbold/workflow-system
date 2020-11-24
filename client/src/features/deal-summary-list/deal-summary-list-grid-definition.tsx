import { DealListDto } from "../../clients/deal-system-client-definitions"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { momentToDateTimeString } from "features/shared/helpers/utils"

export const dealSummaryListGridDefinition: Column<DealListDto>[] = [
    {
        title: 'Deal Number',
        field: 'dealNumber',
    }, 
    {
        title: 'Status',
        field: 'dealStatusName',
    }, 
    {
        title: 'Counterparty',
        field: 'counterpartyName',
    },
    {
        title: 'Deal',
        field: 'dealCategoryName',
    }, 
    {
        title: 'Type',
        field: 'dealTypeName',
    }, 
    {
        title: 'Created By',
        field: 'creationUserName',
    },
    {
        title: 'Created On',
        field: 'creationDate',
        render: (data) => momentToDateTimeString(data.creationDate),
    },
]