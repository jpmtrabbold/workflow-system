import React from "react"
import { CounterpartyListDto } from "../../clients/deal-system-client-definitions"
import Chip from "@material-ui/core/Chip"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { formatElementValue } from "shared-do-not-touch/input-props/field-props"
import { momentToDateString } from "features/shared/helpers/utils"

export const counterpartyGridDefinition: Column<CounterpartyListDto>[] = [
    {
        title: 'Name',
        field: 'name',
    }, 
    {
        title: 'Code',
        field: 'code',
    }, 
    {
        title: 'Expiry Date',
        field: 'expiryDate',
        render: (data) => momentToDateString(data.expiryDate)
    }, 
    {
        title: 'Exposure Limit',
        field: 'exposureLimit',
        render: (data) => `$ ${formatElementValue(data.exposureLimit, 'numeric')}`,
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
    }, 
]