import React from 'react'
import { DealItemFieldDefinition, countDecimals, itemFieldTextFieldVariant, itemFieldTextFieldStyle } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { formatElementValue } from 'shared-do-not-touch/input-props/field-props'
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'

export const DealItemFieldDefinitionMaxQuantity = {
    field: 'maxQuantity',
    render: (data) => {
        let numberOfDecimalsAlwaysAppearing = countDecimals(data.quantity.value ?? 0)
        if (numberOfDecimalsAlwaysAppearing <= 2) {
            numberOfDecimalsAlwaysAppearing = 2
        } else {
            numberOfDecimalsAlwaysAppearing = 3
        }
        return formatElementValue(data.maxQuantity.value, 'numeric', { maxDecimalPlaces: 3, numberOfDecimalsAlwaysAppearing })
    },
    updatable: dealItem => dealItem.maxQuantity,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} variant='numeric' config={{ maxDecimalPlaces: 3 }}>
            <TextField
                disabled={disabled}
                variant={itemFieldTextFieldVariant}
                style={itemFieldTextFieldStyle(128)}
                autoFocus={autofocus}
                margin="dense"
                label={fieldLabel}
                fullWidth
                InputProps={{
                    endAdornment: <InputAdornment position="start">{store.rootStore.dealTypeConfiguration!!.unitOfMeasure}</InputAdornment>
                }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.maxQuantity, 0),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.maxQuantity.value
        if (value) {
            if (value < 0) {
                return (extendedMessage ? "This field requires a positive number." : 'Has to be positive')
            } else if (isNaN(value)) {
                return (extendedMessage ? "This field requires a valid number." : 'Invalid number')
            }
        }
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (executedItem.maxQuantity.value !== undefined) {
            if (executedItem.quantity.value !== undefined) {
                if (executedItem.maxQuantity.value < executedItem.quantity.value) {
                    return "Can't be more than deal's quantity"
                }
            }
            if (dealItem.maxQuantity.value !== undefined) {
                if (executedItem.maxQuantity.value > dealItem.maxQuantity.value) {
                    return "Can't be more than deal's max quantity"
                }
            }
        }
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.maxQuantity, Number(cellValue?.toString() ?? ""))
    }
} as DealItemFieldDefinition