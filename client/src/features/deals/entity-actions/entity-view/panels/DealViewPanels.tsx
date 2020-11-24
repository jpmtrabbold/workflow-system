import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealViewStoreContext } from '../DealViewStore'
import { DealViewPanel } from './DealViewPanel'

export const DealViewPanels = observer(() => {
    const store = useContext(DealViewStoreContext)
    return (
        <>
            {(store.panels.map((item, index)=> <DealViewPanel key={index} panel={item}/>))}
        </>
    )
})