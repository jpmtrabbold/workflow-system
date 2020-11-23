import React from 'react'
import { DealItemFieldDefinition, countDecimals, itemFieldTextFieldVariant, itemFieldTextFieldStyle } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { formatElementValue } from 'shared-do-not-touch/input-props/field-props'
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'

export const DealItemFieldDefinitionMinQuantity = {
    field: 'minQuantity',
    render: (data) => {
        let numberOfDecimalsAlwaysAppearing = countDecimals(data.quantity.value ?? 0)
        if (numberOfDecimalsAlwaysAppearing <= 2) {
            numberOfDecimalsAlwaysAppearing = 2
        } else {
            numberOfDecimalsAlwaysAppearing = 3
        }
        return formatElementValue(data.minQuantity.value, 'numeric', { maxDecimalPlaces: 3, numberOfDecimalsAlwaysAppearing })
    },
    updatable: dealItem => dealItem.minQuantity,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} variant='numeric' config={{ maxDecimalPlaces: 3 }} >
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
    setDefault: (dealItem, store) => updatable(dealItem.minQuantity, 0),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.minQuantity.value
        if (value) {
            if (value < 0) {
                return (extendedMessage ? "This field requires a positive number." : 'Has to be positive')
            } else if (isNaN(value)) {
                return (extendedMessage ? "This field requires a valid number." : 'Invalid number')
            }
        }
        if (dealItem.minQuantity && dealItem.maxQuantity && Number(value!) > Number(dealItem.maxQuantity.value!))
            return (extendedMessage ? "The minimum quantity can't be greater than the maximum quantity." : "Can't be greater than max quantity")
    },
    executionValidation: (dealItem, executedItem, store) => {
        if (executedItem.minQuantity.value !== undefined && dealItem.quantity.value !== undefined) {
            if (executedItem.minQuantity.value > dealItem.quantity.value) {
                return "Can't be more than deal's quantity"
            }
        }
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.minQuantity, Number(cellValue?.toString() ?? ""))

    }
} as DealItemFieldDefinition