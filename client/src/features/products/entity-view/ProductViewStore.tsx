import { LookupRequest, ProductDto } from "../../../clients/deal-system-client-definitions"
import { productClient, dealClient } from "../../../clients/deal-system-rest-clients"
import { createContext } from "react"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import { snackbar } from "shared-do-not-touch/material-ui-modals"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"

export class ProductViewStore {
    creation = true

    product = new ProductDto()

    loaded = false
    isSaving = false

    dealCategories: LookupRequest[] = []
    
    async saveProduct() {
        let success = true
        try {
            const result = await executeLoading("Saving Product...", () => productClient.post(this.product))    
            snackbar({ title: `Product ${result.productName} saved successfully` })
        } catch {
            success = false
        }
        return success
    }

    fetchDealCategories = async () => {
        try {
            this.dealCategories = await dealClient.getDealCategories()
        } catch (error) {
            return false
        }
        return true
    }

    async fetchData(productId?: number) {
        if (!productId)
            return false

        this.creation = false
        if (!await this.fetchProduct(productId))
            return false

        if (!await this.fetchDealCategories())
            return false

        this.loaded = true

        return true
    }

    async setupNewProduct() {

        this.creation = true
        this.product = new ProductDto()
        initializeUpdatable(this.product.name, '')
        initializeUpdatable(this.product.dealCategoryId, 1)
        initializeUpdatable(this.product.active, true)

        if (!await this.fetchDealCategories())
            return false

        return true
    }

    async fetchProduct(productId: number): Promise<boolean> {
        try {
            this.product = await productClient.get(productId)
            return true
        } catch {
            return false
        }
    }
}

export const ProductViewStoreContext = createContext(new ProductViewStore())