// ** Common
import React, { useCallback } from 'react'
import { observer } from 'mobx-react-lite'
import Checkbox from '@material-ui/core/Checkbox'
import { InputProps } from 'shared-do-not-touch/input-props'
import { DealWorkflowTaskDto } from '../../../../../../../clients/deal-system-client-definitions'

interface DealWorkflowTaskDoneCheck {
    task: DealWorkflowTaskDto
    onChange: (task: DealWorkflowTaskDto, newValue: boolean) => Promise<boolean>
    readOnly?: boolean
}

const DealWorkflowActionTaskDoneCheck = (props: DealWorkflowTaskDoneCheck) => {
    const { onChange, task, readOnly } = props
    const doneCheckChanged = useCallback((newValue: boolean) => onChange(task, newValue), [task, onChange])
    return (
        <InputProps stateObject={task} propertyName="done" onValueChange={doneCheckChanged}>
            <Checkbox disabled={readOnly} />
        </InputProps>
    )
}

export default observer(DealWorkflowActionTaskDoneCheck)