import { createContext } from "react"
import { DealCategoriesListRequest, DealCategoryListDto, DealCategoriesListResponse } from "../../clients/deal-system-client-definitions"
import { dealCategoryClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { dealCategoryGridDefinition } from './DealCategoryGridDefinition'

export default class DealCategoryGridViewStore {    
    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: dealCategoryGridDefinition,
            listRequestType: DealCategoriesListRequest,
            listMethod: (listRequest) => dealCategoryClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.dealCategories,
        })
    }
    gridStore: GridViewStore<DealCategoryListDto, DealCategoriesListRequest, DealCategoriesListResponse>
}

export const DealCategoryGridViewStoreContext = createContext(new DealCategoryGridViewStore())
