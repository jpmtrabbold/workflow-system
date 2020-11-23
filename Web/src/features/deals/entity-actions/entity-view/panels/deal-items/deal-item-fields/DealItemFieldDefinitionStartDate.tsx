import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import { observer } from 'mobx-react-lite'
import { startDateHigherThanEndDate, compareDates, momentToDateString, isValidMoment } from 'features/shared/helpers/utils'
import moment from 'moment'

export const DealItemFieldDefinitionStartDate = {
    field: 'startDate',
    rawDataForSorting: data => data.startDate.value,
    render: (data) => momentToDateString(data.startDate.value),
    updatable: dealItem => dealItem.startDate,
    componentRenderer: observer(({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) => {
        return (
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
                    style={{ minWidth: 152 }}
                />
            </InputProps>
        )
    }),
    setDefault: (dealItem, store) => {
        const date = moment().add(1, 'month').startOf('month')
        updatable(dealItem.startDate, date)
    },
    validation: (dealItem, store, extendedMessage) => {
        const startDate = dealItem.startDate.value
        const endDate = dealItem.endDate.value
        if (!isValidMoment(startDate))
            return (extendedMessage ? "This field is mandatory." : "Required")

        if (startDateHigherThanEndDate(startDate, endDate)) {
            return "Can't be greater than end date"
        }
        
        const error = store.itemDateErrorMessage(startDate)
        if (error) {
            return error
        }
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (compareDates(executedItem.startDate.value, dealItem.startDate.value) === -1) {
            return "Out of deal dealItem range"
        }

        return store.validateExecutionDatesPeriods(dealItem, executedItem)
    },
    alternativeImportRenderer: (data) => data.importedInvalidStartDate || undefined,
    setFieldFromImport: (cellValue, dealItem, store) => {
        if (typeof cellValue === "string") {
            dealItem.importedInvalidStartDate = cellValue
            return "The date format is invalid."
        }
        updatable(dealItem.startDate, moment(cellValue as Date))
        return
    }
} as DealItemFieldDefinition