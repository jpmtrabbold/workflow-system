import React from 'react'
import { DealCategoryViewStoreContext, DealCategoryViewStore } from './DealCategoryViewStore'
import DealCategoryView from './DealCategoryView'
import { observer } from 'mobx-react-lite'
import ViewLoaderProps from 'shared-do-not-touch/interfaces/ViewLoaderProps'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const DealCategoryViewLoader = (props: ViewLoaderProps) => {
    const store = useStore(() => new DealCategoryViewStore())
    return (
        <DealCategoryViewStoreContext.Provider value={store}>
            <DealCategoryView
                {...props}
            />
        </DealCategoryViewStoreContext.Provider>
    )
}

export default observer(DealCategoryViewLoader)