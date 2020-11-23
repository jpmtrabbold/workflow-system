import { DealListDto } from "../../clients/deal-system-client-definitions"
import React from "react"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import Chip from "@material-ui/core/Chip"
import { momentToDateTimeString } from "features/shared/helpers/utils"

export const dealGridDefinition: Column<DealListDto>[] = [
    {
        title: 'Deal Number',
        field: 'dealNumber',
    }, 
    {
        title: 'Deal',
        field: 'dealCategoryName',
        render: (data) => {
            
            return (
                <span>
                    <Chip label={data.dealCategoryName} color='default' variant='outlined'/>
                </span>
            )
        },
    }, 
    {
        title: 'Type',
        field: 'dealTypeName',
    }, 
    {
        title: 'Counterparty',
        field: 'counterpartyName',
    },
    {
        title: 'Deal Status',
        field: 'dealStatusName',
        render: (data) => (
            <span>
                <Chip label={data.dealStatusName} color='default' variant='outlined' />
            </span>
        ),
    }, 
    {
        title: 'Assigned To',
        field: 'assignedTo',
    },
    {
        title: 'Executed?',
        field: 'executed',
        render: (data) => (
            <span>
                <Chip label={data.executed ? 'Yes' : 'No'} color='default' variant='outlined' />
            </span>
        ),
    },
    {
        title: 'Created On',
        field: 'creationDate',
        render: (data) => momentToDateTimeString(data.creationDate),
    },
    {
        title: 'Created By',
        field: 'creationUserName',
    },
]