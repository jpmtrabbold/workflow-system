import { createContext } from "react"
import { SalesForecastsListRequest, SalesForecastListDto, SalesForecastsListResponse } from "../../clients/deal-system-client-definitions"
import { salesForecastClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { salesForecastGridDefinition } from './SalesForecastGridDefinition'

export default class SalesForecastGridViewStore {
    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: salesForecastGridDefinition,
            listRequestType: SalesForecastsListRequest,
            listMethod: (listRequest) => salesForecastClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.salesForecasts,
        })
    }
    gridStore: GridViewStore<SalesForecastListDto, SalesForecastsListRequest, SalesForecastsListResponse>
    canAddForecast = false
}

export const SalesForecastGridViewStoreContext = createContext(new SalesForecastGridViewStore())
