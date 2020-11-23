import React from "react"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { ProductListDto } from "../../clients/deal-system-client-definitions"
import Chip from "@material-ui/core/Chip"

export const productGridDefinition: Column<ProductListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    }, 
    {
        title: 'Deal Category',
        field: 'dealCategory',
    }, 
    {
        title: 'Active',
        field: 'active',
        render: (data) => {
            return (
                <span>
                    <Chip label={data.active ? "Yes" : "No"} color='default' variant='outlined'/>
                </span>
            )
        }
    }
]