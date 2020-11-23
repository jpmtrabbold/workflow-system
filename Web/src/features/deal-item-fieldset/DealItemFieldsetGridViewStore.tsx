import { createContext } from "react"
import { DealItemFieldsetsListRequest, DealItemFieldsetListDto, DealItemFieldsetsListResponse } from "../../clients/deal-system-client-definitions"
import { itemFieldsetClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { itemFieldsetGridDefinition } from './DealItemFieldsetGridDefinition'

export default class DealItemFieldsetGridViewStore {
    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: itemFieldsetGridDefinition,
            listRequestType: DealItemFieldsetsListRequest,
            listMethod: (listRequest) => itemFieldsetClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.itemFieldsets,
        })
    }
    gridStore: GridViewStore<DealItemFieldsetListDto, DealItemFieldsetsListRequest, DealItemFieldsetsListResponse>
}

export const DealItemFieldsetGridViewStoreContext = createContext(new DealItemFieldsetGridViewStore())
