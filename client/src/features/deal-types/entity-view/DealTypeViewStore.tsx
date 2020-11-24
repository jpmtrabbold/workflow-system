import { LookupRequest, DealTypeDto, PositionEnum } from "../../../clients/deal-system-client-definitions"
import { dealTypeClient, dealClient } from "../../../clients/deal-system-rest-clients"
import { createContext } from "react"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import { snackbar } from "shared-do-not-touch/material-ui-modals"

export class DealTypeViewStore {

    creation = true

    dealType = new DealTypeDto()
    
    loaded = false
    isSaving = false

    dealCategories : LookupRequest[] = []
    
    async saveDealType() {
        let success = true
        this.isSaving = true
        try {
            const result = await dealTypeClient.post(this.dealType)
            snackbar({ title: `Deal Type ${result.dealTypeName} saved successfully` })
        } catch {
            success = false
        }
        this.isSaving = false
        return success
    }

    async fetchData(dealTypeId?: number) {
        if (!dealTypeId)
            return false

        this.creation = false
        if (!await this.fetchDealType(dealTypeId))
            return false

        if (!await this.fetchLookups())
            return false
             
        this.loaded = true

        return true
    }

    async setupNewDealType() {
    
        if (!await this.fetchLookups())
            return false

        this.creation = true
        this.dealType = new DealTypeDto()

        initializeUpdatable(this.dealType.name, '')
        initializeUpdatable(this.dealType.unitOfMeasure, '')
        initializeUpdatable(this.dealType.dealItemFieldsetId, undefined)
        initializeUpdatable(this.dealType.workflowSetId, undefined)
        initializeUpdatable(this.dealType.position, PositionEnum.Buy)
        initializeUpdatable(this.dealType.active, false)
        
        return true
    }

    async fetchDealType(dealTypeId: number): Promise<boolean> {
        try {
            this.dealType = await dealTypeClient.get(dealTypeId)
            return true
        } catch {
            return false
        }
    }
    itemFieldsetLookups = [] as LookupRequest[]
    workflowSetLookups = [] as LookupRequest[]

    async fetchLookups() {
        try {
            this.itemFieldsetLookups = await dealTypeClient.getDealItemFieldsetLookups()
            this.workflowSetLookups = await dealTypeClient.getWorkflowSetLookups()
            this.dealCategories = await dealClient.getDealCategories()
            
            return true
        } catch (error) {
            return false
        }
    }
}

export const DealTypeViewStoreContext = createContext(new DealTypeViewStore())