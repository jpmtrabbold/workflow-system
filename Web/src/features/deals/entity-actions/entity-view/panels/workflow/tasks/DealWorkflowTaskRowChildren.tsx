// ** Common
import React from 'react'
import { observer } from 'mobx-react-lite'
import { DealWorkflowTaskWithChildren } from '../DealWorkflowStore'
import DealWorkflowTaskRow from './DealWorkflowTaskRow'

interface DealWorkflowTaskRowChildrenProps {
    taskWithChildren: DealWorkflowTaskWithChildren
    level?: number
    readOnly?: boolean
}

const DealWorkflowTaskRowChildren = (props: DealWorkflowTaskRowChildrenProps) => {
    const { taskWithChildren, readOnly } = props
    const level = !!props.level ? props.level : 0
    
    return (
        <>
            {taskWithChildren.children.map(item => <DealWorkflowTaskRow readOnly={readOnly} level={level + 1} key={item.task.id} taskWithChildren={item} />)}
        </>
    )
}

export default observer(DealWorkflowTaskRowChildren)