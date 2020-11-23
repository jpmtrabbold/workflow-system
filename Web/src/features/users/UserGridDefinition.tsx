import React from "react"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { UserListDto } from "../../clients/deal-system-client-definitions"
import Chip from "@material-ui/core/Chip"

export const userGridDefinition: Column<UserListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    },
    {
        title: 'Username / e-mail',
        field: 'username',
    },
    {
        title: 'User Role',
        field: 'userRoleName',
    },
    {
        title: 'Workflow Role',
        field: 'workflowRoleNames',
        render: user => user.workflowRoleNames.reduce((previous, current) => previous + (!!previous ? ", " : "") + current, "")
    },
    {
        title: 'Active',
        field: 'active',
        render: (data) => (
            <span>
                <Chip label={data.active ? "Yes" : "No"} color='default' variant='outlined' />
            </span>
        )
    }
]