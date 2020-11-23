import React, { createContext } from 'react'
import { makeAutoObservable } from 'mobx'
import { DealViewStore } from '../../../../DealViewStore'
import { DealItemExecutionsListViewProps } from './DealItemExecutionsListView'
import { DealItemExecutionItem } from '../execution-item/DealItemExecutionItem'
import AddIcon from "@material-ui/icons/Add"
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
interface DealItemExecutionsListViewStoreProps extends DealItemExecutionsListViewProps {
    rootStore: DealViewStore
}

export class DealItemExecutionsListViewStore {
    constructor(sp?: DealItemExecutionsListViewStoreProps) {
        this.sp = sp
        makeAutoObservable(this)
    }
    sp?: DealItemExecutionsListViewStoreProps
    add = () => {
        this.sp!.rootStore.itemStore.executionStore.addExecution(this.sp!.dealItem)
    }
    get editable() {
        return this.sp!.rootStore.itemStore.executionStore.canChangeExecutions
    }
    get addButton() {
        return (
            <Tooltip title='Add new execution'>
                <IconButton onClick={this.add}><AddIcon /></IconButton>
            </Tooltip>
        )
    }
    get rows() {
        const lastIndex = this.executedItems.length - 1
        return this.executedItems.map((row, index) => (
            <DealItemExecutionItem key={index} executionItem={row} first={index === 0} last={index === lastIndex} />)
        )

    }
    get executedItems() {
        return this.sp!.dealItem.executedItems.filter(et => !et.deleted)
    }
    get hasExecutedItems() {
        return this.executedItems.length > 0
    }

}
export const DealItemExecutionsListViewStoreContext = createContext(new DealItemExecutionsListViewStore())