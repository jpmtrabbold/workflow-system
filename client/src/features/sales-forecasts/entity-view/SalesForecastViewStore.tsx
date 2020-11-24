import { LookupRequest, SalesForecastDto } from "../../../clients/deal-system-client-definitions"
import { salesForecastClient } from "../../../clients/deal-system-rest-clients"
import { createContext } from "react"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable";
import { snackbar } from "shared-do-not-touch/material-ui-modals"

export class SalesForecastViewStore {
    creation = true

    salesForecast = new SalesForecastDto()
    
    loaded = false
    isSaving = false
    
    salesForecasts : LookupRequest[] = []
    
    nullChange = () => null

    async saveSalesForecast() {
        let success = true
        this.isSaving = true
        try {
            const result = await salesForecastClient.post(this.salesForecast)
            snackbar({ title: `Sales Forecast for ${result.salesForecastName} saved successfully` })
        } catch {
            success = false
        }
        this.isSaving = false
        return success
    }

    async fetchData(salesForecastId?: number) {
        if (!salesForecastId)
            return false

        this.creation = false
        if (!await this.fetchSalesForecast(salesForecastId))
            return false
             
        this.loaded = true

        return true
    }

    async setupNewSalesForecast() {
        
        this.creation = true
        this.salesForecast = new SalesForecastDto()
        initializeUpdatable(this.salesForecast.volume, 0)
        initializeUpdatable(this.salesForecast.monthYear, null)        
        return true
    }

    async fetchSalesForecast(salesForecastId: number): Promise<boolean> {
        try {
            this.salesForecast = await salesForecastClient.get(salesForecastId)
            return true
        } catch {
            return false
        }
    }
}

export const SalesForecastViewStoreContext = createContext(new SalesForecastViewStore())