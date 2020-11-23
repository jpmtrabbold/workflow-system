import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { observer } from 'mobx-react-lite'
import { generateNumbersArray } from '../DealItemStore'
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const DealItemFieldDefinitionTPEnd = {
    field: 'halfHourTradingPeriodEnd',
    render: (data) => data.halfHourTradingPeriodEnd.value,
    updatable: dealItem => dealItem.halfHourTradingPeriodEnd,
    componentRenderer: observer(({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) => {
        const localStore = useStore(sp => ({
            get endPeriodLookup() {
                return generateNumbersArray(48, sp.dealItem.halfHourTradingPeriodStart.value).map(item => ({ id: item }))
            }
        }), { dealItem })
        return (
            <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} >
                <SelectWithLabel
                    disabled={disabled}
                    autoFocus={autofocus}
                    required
                    fullWidth
                    label={fieldLabel}
                    lookups={localStore.endPeriodLookup}
                    style={{ minWidth: 128 }}
                />
            </InputProps>
        )
    }),
    setDefault: (dealItem, store) => updatable(dealItem.halfHourTradingPeriodEnd, 48),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.halfHourTradingPeriodEnd.value
        if (!value) {
            return (extendedMessage ? "This field is mandatory." : "Required")
        } else if (value < dealItem.halfHourTradingPeriodStart.value!) {
            return "Can't be less than the start period"
        } else if (value && value > 48) {
            return (extendedMessage ? "The maximum value for this field is 48" : "Can't be more than 48")
        } else if ((value % 1) !== 0) {
            return "Can't have decimal places"
        }

    },
    executionValidation: (dealItem, executedItem, store) => {
        if (!!executedItem.halfHourTradingPeriodEnd.value && !!dealItem.halfHourTradingPeriodEnd.value) {
            if (executedItem.halfHourTradingPeriodEnd.value > dealItem.halfHourTradingPeriodEnd.value) {
                return "Out of deal dealItem range"
            }
        }

        return store.validateExecutionDatesPeriods(dealItem, executedItem)
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.halfHourTradingPeriodEnd, Number(cellValue?.toString() ?? ""))
    }
} as DealItemFieldDefinition