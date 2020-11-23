import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { dayTypeName, dayTypeLookup } from 'features/shared/helpers/lookups'
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'
import { DayTypeEnum } from 'clients/deal-system-client-definitions'

export const DealItemFieldDefinitionDayType = {
    field: 'dayType',
    render: (data) => dayTypeName(data.dayType.value),
    updatable: dealItem => dealItem.dayType,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} >
            <SelectWithLabel
                disabled={disabled}
                autoFocus={autofocus}
                required
                fullWidth
                label={fieldLabel}
                lookups={dayTypeLookup}
                style={{ minWidth: 152 }}
            />
        </InputProps>
    ,
    setDefault: (dealItem, store) => updatable(dealItem.dayType, DayTypeEnum.AllDays),
    validation: (dealItem, store, extendedMessage) => {
        if (!dealItem.dayType.value || dealItem.dayType.value <= 0)
            return (extendedMessage ? "This field is mandatory." : "Required")
    },
    alternativeImportRenderer: (data) => data.dayTypeDescription,
    setFieldFromImport: (cellValue, dealItem, store) => {
        dealItem.dayTypeDescription = cellValue?.toString() ?? ""
        var dayType = dayTypeLookup.find(d => d.name.toLowerCase() === dealItem.dayTypeDescription!.toLowerCase())
        if (!dayType) {
            return "This is not a valid day type."
        }
        updatable(dealItem.dayType, dayType.id)
    },
    lookups: (store) => dayTypeLookup
} as DealItemFieldDefinition