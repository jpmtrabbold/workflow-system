import React from 'react'
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { DealItemStore } from './DealItemStore'
import Excel from 'exceljs'
import { ImportedItem } from './import/DealItemImportStore'
import { DealItemDto } from '../../../../../../clients/deal-system-client-definitions'
import { OnValueChangedType, IUpdatable } from 'shared-do-not-touch/input-props/field-props'
import { DealItemExecutionStore } from './execution/DealItemExecutionStore'
import FormErrorHandler from 'shared-do-not-touch/input-props/form-error-handler'
import { DealItemFieldDefinitionProductId } from './deal-item-fields/DealItemFieldDefinitionProductId'
import { DealItemFieldDefinitionDayType } from './deal-item-fields/DealItemFieldDefinitionDayType'
import { DealItemFieldDefinitionStartDate } from './deal-item-fields/DealItemFieldDefinitionStartDate'
import { DealItemFieldDefinitionTPStart } from './deal-item-fields/DealItemFieldDefinitionTPStart'
import { DealItemFieldDefinitionEndDate } from './deal-item-fields/DealItemFieldDefinitionEndDate'
import { DealItemFieldDefinitionTPEnd } from './deal-item-fields/DealItemFieldDefinitionTPEnd'
import { DealItemFieldDefinitionPrice } from './deal-item-fields/DealItemFieldDefinitionPrice'
import { DealItemFieldDefinitionQuantity } from './deal-item-fields/DealItemFieldDefinitionQuantity'
import { DealItemFieldDefinitionMinQuantity } from './deal-item-fields/DealItemFieldDefinitionMinQuantity'
import { DealItemFieldDefinitionMaxQuantity } from './deal-item-fields/DealItemFieldDefinitionMaxQuantity'
import { DealItemFieldDefinitionPosition } from './deal-item-fields/DealItemFieldDefinitionPosition'
import { DealItemFieldDefinitionCriteria } from './deal-item-fields/DealItemFieldDefinitionCriteria'

export interface ILookup {
    id: number
    name: string
}

type ColumnWithRequiredField<T extends Object> = Omit<Column<T>, 'field' | 'title' | 'render'>
    & {
        field: keyof T
        title?: string
        render?: (data: DealItemDto) => string | number | undefined
    }

export interface DealItemFieldDefinition extends ColumnWithRequiredField<DealItemDto> {
    inputLabel?: string
    componentRenderer: DealItemFieldRendererType,
    validation?: ItemValidationFunction,
    executionValidation?: ExecutedItemValidationFunction,
    setDefault?: (dealItem: DealItemDto, store: DealItemStore) => any,
    setFieldFromImport: (cellValue: Excel.CellValue, dealItem: ImportedItem, store: DealItemStore) => string | undefined | void,
    alternativeImportRenderer?: ((data: ImportedItem) => any) | undefined,
    lookups?: (store: DealItemStore) => ILookup[]
    execution?: boolean | ((store: DealItemStore) => boolean)
    updatable: (dealItem: DealItemDto) => IUpdatable
}
interface DealItemFieldRendererTypeProps {
    dealItem: DealItemDto,
    fieldLabel: string,
    store: DealItemStore,
    fieldName: keyof DealItemDto,
    autofocus?: boolean,
    execution?: boolean,
    disabled?: boolean,
    onValueChanged?: OnValueChangedType<{}>,
    errorHandler?: FormErrorHandler<DealItemDto>,
    updatable: IUpdatable
}
type DealItemFieldRendererType = React.FunctionComponent<DealItemFieldRendererTypeProps>

type ItemValidationFunction = (dealItem: DealItemDto, store: DealItemStore, extendedMessage?: boolean) => string | undefined | void
type ExecutedItemValidationFunction = (parentItem: DealItemDto, executedItem: DealItemDto, store: DealItemExecutionStore) => string | undefined | void

export const itemFieldTextFieldVariant = 'standard'
export const itemFieldTextFieldStyle = (minWidth?: number) => ({
    marginTop: 3,
    minWidth: minWidth || 152,
})

export const dealDealItemFieldDefinition: DealItemFieldDefinition[] = [
    DealItemFieldDefinitionProductId,
    DealItemFieldDefinitionDayType,
    DealItemFieldDefinitionStartDate,
    DealItemFieldDefinitionTPStart,
    DealItemFieldDefinitionEndDate,
    DealItemFieldDefinitionTPEnd,
    DealItemFieldDefinitionPrice,
    DealItemFieldDefinitionQuantity,
    DealItemFieldDefinitionMinQuantity,
    DealItemFieldDefinitionMaxQuantity,
    DealItemFieldDefinitionPosition,
    DealItemFieldDefinitionCriteria,
]

export function countDecimals(number: number) {
    if (Math.floor(number.valueOf()) === number.valueOf()) return 0;
    const split = number.toString().split(".")
    return (split.length > 1 && split[1].length) || 0;
}