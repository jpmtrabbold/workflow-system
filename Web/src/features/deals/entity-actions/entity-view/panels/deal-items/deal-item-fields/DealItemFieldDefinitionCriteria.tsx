import React from 'react'
import { DealItemFieldDefinition, itemFieldTextFieldVariant, itemFieldTextFieldStyle } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import TextField from '@material-ui/core/TextField'

export const DealItemFieldDefinitionCriteria = {
    field: 'criteria',
    render: (data) => data.criteria.value,
    updatable: dealItem => dealItem.criteria,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} >
            <TextField
                disabled={disabled}
                variant={itemFieldTextFieldVariant}
                style={itemFieldTextFieldStyle(240)}
                autoFocus={autofocus}
                margin="dense"
                label={fieldLabel}
                fullWidth
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.criteria, ""),
    validation: (dealItem, store, extendedMessage) => {
        return undefined
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        updatable(dealItem.criteria, cellValue)
    }
} as DealItemFieldDefinition