import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { DealItemFieldsetListDto } from "../../clients/deal-system-client-definitions"

export const itemFieldsetGridDefinition: Column<DealItemFieldsetListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    }, 
    {
        title: 'Description',
        field: 'description',
    }, 
]