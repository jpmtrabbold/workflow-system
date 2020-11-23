import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import updatable from "shared-do-not-touch/utils/updatable"
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'
import { positionName, positionLookup } from 'features/shared/helpers/lookups'

export const DealItemFieldDefinitionPosition = {
    field: 'position',
    render: (data) => positionName(data.position.value),
    updatable: dealItem => dealItem.position,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName} >
            <SelectWithLabel
                disabled={disabled || (store?.rootStore?.dealTypeConfiguration?.forcePosition ?? false)}
                autoFocus={autofocus}
                required
                fullWidth
                label={fieldLabel}
                lookups={positionLookup}
                style={{ minWidth: 128 }}
            />
        </InputProps>,
    setDefault: (dealItem, store) => updatable(dealItem.position, store.rootStore.dealTypeConfiguration!!.position),
    validation: (dealItem, store, extendedMessage) => {
        return undefined
    },
    alternativeImportRenderer: (data) => data.positionDescription,
    setFieldFromImport: (cellValue, dealItem, store) => {
        dealItem.positionDescription = cellValue?.toString() ?? ""
        var positionLookedUp = positionLookup.find(d => d.name.toLowerCase() === dealItem.positionDescription!.toLowerCase())
        if (!positionLookedUp) {
            return "This is not a valid position."
        }

        if (store?.rootStore?.dealTypeConfiguration) {
            const { forcePosition, position } = store.rootStore.dealTypeConfiguration
            if (forcePosition && position !== positionLookedUp.id) {
                return `This deal type is configure to force position '${positionName(position)}', therefore '${dealItem.positionDescription}' is not accepted.`
            }
        }
        updatable(dealItem.position, positionLookedUp.id)
    },
    lookups: (store) => positionLookup
} as DealItemFieldDefinition