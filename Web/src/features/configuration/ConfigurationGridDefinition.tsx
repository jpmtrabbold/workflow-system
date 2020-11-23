import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { ConfigurationGroupsListDto } from "../../clients/deal-system-client-definitions"

export const ConfigurationGridDefinition: Column<ConfigurationGroupsListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    },
    {
        title: 'Description',
        field: 'description',
    },
]