import React from "react"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { DealCategoryListDto } from "../../clients/deal-system-client-definitions"
import Chip from "@material-ui/core/Chip"

export const dealCategoryGridDefinition: Column<DealCategoryListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    }, 
    {
        title: 'Unit of Measure',
        field: 'unitOfMeasure',
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