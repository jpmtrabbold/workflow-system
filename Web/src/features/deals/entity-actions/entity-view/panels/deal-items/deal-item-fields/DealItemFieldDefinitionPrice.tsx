import React from 'react'
import { DealItemFieldDefinition, itemFieldTextFieldVariant, itemFieldTextFieldStyle } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'
import { formatElementValue } from 'shared-do-not-touch/input-props/field-props'
import { PositionEnum } from 'clients/deal-system-client-definitions'

export const DealItemFieldDefinitionPrice = {
    field: 'price',
    render: (data) => {
        if (data.price.value) {
            return `$ ${formatElementValue(data.price.value, 'numeric', { maxDecimalPlaces: 2, numberOfDecimalsAlwaysAppearing: 2 })}`
        } else {
            return `$ 0.00`
        }
    },
    rawDataForSorting: (data) => data.price.value || 0,
    updatable: dealItem => dealItem.price,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} variant='numeric' config={{ maxDecimalPlaces: 2 }} >
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
                    startAdornment: <InputAdornment position="start">$</InputAdornment>
                }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.price, 0),
    validation: (dealItem, store, extendedMessage) => {
        const value = dealItem.price.value
        if (value && isNaN(value)) {
            return (extendedMessage ? "This field requires a valid number." : 'Invalid number')
        } else if (value === undefined || (value as any) === '') {
            return (extendedMessage ? "This field is mandatory." : 'Required')
        } else if (value < 0) {
            return (extendedMessage ? "The price needs to be a positive number." : 'Has to be positive')
        }

    },
    executionValidation: (dealItem, executedItem, store) => {
        if (executedItem.price.value !== undefined && dealItem.price.value !== undefined) {
            if (dealItem.position.value === PositionEnum.Buy && executedItem.price.value > dealItem.price.value) {
                return "Can't buy for higher price than deal"
            } else if (dealItem.position.value === PositionEnum.Sell && executedItem.price.value < dealItem.price.value) {
                return "Can't sell for lower price than deal"
            }
        }
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        const val = cellValue?.toString() ?? ""
        if (val === "") {
            dealItem.importedInvalidPrice = val
            return "Price is mandatory"
        }
        updatable(dealItem.price, Number(val))
        if (dealItem.price.value === 0) {
            dealItem.warnings!.push("Deal Item price is 0 (zero)")
        }
        return
    },
    alternativeImportRenderer: (data) => {
        if (data.importedInvalidPrice !== undefined) {
            return data.importedInvalidPrice
        }
    },
} as DealItemFieldDefinition