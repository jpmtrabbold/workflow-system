import React from 'react'
import { SalesForecastViewStoreContext, SalesForecastViewStore } from './SalesForecastViewStore'
import SalesForecastView from './SalesForecastView'
import { observer } from 'mobx-react-lite'
import ViewLoaderProps from 'shared-do-not-touch/interfaces/ViewLoaderProps'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const SalesForecastViewLoader = (props: ViewLoaderProps) => {
    const store = useStore(() => new SalesForecastViewStore())
    return (
        <SalesForecastViewStoreContext.Provider value={store}>
            <SalesForecastView
                {...props}
            />
        </SalesForecastViewStoreContext.Provider>
    )
}

export default observer(SalesForecastViewLoader)