import FormErrorHandler from "shared-do-not-touch/input-props/form-error-handler"
import { DealItemDto } from "clients/deal-system-client-definitions"
import Excel from 'exceljs'
import { isDate } from "util"
import moment from "moment"
import { observable } from "mobx"

export const itemErrorHandler = (dealItem: DealItemDtoWithErrorHandler, forceCreate = false) : FormErrorHandler<DealItemDto> | undefined => {
    if (!!dealItem.errorHandler) {
        return dealItem.errorHandler
    } else if (forceCreate) {
        dealItem.errorHandler = observable<FormErrorHandler<DealItemDto>>(new FormErrorHandler<DealItemDto>())
        return dealItem.errorHandler
    }
    return undefined
}
interface DealItemDtoWithErrorHandler extends DealItemDto {
    errorHandler?: FormErrorHandler<DealItemDto>
}

export const getMonthIfFullMonthItem = ({ dealItem, endOfMonthDayRounding = 3 }: { dealItem: DealItemDto, endOfMonthDayRounding?: number }) => {
    const start = dealItem.startDate.value?.clone()
    const end = dealItem.endDate.value?.clone()
    const startPeriod = dealItem.halfHourTradingPeriodStart.value
    const endPeriod = dealItem.halfHourTradingPeriodEnd.value
    if (
        !!start && !!end && startPeriod === 1 && endPeriod === 48 &&
        start.isBetween(start.clone().startOf('month'), start.clone().startOf('month'), 'days', '[]') &&
        end.isBetween(end.clone().endOf('month').add(-endOfMonthDayRounding, 'days'), end.clone().endOf('month'), 'days', '[]')
    ) {
        start.startOf('month')
        return start
    }
}

export const addStringWithSeparator = (s: string, ps: string, separator = ';') => {
    if (s) {
        return s + "; " + ps
    } else {
        return ps
    }
}

export const getStringOrMonthString = (cell?: Excel.Cell) : string => {
    if (!cell) {
        return ""
    }
    if (isDate(cell.value)) {
        return moment(cell.value).format('MMM-YY')
    } else {
        return cell.value?.toString() || ""
    }
}