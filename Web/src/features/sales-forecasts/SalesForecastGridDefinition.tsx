import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { SalesForecastListDto } from "../../clients/deal-system-client-definitions"
import { formatElementValue } from "shared-do-not-touch/input-props/field-props"
import { momentToMonthYearString } from "features/shared/helpers/utils"

export const salesForecastGridDefinition: Column<SalesForecastListDto>[] = [
    {
        title: 'Month/Year',
        field: 'monthYear',
        render: data => momentToMonthYearString(data.monthYear),
    }, 
    {
        title: 'Volume (MW)',
        field: 'volume',
        render: data => formatElementValue(data.volume, 'numeric')
    },
]