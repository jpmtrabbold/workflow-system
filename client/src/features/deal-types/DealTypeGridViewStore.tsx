import { createContext } from "react"
import { DealTypesListRequest, DealTypeListDto, DealTypesListResponse } from "../../clients/deal-system-client-definitions"
import { dealTypeClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { dealTypeGridDefinition } from './DealTypeGridDefinition'
export default class DealTypeGridViewStore {

    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: dealTypeGridDefinition,
            listRequestType: DealTypesListRequest,
            listMethod: (listRequest) => dealTypeClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.dealTypes,
        })
    }
    gridStore: GridViewStore<DealTypeListDto, DealTypesListRequest, DealTypesListResponse>
}

export const DealTypeGridViewStoreContext = createContext(new DealTypeGridViewStore())
