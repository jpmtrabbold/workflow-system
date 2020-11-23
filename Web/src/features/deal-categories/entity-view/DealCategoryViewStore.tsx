import { LookupRequest, DealCategoryDto } from "../../../clients/deal-system-client-definitions"
import { dealCategoryClient } from "../../../clients/deal-system-rest-clients"
import { createContext } from "react"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable";
import { snackbar } from "shared-do-not-touch/material-ui-modals"

export class DealCategoryViewStore{
    creation = true

    dealCategory = new DealCategoryDto()
    
    loaded = false
    isSaving = false
    
    dealCategories : LookupRequest[] = []
    
    async saveDealCategory() {
        let success = true
        this.isSaving = true
        try {
            const result = await dealCategoryClient.post(this.dealCategory)
            snackbar({ title: `Deal Category ${result.dealCategoryName} saved successfully` })
        } catch {
            success = false
        }
        this.isSaving = false
        return success
    }

    async fetchData(dealCategoryId?: number) {
        if (!dealCategoryId)
            return false

        this.creation = false
        if (!await this.fetchDealCategory(dealCategoryId))
            return false
             
        this.loaded = true

        return true
    }

    async setupNewDealCategory() {
        
        this.creation = true
        this.dealCategory = new DealCategoryDto()
        initializeUpdatable(this.dealCategory.name, '')
        initializeUpdatable(this.dealCategory.unitOfMeasure, '')
        initializeUpdatable(this.dealCategory.active, false)
        
        return true
    }

    async fetchDealCategory(dealCategoryId: number): Promise<boolean> {
        try {
            this.dealCategory = await dealCategoryClient.get(dealCategoryId)
            return true
        } catch {
            return false
        }
    }
}

export const DealCategoryViewStoreContext = createContext(new DealCategoryViewStore())