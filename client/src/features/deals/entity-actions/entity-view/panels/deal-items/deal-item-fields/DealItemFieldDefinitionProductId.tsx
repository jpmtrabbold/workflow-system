import React from 'react'
import { DealItemFieldDefinition } from "../DealItemFieldDefinition"
import { InputProps } from "shared-do-not-touch/input-props"
import { AutoCompleteField } from "shared-do-not-touch/material-ui-auto-complete"
import updatable from "shared-do-not-touch/utils/updatable"
import ProductViewLoader from "features/products/entity-view/ProductViewLoader"

export const DealItemFieldDefinitionProductId = {
    field: 'productId',
    render: (data) => data.productDescription,
    updatable: dealItem => dealItem.productId,
    componentRenderer: ({ dealItem, fieldLabel, store, fieldName, autofocus, disabled, onValueChanged, errorHandler, updatable }) =>
        <InputProps onValueChanged={onValueChanged} errorHandler={errorHandler} stateObject={dealItem} propertyName={fieldName}>
            <AutoCompleteField
                disabled={disabled}
                autoFocus={autofocus}
                required
                fullWidth
                label={fieldLabel}
                dataSource={store.products}
                initialInputValue={dealItem.productDescription}
                entityView={(props) => <ProductViewLoader {...props} />}
                style={{ minWidth: 192, width: 192 }}
            />
        </InputProps>
    ,
    setDefault: (dealItem, store) => undefined,
    validation: (dealItem, store, extendedMessage) => {
        if (!dealItem.productId.value || dealItem.productId.value <= 0) {
            return (extendedMessage ? "This field is mandatory." : "Required")
        }
        dealItem.productDescription = store.getProductDescription(dealItem.productId.value)
    },
    setFieldFromImport: (cellValue, dealItem, store) => {
        dealItem.productDescription = cellValue?.toString() ?? ""
        const product = store.products.find(n => n.name === dealItem.productDescription)
        if (!product) {
            return "Could not find this product in the database"
        }
        updatable(dealItem.productId, product.id)
    },
    lookups: (store) => store.products,
} as DealItemFieldDefinition