// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealViewStoreContext } from '../../../DealViewStore'
import { DealWorkflowTaskWithChildren } from '../DealWorkflowStore'
import { WorkflowTaskTypeEnum } from '../../../../../../../clients/deal-system-client-definitions'
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import { InputProps } from 'shared-do-not-touch/input-props'
import { KeyboardDateTimePicker } from '@material-ui/pickers/DateTimePicker'
import TextField from '@material-ui/core/TextField'
import ChipInput from 'material-ui-chip-input'
import updatable from 'shared-do-not-touch/utils/updatable'
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'
import { DealWorkflowTradingPolicyAssessmentDialog } from '../action/DealWorkflowTradingPolicyAssessmentDialog'
import Link from '@material-ui/core/Link'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'
interface DealWorkflowTaskFieldProps {
    taskWithChildren: DealWorkflowTaskWithChildren
    readOnly?: boolean
}
const fullWidth = true

const DealWorkflowTaskField = (props: DealWorkflowTaskFieldProps) => {
    const rootStore = useContext(DealViewStoreContext)
    const workflowStore = rootStore.workflowStore

    const store = useStore(sp => ({
        get lookups() {
            if ([WorkflowTaskTypeEnum.AnswerToQuestion, WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels].find(t => t === sp.taskWithChildren.task.type)) {
                const possibleAnswers = sp.taskWithChildren.task.possibleAnswers
                    .map(p => ({ id: p.id, name: p.description + (!!p.alternateWorkflowActionId ? ' (this option will change the current action)' : '') }))

                return possibleAnswers
            }
            return undefined
        },
        get customCheckDescription() {
            switch (sp.taskWithChildren.task.type) {
                case WorkflowTaskTypeEnum.ExpiryDateCheck:
                    return this.generateTaskDescription(workflowStore.expiryDateCheckDescription())
                case WorkflowTaskTypeEnum.DealExecutedCheck:
                    return this.generateTaskDescription(workflowStore.executionCheckDescription())
                case WorkflowTaskTypeEnum.DealNotExecutedCheck:
                    return this.generateTaskDescription(workflowStore.executionCheckDescription(true))
            }
            return undefined
        },
        get taskDescription() {
            return this.generateTaskDescription(sp.taskWithChildren.task.workflowTaskDescription.value)
        },
        generateTaskDescription: (description: string) => {
            if (sp.taskWithChildren.task.mandatory) {
                return description
            } else {
                return "(Optional) " + description
            }
        },
        async onValueChange(newValue: any) {
            return await workflowStore.taskValueChanged(props.taskWithChildren, newValue)
        },
        nullChange() {

        },
        multipleOnChange(chips: string[]) {
            const info = sp.taskWithChildren.task.textInformation
            updatable(info, chips.join('; '))
            store.onValueChange(info.value)
        },
        get multipleDefault() {
            const value = sp.taskWithChildren.task.textInformation.value
            return !!value ? value.split("; ") : []
        },
        get cannotEditAnswer() {
            return sp.taskWithChildren.task.type === WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels
        }
    }), props)

    const { task } = props.taskWithChildren
    const { type } = task

    if (store.customCheckDescription) {
        return <>{store.customCheckDescription}</>
    }

    switch (type) {
        case WorkflowTaskTypeEnum.EnterDateInformation:
            return (
                <InputProps stateObject={task} propertyName="dateInformation" onValueChange={store.onValueChange} config={{ elementValueForUndefinedOrNull: () => null }}>
                    <KeyboardDatePicker
                        autoOk
                        format="DD/MM/YYYY"
                        disabled={props.readOnly}
                        label={store.taskDescription}
                        fullWidth={fullWidth}
                        value={null}
                        onChange={store.nullChange}
                    />
                </InputProps>
            )
        case WorkflowTaskTypeEnum.EnterDateTimeInformation:
            return (
                <InputProps stateObject={task} propertyName="dateInformation" onValueChange={store.onValueChange} config={{ elementValueForUndefinedOrNull: () => null }}>
                    <KeyboardDateTimePicker
                        autoOk
                        format="DD/MM/YYYY HH:mm"
                        disabled={props.readOnly}
                        label={store.taskDescription}
                        fullWidth={fullWidth}
                        value={null}
                        onChange={store.nullChange}
                    />
                </InputProps>
            )
        case WorkflowTaskTypeEnum.EnterMonthAndYearInformation:
            return (
                <InputProps stateObject={task} propertyName="dateInformation" onValueChange={store.onValueChange} config={{ elementValueForUndefinedOrNull: () => null }}>
                    <KeyboardDatePicker
                        autoOk
                        disabled={props.readOnly}
                        views={['month', 'year']}
                        label={store.taskDescription}
                        fullWidth={fullWidth}
                        value={null}
                        onChange={store.nullChange}
                    />
                </InputProps>
            )
        case WorkflowTaskTypeEnum.EnterNumberInformation:
            return (
                <InputProps stateObject={task} propertyName="numberInformation" onValueChange={store.onValueChange} variant='onlyNumbers'>
                    <TextField disabled={props.readOnly} label={store.taskDescription} fullWidth={fullWidth} />
                </InputProps>
            )
        case WorkflowTaskTypeEnum.EnterTextInformation:
            return (
                <InputProps stateObject={task} propertyName="textInformation" onValueChange={store.onValueChange} >
                    <TextField multiline disabled={props.readOnly} label={store.taskDescription} fullWidth={fullWidth} />
                </InputProps>
            )
        case WorkflowTaskTypeEnum.SimpleCheck:
        case WorkflowTaskTypeEnum.HasItems:
        case WorkflowTaskTypeEnum.CreatedNoteDuringStatus:
        case WorkflowTaskTypeEnum.AttachedDocumentDuringStatus:
            return <>{store.taskDescription}</>
        case WorkflowTaskTypeEnum.AnswerToQuestion:
        case WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels:

            if (props.readOnly) {
                return (
                    <TextField
                        disabled={true}
                        value={task.workflowTaskAnswerText.value || ""}
                        label={store.taskDescription}
                        fullWidth={fullWidth}
                    />
                )
            }
            return (
                <>
                    <InputProps stateObject={task} propertyName="workflowTaskAnswerId" onValueChange={store.onValueChange} >
                        <SelectWithLabel disabled={props.readOnly || store.cannotEditAnswer} label={store.taskDescription} lookups={store.lookups} fullWidth={fullWidth} />
                    </InputProps>
                    
                    {type === WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels && (<>
                        <Link onClick={workflowStore.openTradingPolicyAssessmentDialog} style={{ cursor: 'pointer' }}>
                            Why?
                        </Link>
                        <DealWorkflowTradingPolicyAssessmentDialog />
                    </>)}
                </>
            )
        case WorkflowTaskTypeEnum.EnterMultipleInformation:
            return (
                <ChipInput
                    disabled={props.readOnly}
                    label={store.taskDescription + (props.readOnly ? "" : " (Press ENTER after typing each one)")}
                    defaultValue={store.multipleDefault}
                    onChange={store.multipleOnChange}
                    fullWidth={fullWidth}
                />
            )

    }
    return null
}

export default observer(DealWorkflowTaskField)