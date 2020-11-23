import React from "react"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { DealTypeListDto } from "../../clients/deal-system-client-definitions"
import Chip from "@material-ui/core/Chip"

export const dealTypeGridDefinition: Column<DealTypeListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    }, 
    {
        title: 'Default Position',
        field: 'positionName',
    }, 
    {
        title: 'Unit of Measure',
        field: 'unitOfMeasure',
    }, 
    {
        title: 'Deal Item Fieldset',
        field: 'dealItemFieldsetName',
    }, 
    {
        title: 'Workflow Set',
        field: 'workflowSetName',
    }, 
    {
        title: 'Active',
        field: 'activeDescription',
        render: (data: DealTypeListDto) => {
            return (
                <span>
                    <Chip label={data.activeDescription} color='default' variant='outlined'/>
                </span>
            )
        }
    }
]