import React from 'react'
import { DealTypeViewStoreContext, DealTypeViewStore } from './DealTypeViewStore'
import DealTypeView from './DealTypeView'
import { observer } from 'mobx-react-lite'
import ViewLoaderProps from 'shared-do-not-touch/interfaces/ViewLoaderProps'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const DealTypeViewLoader = (props: ViewLoaderProps) => {
    const store = useStore(() => new DealTypeViewStore())
    return (
        <DealTypeViewStoreContext.Provider value={store}>
            <DealTypeView
                {...props}
            />
        </DealTypeViewStoreContext.Provider>
    )
}

export default observer(DealTypeViewLoader)