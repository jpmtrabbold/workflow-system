import React from 'react'
import { makeAutoObservable } from 'mobx'
import { DealItemExecutionItemProps } from './DealItemExecutionItem'
import { DealViewStore } from 'features/deals/entity-actions/entity-view/DealViewStore'
import { DealItemExecutionsListViewStore } from '../executions-list/DealItemExecutionsListViewStore'
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
import DeleteIcon from "@material-ui/icons/Delete"
import CloudIcon from "@material-ui/icons/Cloud"
import { itemErrorHandler } from '../../deal-items-utils'
import { momentToDateTimeString } from 'features/shared/helpers/utils'
import { DealItemStore } from '../../DealItemStore'
import ModalState from 'features/shared/helpers/ModalState'

interface DealItemExecutionItemStoreProps extends DealItemExecutionItemProps {
    rootStore: DealViewStore
    executionsStore: DealItemExecutionsListViewStore
    classes: Record<'alignCenter', string>
}

export class DealItemExecutionItemStore {

    constructor(sp: DealItemExecutionItemStoreProps) {
        this.sp = sp
        if (sp.executionItem) {
            itemErrorHandler(sp.executionItem, true)
        }
        makeAutoObservable(this)
    }

    sp: DealItemExecutionItemStoreProps

    delete = () => {
        if (!this.sp!.executionItem) {
            throw new Error(`this.sp!.executionItem can't be undefined.`)
        }
        this.sp!.rootStore.itemStore.executionStore.deleteExecution(this.sp.executionsStore.sp!.dealItem, this.sp!.executionItem)
    }

    sourceDataModal = new ModalState()
    get sourceDataButton() {
        const sourceData = this.sp?.executionItem?.sourceData
        if (sourceData) {
            let source = [] as string[]
            if (sourceData.type) {
                source.push(`Type: ${DealItemStore.getIntegrationSourceTypeDescription(sourceData.type)}`)
            }
            if (sourceData.sourceId) {
                source.push(`ID: ${sourceData.sourceId}`)
            }
            if (sourceData.creationDate) {
                source.push(`Creation Date: ${momentToDateTimeString(sourceData.creationDate)}`)
            }
            if (source.length) {
                return (
                    <Tooltip title={`Source Data: ${source.join('; ')}`}>
                        <IconButton onClick={this.sourceDataModal.open}><CloudIcon /></IconButton>
                    </Tooltip>
                )
            }
        }
        return null
    }
    
    get deleteButton() {
        return (
            <Tooltip title='Delete this execution'>
                <IconButton onClick={this.delete}>
                    <DeleteIcon />
                </IconButton>
            </Tooltip>
        )
    }
}