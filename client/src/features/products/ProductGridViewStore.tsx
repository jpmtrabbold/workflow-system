import { createContext } from "react"
import { ProductListDto, ProductsListRequest, ProductsListResponse } from "../../clients/deal-system-client-definitions"
import { productClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { productGridDefinition } from './ProductGridDefinition'

export default class ProductGridViewStore {
    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: productGridDefinition,
            listRequestType: ProductsListRequest,
            listMethod: (listRequest) => productClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.products,
        })
    }
    gridStore: GridViewStore<ProductListDto, ProductsListRequest, ProductsListResponse>
}

export const ProductGridViewStoreContext = createContext(new ProductGridViewStore())
