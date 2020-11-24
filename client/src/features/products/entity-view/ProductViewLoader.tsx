import React from 'react'
import { ProductViewStoreContext, ProductViewStore } from './ProductViewStore'
import ProductView from './ProductView'
import { observer } from 'mobx-react-lite'
import ViewLoaderProps from 'shared-do-not-touch/interfaces/ViewLoaderProps'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const ProductViewLoader = (props: ViewLoaderProps) => {
    const store = useStore(() => new ProductViewStore())
    return (
        <ProductViewStoreContext.Provider value={store}>
            <ProductView
                {...props}
            />
        </ProductViewStoreContext.Provider>
    )
}

export default observer(ProductViewLoader)