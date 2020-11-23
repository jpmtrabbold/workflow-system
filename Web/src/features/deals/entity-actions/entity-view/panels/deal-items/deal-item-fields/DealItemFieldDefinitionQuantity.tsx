import React from 'react'
import { DealItemFieldDefinition, countDecimals, itemFieldTextFieldVariant, itemFieldTextFieldStyle } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'
import { formatElementValue } from 'shared-do-not-touch/input-props/field-props'

export const DealItemFieldDefinitionQuantity = {
    field: 'quantity',
    render: (data) => {
        const val = data.quantity.value
        if (typeof val !== 'number' || isNaN(val)) {
            return ""
        }
        let numberOfDecimalsAlwaysAppearing = countDecimals(val ?? 0)
        if (numberOfDecimalsAlwaysAppearing <= 2) {
            numberOfDecimalsAlwaysAppearing = 2
        } else {
            numberOfDecimalsAlwaysAppearing = 3
        }
        return formatElementValue(val, 'numeric', { maxDecimalPlaces: 3, numberOfDecimalsAlwaysAppearing })
    },
    updatable: dealItem => dealItem.quantity,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} variant='numeric' config={{ maxDecimalPlaces: 3 }} >
            <TextField
                disabled={disabled}
                variant={itemFieldTextFieldVariant}
                style={itemFieldTextFieldStyle(128)}
                autoFocus={autofocus}
                required
                margin="dense"
                label={fieldLabel}
                fullWidth
                InputProps={{
                    endAdornment: <InputAdornment position="start">{store.rootStore.dealTypeConfiguration!!.unitOfMeasure}</InputAdornment>
                }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.quantity, 0),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.quantity.value
        if (!value || value === 0) {
            return (extendedMessage ? "This field is mandatory." : 'Required')
        } else if (value < 0) {
            return (extendedMessage ? "This field requires a positive number." : 'Has to be positive')
        } else if (isNaN(value)) {
            return (extendedMessage ? "This field requires a valid number." : 'Invalid number')
        }
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (executedItem.quantity.value !== undefined && dealItem.quantity.value !== undefined) {
            if (executedItem.quantity.value > dealItem.quantity.value) {
                return "Can't be more than deal"
            }
        }
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.quantity, Number(cellValue?.toString() ?? ""))
    }
} as DealItemFieldDefinition