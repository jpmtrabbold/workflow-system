import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealItemFieldDefinition } from '../../DealItemFieldDefinition'
import { DealItemDto } from 'clients/deal-system-client-definitions'
import { DealViewStoreContext } from 'features/deals/entity-actions/entity-view/DealViewStore'
import { itemErrorHandler } from '../../deal-items-utils'
import { DealItemExecutionsListViewStoreContext } from '../executions-list/DealItemExecutionsListViewStore'
import { action } from 'mobx'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface DealItemExecutionItemFieldProps {
    executionField: DealItemFieldDefinition
    executionItem: DealItemDto
}

export const DealItemExecutionItemField = observer((props: DealItemExecutionItemFieldProps) => {
    const itemStore = useContext(DealViewStoreContext).itemStore
    const executionStore = itemStore.executionStore
    const executionsStore = useContext(DealItemExecutionsListViewStoreContext)

    const store = useStore(sp => ({
        onValueChanged() {
            if (!!sp.executionField.executionValidation) {
                const errorHandler = store.errorHandler
                
                const message = sp.executionField.executionValidation(executionsStore.sp!.dealItem, sp.executionItem, executionStore)
                if (message) {
                    setTimeout(action(() => {
                        errorHandler.resetFieldError(sp.executionField.field)
                        errorHandler.error(sp.executionField.field, message)   
                    }));
                } else {
                    setTimeout(action(() => {
                        errorHandler.resetFieldError(sp.executionField.field)
                    }))
                }
            }
            sp.executionItem.updated = true
            executionsStore.sp!.dealItem.updated = true
        },
        get errorHandler() {
            return itemErrorHandler(sp.executionItem, true)!
        },
        get updatable() {
            return sp.executionField.updatable(sp.executionItem)
        },
    }), props)

    const ef = props.executionField
    if (executionsStore.editable) {
        const Comp = ef.componentRenderer
        return (
            <Comp
                dealItem={props.executionItem}
                fieldLabel={ef.inputLabel || ""}
                store={itemStore}
                fieldName={ef.field}
                execution={true}
                onValueChanged={store.onValueChanged}
                errorHandler={store.errorHandler}
                updatable={store.updatable}
            />
        )
    } else {
        let comp = null
        if (!!ef.render) {
            comp = ef.render(props.executionItem)
        } else {
            comp = props.executionItem[ef.field]
        }
        return <>{ef.title + ": " + comp}</>
    }
})