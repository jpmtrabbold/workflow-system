import { DealItemFieldsetDto, StringLookupRequest, DealItemFieldDto, UpdatableOfString, UpdatableOfBoolean } from "../../../clients/deal-system-client-definitions"
import { itemFieldsetClient } from "../../../clients/deal-system-rest-clients"
import { createContext } from "react"
import updatable, { anyPropertyUpdated, initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import { snackbar } from "shared-do-not-touch/material-ui-modals"

export class SelectableDealItemField {
    selected = false
    name = new UpdatableOfString({updated: false, value: ""})
    execution = new UpdatableOfBoolean({updated: false, value: false})
    field = ""
    id?= 0
}

export class DealItemFieldsetViewStore {
    creation = true

    itemFieldset = new DealItemFieldsetDto()

    loaded = false
    isSaving = false

    itemFields = [] as SelectableDealItemField[]
    fieldLookups = [] as StringLookupRequest[]

    async saveDealItemFieldset() {
        let success = true
        this.isSaving = true
        try {
            this.preSave()
            const result = await itemFieldsetClient.post(this.itemFieldset)
            snackbar({ title: `Deal Item Fieldset ${result.dealItemFieldsetName} saved successfully` })
        } catch {
            success = false
        }
        this.isSaving = false
        return success
    }

    async fetchData(itemFieldsetId?: number) {
        if (!itemFieldsetId)
            return false

        this.creation = false
        if (!await this.fetchDealItemFieldset(itemFieldsetId))
            return false

        if (!await this.fetchLookups())
            return false

        this.postFetch()

        this.loaded = true

        return true
    }

    async setupNewDealItemFieldset() {

        this.creation = true
        this.itemFieldset = new DealItemFieldsetDto()
        initializeUpdatable(this.itemFieldset.description, '')

        if (!await this.fetchLookups())
            return false

        this.postFetch()

        return true
    }

    async fetchDealItemFieldset(itemFieldsetId: number): Promise<boolean> {
        try {
            this.itemFieldset = await itemFieldsetClient.get(itemFieldsetId)
            return true
        } catch {
            return false
        }
    }

    async fetchLookups() {
        try {
            this.fieldLookups = await itemFieldsetClient.getItemFieldLookups()
            return true
        } catch {
            return false
        }
    }

    postFetch() {
        const currentFields = this.itemFieldset.fields.map(field => {
            const newField = new SelectableDealItemField()
            newField.name = field.name
            newField.execution = field.execution
            newField.field = field.field.value
            newField.selected = true
            newField.id = field.id
            return newField
        })

        const newFields = this.fieldLookups.filter(field => !currentFields.find(tf => tf.field === field.id)).map(field => {
            const newField = new SelectableDealItemField()
            initializeUpdatable(newField.name, field.name)
            initializeUpdatable(newField.execution, false)
            newField.field = field.id
            newField.selected = false
            return newField
        })

        this.itemFields = [...currentFields, ...newFields]
    }

    preSave() {
        let count = 0
        this.itemFieldset.fields = this.itemFields
            .filter(field => field.selected || !!this.itemFieldset.fields.find(tfs => tfs.id === field.id))
            .map(field => {
                let saveField = this.itemFieldset.fields.find(tfs => tfs.id === field.id)

                if (!field.selected) {
                    if (!!saveField) {
                        saveField.deleted = true
                    }
                } else {
                    if (!saveField) {
                        saveField = new DealItemFieldDto()
                        updatable(saveField.field, field.field)
                    }
                    count += 10
                    saveField.name = field.name
                    saveField.execution = field.execution
                    updatable(saveField.displayOrder, count)
                }
                
                saveField!.updated = anyPropertyUpdated(saveField)

                return saveField!
            })
    }


    MoveFieldDown(fieldIndex: number): void {
        if (fieldIndex !== (this.itemFields.length - 1)) {
            const thisRow = this.itemFields[fieldIndex]
            this.itemFields.splice(fieldIndex, 1)
            this.itemFields.splice(fieldIndex+1, 0, thisRow)
        }
    }

    MoveFieldUp(fieldIndex: number): void {
        if (fieldIndex !== 0) {
            const thisRow = this.itemFields[fieldIndex]
            this.itemFields.splice(fieldIndex, 1)
            this.itemFields.splice(fieldIndex-1, 0, thisRow)
        }
    }

}

export const DealItemFieldsetViewStoreContext = createContext(new DealItemFieldsetViewStore())