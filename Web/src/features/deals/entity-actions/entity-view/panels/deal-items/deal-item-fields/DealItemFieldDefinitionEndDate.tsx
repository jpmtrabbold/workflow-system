import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import { startDateHigherThanEndDate, compareDates, momentToDateString, isValidMoment } from 'features/shared/helpers/utils'
import moment from 'moment'

export const DealItemFieldDefinitionEndDate = {
    field: 'endDate',
    rawDataForSorting: data => data.endDate.value,
    render: (data) => momentToDateString(data.endDate.value),
    updatable: dealItem => dealItem.endDate,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} config={{ elementValueForUndefinedOrNull: () => null }}>
            <KeyboardDatePicker
                disabled={disabled}
                autoFocus={autofocus}
                required
                fullWidth
                format="DD/MM/YYYY"
                label={fieldLabel}
                autoOk
                value={null}
                onChange={() => null}
                minDate={dealItem.startDate.value}
                style={{ minWidth: 152 }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => {
        const date = moment().add(1, 'month').endOf('month').startOf('day')
        updatable(dealItem.endDate, date)
    } ,
    validation: (dealItem, store, extendedMessage) => {
        const startDate = dealItem.startDate.value
        const endDate = dealItem.endDate.value
        if (!isValidMoment(endDate))
            return (extendedMessage ? "This field is mandatory." : "Required")

        if (startDateHigherThanEndDate(startDate, endDate)) {
            return "Start date can't be greater than end date."
        }

        const error = store.itemDateErrorMessage(endDate)
        if (error) {
            return error
        }
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (compareDates(executedItem.endDate.value, dealItem.endDate.value) === 1) {
            return "Out of deal dealItem range"
        }
        return store.validateExecutionDatesPeriods(dealItem, executedItem)
    },
    alternativeImportRenderer: (data) => data.importedInvalidEndDate || undefined,
    setFieldFromImport: (cellValue, dealItem, store) => {
        if (typeof cellValue === "string") {
            dealItem.importedInvalidEndDate = cellValue
            return "The date format is invalid."
        }
        updatable(dealItem.endDate, moment(cellValue as Date))
    }
} as DealItemFieldDefinition