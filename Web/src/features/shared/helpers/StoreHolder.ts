
export interface StoreHolderParams {
    
}
export default class StoreHolder<Store extends Object> {
    constructor(params: StoreHolderParams = {}) {
        this.params = params
    }
    params: StoreHolderParams
    store?: Store
    visible = false
    set = (store: Store) => {
        this.store = store
    }
}