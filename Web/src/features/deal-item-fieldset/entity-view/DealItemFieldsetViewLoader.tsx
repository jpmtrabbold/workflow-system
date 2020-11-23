import React from 'react'
import { DealItemFieldsetViewStoreContext, DealItemFieldsetViewStore } from './DealItemFieldsetViewStore'
import DealItemFieldsetView from './DealItemFieldsetView'
import { observer } from 'mobx-react-lite'
import ViewLoaderProps from 'shared-do-not-touch/interfaces/ViewLoaderProps'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const DealItemFieldsetViewLoader = (props: ViewLoaderProps) => {
    const store = useStore(() => new DealItemFieldsetViewStore())
    return (
        <DealItemFieldsetViewStoreContext.Provider value={store}>
            <DealItemFieldsetView
                {...props}
            />
        </DealItemFieldsetViewStoreContext.Provider>
    )
}

export default observer(DealItemFieldsetViewLoader)