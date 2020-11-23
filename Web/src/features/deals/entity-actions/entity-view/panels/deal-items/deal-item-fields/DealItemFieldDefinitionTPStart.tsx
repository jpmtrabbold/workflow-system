import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'

export const DealItemFieldDefinitionTPStart = {
    field: 'halfHourTradingPeriodStart',
    render: (data) => data.halfHourTradingPeriodStart.value,
    updatable: dealItem => dealItem.halfHourTradingPeriodStart,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} >
            <SelectWithLabel
                disabled={disabled}
                autoFocus={autofocus}
                required
                fullWidth
                label={fieldLabel}
                lookups={store.startPeriodLookup}
                style={{ minWidth: 128 }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.halfHourTradingPeriodStart, 1),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.halfHourTradingPeriodStart.value
        if (!value)
            return (extendedMessage ? "This field is mandatory." : "Required")

        if (value > dealItem.halfHourTradingPeriodEnd.value!) {
            return "Can't be greater than end period"
        }

        if ((value % 1) !== 0) {
            return "Can't have decimal places"
        }
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (!!executedItem.halfHourTradingPeriodStart.value && !!dealItem.halfHourTradingPeriodStart.value) {
            if (executedItem.halfHourTradingPeriodStart.value < dealItem.halfHourTradingPeriodStart.value) {
                return "Out of deal dealItem range"
            }
        }

        return store.validateExecutionDatesPeriods(dealItem, executedItem)
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.halfHourTradingPeriodStart, Number(cellValue?.toString() ?? ""))
    }
} as DealItemFieldDefinition