import React from 'react'
import DealGridView from '../DealGridView'
import { observer } from 'mobx-react-lite'

const DealGridAssignedView = () => {
    return <DealGridView assignedToMe />
}

export default observer(DealGridAssignedView)