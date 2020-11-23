import React, { createRef } from 'react'
import { DealViewStore } from "../../DealViewStore";
import updatable from "shared-do-not-touch/utils/updatable";
import { WorkflowActionReadDto, DealWorkflowStatusDto, WorkflowAssignmentTypeEnum, DealWorkflowTaskDto, WorkflowTaskTypeEnum, TraderAuthorityPolicyAssessment, WorkflowTaskAnswerReadDto, DealWorkflowAssignmentDto, WorkflowTaskAnswerTypeEnum } from "../../../../../../clients/deal-system-client-definitions";
import { makeAutoObservable } from "mobx";
import { messageConfirm, messageError, messageYesNo } from "shared-do-not-touch/material-ui-modals";
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading";
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable";
import { MessageDialogAction } from "shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog";
import { htmlSpace } from "shared-do-not-touch/utils/utils";
import Link from "@material-ui/core/Link";
import Tooltip from '@material-ui/core/Tooltip'
import Typography from '@material-ui/core/Typography'
import { TraderAuthorityPolicyAssessmentRow } from 'clients/deal-system-client-definitions'
import { observer } from 'mobx-react-lite';
import FormErrorHandler from 'shared-do-not-touch/input-props/form-error-handler';
import { selectAllTextOnInput, momentToDateTimeString, isValidMoment, momentToDateString } from 'features/shared/helpers/utils';
import { dealClient } from 'clients/deal-system-rest-clients';

export interface WorkflowStatusAssignmentOption {
    checked: boolean
    id: number
    name: string
    type: WorkflowAssignmentTypeEnum
    enabledForSelection?: boolean
    meetsTradingPolicy?: boolean
    isTraderWorkflowLevel?: boolean
    assessments?: TraderAuthorityPolicyAssessment[]
}

export class DealWorkflowTaskWithChildren {
    constructor(task: DealWorkflowTaskDto) {
        this.task = task
        makeAutoObservable(this)
    }
    task: DealWorkflowTaskDto
    children = [] as DealWorkflowTaskWithChildren[]
}

export class DealWorkflowStore {
    constructor(rootStore: DealViewStore) {
        this.rootStore = rootStore
        makeAutoObservable(this)
    }

    errorHandler = new FormErrorHandler<DealWorkflowStore>()

    actionDialogVisible = false
    assignmentDialogVisible = false
    tasks = [] as DealWorkflowTaskWithChildren[]

    alternativeActionId?: number
    alternativeAction?: WorkflowActionReadDto

    action?: WorkflowActionReadDto

    rootStore: DealViewStore

    statusColumns: Column<DealWorkflowStatusDto>[] = [
        { title: "Status", field: 'workflowStatusName' },
        { title: "Initiated By", field: 'initiatedByUserName' },
        {
            title: "On",
            render: (status) => momentToDateTimeString(status.dateTimeConfirmed),
            rawDataForSearchingAndSorting: (status) => status.dateTimeConfirmed,
            defaultSort: 'desc',
        },
        { title: "Assigned To", render: (status) => this.assignedToDescription(status) },
    ]

    setupNewDeal = () => {
        this.rootStore.deal.assignedToSelf = true
    }

    workflowIsSetUp = false
    setupExistingDealPromise?: Promise<void>
    setupExistingDeal = async () => {
        this.workflowIsSetUp = false
        this.setupExistingDealPromise = this.setupExistingDealAux()
    }

    setupExistingDealAux = async () => {
        this.action = await this.getAction(this.rootStore.deal.ongoingWorkflowActionId.value)
        this.configureAssignments()
        this.workflowIsSetUp = true
    }

    gotDealTypeConfiguration = (fromDealTypeChanging: boolean | undefined) => {
        if (fromDealTypeChanging || !this.rootStore.deal.workflowSetId.value) {
            updatable(this.rootStore.deal.workflowSetId, this.rootStore.dealTypeConfiguration!!.workflowSetId);
        }
    }

    getRoleAssignments = (possibleAssignments: DealWorkflowAssignmentDto[]): WorkflowStatusAssignmentOption[] => {
        return possibleAssignments.map(pa => {
            let assignmentAlreadyMade = !!this.nextStatus.assigneeWorkflowRoleId.value

            if (!assignmentAlreadyMade && pa.meetsTradingPolicy) {
                this.nextStatus.updated = true
                updatable(this.nextStatus.assigneeWorkflowRoleId, pa.workflowRoleId)
            }

            return {
                checked: (pa.workflowRoleId === this.nextStatus.assigneeWorkflowRoleId.value),
                id: pa.workflowRoleId,
                name: pa.workflowRoleName,
                type: this.nextStatus.assignmentType,
                enabledForSelection: pa.enabledForSelection,
                meetsTradingPolicy: pa.meetsTradingPolicy,
                isTraderWorkflowLevel: pa.isTraderWorkflowLevel,
                assessments: pa.assessments,
            }
        })
    }
    configureAssignments = () => {
        if (this.nextStatus.assignmentType === WorkflowAssignmentTypeEnum.ApprovalLevelSelectionEqualHigher) {
            this.assignments = this.getRoleAssignments(this.rootStore.deal.possibleAssignments)
            this.assignmentTableTitle = "Role Assignment"
        } else if (!!this.nextStatus.assigneeUserId && !!this.nextStatus.assigneeUserName) {
            this.assignments = [{
                checked: true,
                id: this.nextStatus.assigneeUserId,
                name: this.nextStatus.assigneeUserName,
                type: this.nextStatus.assignmentType,
                enabledForSelection: true,
                meetsTradingPolicy: true,
            }]
            this.assignmentTableTitle = "User Assignment"
        } else if (!!this.nextStatus.assigneeWorkflowRoleId.value && !!this.nextStatus.assigneeWorkflowRoleName) {
            this.assignments = [{
                checked: true,
                id: this.nextStatus.assigneeWorkflowRoleId.value,
                name: this.nextStatus.assigneeWorkflowRoleName,
                type: this.nextStatus.assignmentType,
                enabledForSelection: true,
                meetsTradingPolicy: true,
            }]
            this.assignmentTableTitle = "Role Assignment"
        }
    }

    getAction = async (ongoingActionId?: number) => {
        const config = await this.rootStore.fetchDealTypeConfigurationPromise
        if (!config) {
            return undefined
        }
        const { actions } = config.currentWorkflowStatusConfig
        const action = actions.find(a => a.id === ongoingActionId)
        if (action) {
            return action
        } else {
            return undefined
        }
    }

    get nextStatus() {
        const { deal } = this.rootStore
        const status = deal.dealWorkflowStatuses.find(s => s.id === deal.nextDealWorkflowStatusId)
        if (status) {
            return status
        } else {
            return new DealWorkflowStatusDto()
        }
    }

    carryOutAction = async (workflowAction: WorkflowActionReadDto, alternativeAction = false) => {
        const ongoingActionId = this.rootStore.deal.ongoingWorkflowActionId.value
        const newActionId = workflowAction.id

        if (!!ongoingActionId && ongoingActionId !== newActionId) {
            if (this.action) {
                if (alternativeAction) {
                    if (!await messageConfirm({
                        content: `Because of your answers, the '${workflowAction.name}' action will be initiated. ` +
                            `all checks of the '${this.action.name}' action will be discarded. ` +
                            `Would you like to proceed?`,
                        title: 'Change of Action'
                    })) {

                        return
                    }
                } else {
                    if (!await messageConfirm({
                        content: `The '${this.action.name}' action is currently being taken on this deal. ` +
                            `By proceeding with the '${workflowAction.name}' action, all checks of '${this.action.name}' will be discarded`,
                        title: 'Change of Action'
                    })) {

                        return
                    }
                }
            }
        }

        // if the user is taking this action for the first time (as in 'not just resuming the action'), set the ongoing action
        if (!ongoingActionId || ongoingActionId !== newActionId) {
            const backup = this.rootStore.deal.ongoingWorkflowActionId.clone()
            updatable(this.rootStore.deal.ongoingWorkflowActionId, newActionId)
            if (alternativeAction) {
                this.rootStore.deal.ongoingWorkflowActionNote = this.getAlternativeActionNotes(this.tasks)
            }
            if (!await this.rootStore.saveDeal(false)) {
                this.rootStore.deal.ongoingWorkflowActionId = backup
                return
            }
            if (!await executeLoading("Fetching deal...", () => this.rootStore.fetchData(this.rootStore.deal.id))) {
                return
            }
        }
        this.showActionView()
    }

    assignments = [] as WorkflowStatusAssignmentOption[]
    assignmentTableTitle = ""

    getAlternativeActionNotes(tasksWithChildren: DealWorkflowTaskWithChildren[], parent?: DealWorkflowTaskWithChildren) {
        let note = ''
        let parentCausedAlternativeAction = false
        if (!!parent) {
            for (const possibleAnswer of parent.task.possibleAnswers) {
                if (!!possibleAnswer.alternateWorkflowActionId && parent.task.workflowTaskAnswerId.value === possibleAnswer.id) {
                    parentCausedAlternativeAction = true
                    break
                }
            }
        }
        for (const taskWithChildren of tasksWithChildren) {
            if (parentCausedAlternativeAction && taskWithChildren.task.type === WorkflowTaskTypeEnum.EnterTextInformation) {
                note = note + (note ? '\n' : '') + taskWithChildren.task.textInformation.value
            }
            const newNote = this.getAlternativeActionNotes(taskWithChildren.children, taskWithChildren)
            if (newNote) {
                note = note + (note ? '\n' : '') + newNote
            }
        }

        return note
    }

    showActionView = async () => {
        await this.updateTaskTree(this.nextStatus, true)
        if (this.alternativeActionId) {
            this.alternativeAction = await this.getAction(this.alternativeActionId)
        } else {
            this.alternativeAction = undefined
        }
        this.actionDialogVisible = true
    }

    checkTasksDone(tasksWithChildren: DealWorkflowTaskWithChildren[], checkingForOptionalOnly = false) {
        for (const task of tasksWithChildren) {
            if (!task.task.done.value) {
                if (checkingForOptionalOnly) {
                    if (!task.task.mandatory) {
                        return false
                    }
                } else {
                    if (task.task.mandatory) {
                        return false
                    }
                }
            }
            if (!this.checkTasksDone(task.children)) {
                return false
            }
        }

        return true
    }
    confirmOngoingAction = async () => {
        if (!!this.alternativeActionId) {
            this.carryOutAction(this.alternativeAction!, true)
            return
        }

        if (!this.checkTasksDone(this.tasks)) {
            messageError({
                content: "Please complete all the mandatory tasks before proceeding.",
                title: 'Mandatory Tasks'
            })
            return
        }

        if (!this.checkTasksDone(this.tasks, true)) {
            if (!await messageYesNo({
                content: "There are optional tasks that weren't completed. Would you like to proceed anyway?",
                title: 'Optional Tasks'
            })) {
                return
            }
        }

        this.rootStore.deal.completedActionId = this.rootStore.deal.ongoingWorkflowActionId.value
        if (!await this.rootStore.saveDeal(true)) {
            return
        }
        this.rootStore.sp.onEntityClose && this.rootStore.sp.onEntityClose()
    }

    get currentActions() {
        if (this.rootStore.sp.execution || !this.workflowIsSetUp) {
            return []
        }

        const { dealTypeConfiguration } = this.rootStore
        const actions = []
        if (this.rootStore.deal.assignedToSelf && dealTypeConfiguration) {
            const currentWorkflowStatus = dealTypeConfiguration.currentWorkflowStatusConfig
            if (currentWorkflowStatus) {
                for (const workflowAction of currentWorkflowStatus.actions) {
                    actions.push(workflowAction)
                }
            }
        }
        return actions
    }

    handleCloseAction = () => {
        this.actionDialogVisible = false
    }

    handleCloseAssignment = () => {
        this.assignmentDialogVisible = false
    }

    get actions() {
        let actions = [
            {
                name: "Close",
                title: "Just closes this screen and goes back to deal editing. It will preserve the tasks and assignments done so they can be saved when you click the 'Save' button.",
                callback: this.handleCloseAction,
                color: 'secondary'
            },
            {
                name: (!!this.alternativeActionId ? 'Confirm Action Change' : 'Confirm Action'),
                title: (!!this.alternativeActionId
                    ? `Confirms the change to the '${this.alternativeAction?.name}' action.`
                    : `Confirms the '${this.action?.name}' action, proceeding to the '${this.nextStatus.workflowStatusName}' status.`),
                callback: this.confirmOngoingAction,
                color: 'primary'
            }
        ] as MessageDialogAction[]

        return actions
    }

    handleAssignmentCheck = (assignmentOption: WorkflowStatusAssignmentOption, checked: boolean) => {
        const option = this.assignments.find(o => o.id === assignmentOption.id)
        if (!option)
            throw new Error("Internal error: Expression 'const option = this.assignments.find(o => o.id == assignmentOption.id)' didn't return a option")

        if (!option.checked && checked) {
            const otherOption = this.assignments.find(o => o.checked)
            if (otherOption) {
                otherOption.checked = false
            }
            option.checked = true
            this.nextStatus.updated = true
            updatable(this.nextStatus.assigneeWorkflowRoleId, assignmentOption.id)
        }
    }

    checkChanged = async (task: DealWorkflowTaskDto, newValue: boolean) => {
        switch (task.type) {
            case WorkflowTaskTypeEnum.HasItems:
            case WorkflowTaskTypeEnum.CreatedNoteDuringStatus:
            case WorkflowTaskTypeEnum.AttachedDocumentDuringStatus:
            case WorkflowTaskTypeEnum.ExpiryDateCheck:
            case WorkflowTaskTypeEnum.DealExecutedCheck:
            case WorkflowTaskTypeEnum.DealNotExecutedCheck:
                return false
        }
        if (newValue) {
            if (!await this.taskHasValue(task)) {
                return false
            }
        }
        this.nextStatus.updated = true
        task.updated = true
        return true
    }
    resetTaskChildrenValues = (taskWithChildren: DealWorkflowTaskWithChildren) => {
        for (const child of taskWithChildren.children) {
            const { task } = child
            updatable(task.numberInformation, undefined)
            updatable(task.dateInformation, undefined)
            updatable(task.textInformation, undefined)
            updatable(task.workflowTaskAnswerId, undefined)
            updatable(task.workflowTaskAnswerText, "")
            updatable(task.done, false)
            task.updated = true
            this.resetTaskChildrenValues(child)
        }
    }

    taskHasValue = async (task: DealWorkflowTaskDto, forcedValue?: any, forced?: boolean) => {
        switch (task.type) {
            case WorkflowTaskTypeEnum.EnterDateInformation:
            case WorkflowTaskTypeEnum.EnterDateTimeInformation:
            case WorkflowTaskTypeEnum.EnterMonthAndYearInformation:
                return isValidMoment((forced ? forcedValue : task.dateInformation.value))
            case WorkflowTaskTypeEnum.EnterMultipleInformation:
            case WorkflowTaskTypeEnum.EnterTextInformation:
                const val = (forced ? forcedValue : task.textInformation.value)
                return !!val && val !== ""

            case WorkflowTaskTypeEnum.HasItems:
                const { filteredData: dealItems } = await this.rootStore.fetchItemsPromise
                return dealItems.length > 0
            case WorkflowTaskTypeEnum.CreatedNoteDuringStatus:
                const { filteredData: notes } = await this.rootStore.fetchNotesPromise
                return notes.filter(d => !d.isLocked).length > 0 // notes that were added during this status
            case WorkflowTaskTypeEnum.AttachedDocumentDuringStatus:
                const { filteredData: attachments } = await this.rootStore.fetchAttachmentsPromise
                return attachments.filter(d => d.lastVersion && !d.lastVersion.isLocked).length > 0 // attachments that were added during this status
            case WorkflowTaskTypeEnum.EnterNumberInformation:
                const numVal = (forced ? forcedValue : task.numberInformation.value)
                return numVal !== undefined && !isNaN(numVal)
            case WorkflowTaskTypeEnum.AnswerToQuestion:
            case WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels:
                const answerVal = (forced ? forcedValue : task.workflowTaskAnswerId.value)
                return !!answerVal
            case WorkflowTaskTypeEnum.ExpiryDateCheck:
                return !!this.rootStore.deal.expiryDate.value && isValidMoment(this.rootStore.deal.expiryDate.value)
            case WorkflowTaskTypeEnum.DealExecutedCheck:
                return !!this.rootStore.deal.executed
            case WorkflowTaskTypeEnum.DealNotExecutedCheck:
                return !this.rootStore.deal.executed
        }
        return true
    }

    hasAttachmentType = async (attachmentTypeToVerifyId: number) => {
        const { filteredData } = await this.rootStore.fetchAttachmentsPromise
        return filteredData.some(a => a.dealAttachment.attachmentTypeId.value === attachmentTypeToVerifyId)
    }

    setTaskAnswer = (task: DealWorkflowTaskDto, answer: WorkflowTaskAnswerReadDto) => {
        updatable(task.workflowTaskAnswerId, answer.id)
        updatable(task.workflowTaskAnswerText, answer.description)
        updatable(task.done, true)
        task.updated = true
        this.nextStatus.updated = true
    }

    private async ProcessTaskAnswerToQuestion(taskWithChildren: DealWorkflowTaskWithChildren) {
        for (const possibleAnswer of taskWithChildren.task.possibleAnswers) {
            if (!!possibleAnswer.attachmentTypeToVerifyId) {
                if (await this.hasAttachmentType(possibleAnswer.attachmentTypeToVerifyId!)) {
                    this.setTaskAnswer(taskWithChildren.task, possibleAnswer);
                }
            }
            if (!!possibleAnswer.alternateWorkflowActionId && taskWithChildren.task.workflowTaskAnswerId.value === possibleAnswer.id) {
                this.alternativeActionId = possibleAnswer.alternateWorkflowActionId;
            }
        }
    }

    tradingPolicyAssessmentDialogVisible = false
    handleCloseTradingPolicyAssessmentDialog = () => {
        this.tradingPolicyAssessmentDialogVisible = false
    }
    openTradingPolicyAssessmentDialog = () => {
        this.tradingPolicyAssessmentDialogVisible = true
    }

    tradingPolicyAssessmentLevels?: WorkflowStatusAssignmentOption[]
    tradingPolicyAssessmentLevelsUserId?: number
    
    private async ProcessTaskDealWithinRespectiveAuthorityLevels(taskWithChildren: DealWorkflowTaskWithChildren) {
        const { deal } = this.rootStore
        const userId = (!!deal.delegatedAuthorityUserId.value ? deal.delegatedAuthorityUserId.value : deal.submissionUserId)
        if (this.tradingPolicyAssessmentLevels && userId === this.tradingPolicyAssessmentLevelsUserId) {
            return
        }

        await executeLoading("Performing Trading Policy Assessment...", async () => {
            this.tradingPolicyAssessmentLevelsUserId = userId
            const levels = await dealClient.getTradePolicyEvaluation(deal.id, userId)
            this.tradingPolicyAssessmentLevels = this.getRoleAssignments(levels)
            const traderLevel = levels.find(l => l.isTraderWorkflowLevel)
            const meetsTradingPolicy = traderLevel?.meetsTradingPolicy ?? false
            const answerType = (meetsTradingPolicy ? WorkflowTaskAnswerTypeEnum.Yes : WorkflowTaskAnswerTypeEnum.No)
            const answer = taskWithChildren.task.possibleAnswers.find(pa => pa.answerType === answerType)
            if (!answer) {
                messageError({ title: "Configuration Error", content: "The trading policy assessment task is configured incorrectly." })
                return
            }
            this.setTaskAnswer(taskWithChildren.task, answer)

        })
    }

    createNewTaskWithChildren = async (task: DealWorkflowTaskDto, populateFields?: boolean) => {
        var taskWithChildren = new DealWorkflowTaskWithChildren(task)
        if (populateFields) {
            switch (task.type) {
                case WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels:
                    await this.ProcessTaskDealWithinRespectiveAuthorityLevels(taskWithChildren)
                    break;
                case WorkflowTaskTypeEnum.AnswerToQuestion:
                    await this.ProcessTaskAnswerToQuestion(taskWithChildren);
                    break;
                case WorkflowTaskTypeEnum.HasItems:
                case WorkflowTaskTypeEnum.CreatedNoteDuringStatus:
                case WorkflowTaskTypeEnum.AttachedDocumentDuringStatus:
                case WorkflowTaskTypeEnum.ExpiryDateCheck:
                case WorkflowTaskTypeEnum.DealExecutedCheck:
                case WorkflowTaskTypeEnum.DealNotExecutedCheck:
                    const done = await this.taskHasValue(task)
                    if (done !== task.done.value) {
                        updatable(task.done, await this.taskHasValue(task))
                        task.updated = true
                        this.nextStatus.updated = true
                    }
                    break;

            }

        }
        return taskWithChildren
    }

    mapTask = async (task: DealWorkflowTaskDto, status: DealWorkflowStatusDto, populateFields?: boolean) => {
        var taskWithChildren = await this.createNewTaskWithChildren(task, populateFields)
        this.populateNextTaskLevel(status, taskWithChildren)
        return taskWithChildren
    }

    updateTaskTree = async (status: DealWorkflowStatusDto, populateFields?: boolean) => {
        this.alternativeActionId = undefined

        this.tasks = await Promise.all(status.tasks
            .filter(task => !task.precedingAnswerId)
            .map(async task => {
                return await this.mapTask(task, status, populateFields)
            }))
    }

    populateNextTaskLevel = (status: DealWorkflowStatusDto, taskWithChildren: DealWorkflowTaskWithChildren) => {
        const { task } = taskWithChildren
        const newAnswerId = task.workflowTaskAnswerId.value
        if (!newAnswerId) {
            return false
        }
        const newAnswer = task.possibleAnswers.find(pa => pa.id === newAnswerId)
        if (!newAnswer) {
            return false
        }
        for (const subsequentWorkflowTaskId of newAnswer.subsequentWorkflowTaskIds) {
            const subsequentTask = status.tasks.find(t => t.workflowTaskId === subsequentWorkflowTaskId)
            if (subsequentTask) {
                const nextLevel = new DealWorkflowTaskWithChildren(subsequentTask)
                this.populateNextTaskLevel(status, nextLevel)
                taskWithChildren.children.push(nextLevel)
            }
        }
        return true
    }

    updateAlternativeActionId(tasksWithChildren: DealWorkflowTaskWithChildren[]) {
        this.alternativeActionId = undefined
        for (const taskWithChildren of tasksWithChildren) {
            for (const possibleAnswer of taskWithChildren.task.possibleAnswers) {
                if (!!possibleAnswer.alternateWorkflowActionId && taskWithChildren.task.workflowTaskAnswerId.value === possibleAnswer.id) {
                    this.alternativeActionId = possibleAnswer.alternateWorkflowActionId
                    return true
                }
            }
            if (this.updateAlternativeActionId(taskWithChildren.children)) {
                return true
            }
        }

        return false
    }
    taskValueChanged = async (taskWithChildren: DealWorkflowTaskWithChildren, newValue: any): Promise<boolean> => {
        const { task } = taskWithChildren
        switch (task.type) {
            case WorkflowTaskTypeEnum.AnswerToQuestion:
            case WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels:
                const newAnswerId = newValue as number
                const oldAnswerId = task.workflowTaskAnswerId.value
                if (newAnswerId === oldAnswerId) {
                    return true
                }
                const newAnswer = task.possibleAnswers.find(pa => pa.id === newAnswerId)
                if (!newAnswer) {
                    return false
                }
                if (!!newAnswer.attachmentTypeToVerifyId && !(await this.hasAttachmentType(newAnswer.attachmentTypeToVerifyId))) {
                    messageError({
                        content: "This attachment type wasn't found in this deal."
                    })
                    return false
                }
                updatable(task.workflowTaskAnswerId, newAnswerId)
                updatable(task.workflowTaskAnswerText, newAnswer.description)
                this.resetTaskChildrenValues(taskWithChildren)
                taskWithChildren.children = []
                this.populateNextTaskLevel(this.nextStatus, taskWithChildren)

                if (this.updateAlternativeActionId(this.tasks)) {
                    this.alternativeAction = await this.getAction(this.alternativeActionId!)
                } else {
                    this.alternativeAction = undefined
                }

                break;

            default:
                break;
        }

        if (await this.taskHasValue(task, newValue, true)) {
            if (!task.done.value) {
                updatable(task.done, true)
            }
        } else {
            if (task.done.value) {
                updatable(task.done, false)
            }
        }
        task.updated = true
        this.nextStatus.updated = true
        return true
    }

    expiryDateCheckDescription = () => {
        if (!this.rootStore.deal.expiryDate.value || !isValidMoment(this.rootStore.deal.expiryDate.value)) {
            return "Expiry Date not informed yet."
        } else {
            return `Expiry date is ${momentToDateString(this.rootStore.deal.expiryDate.value)}`
        }
    }

    executionCheckDescription = (reverse = false) => {
        if (reverse) {
            if (this.rootStore.deal.executed) {
                return "Deal must have its execution reversed before proceeding."
            } else {
                return `Deal is not executed - you can mark this task as done.`
            }
        } else {
            if (!this.rootStore.deal.executed) {
                return "Deal must be executed before proceeding."
            } else {
                return `Deal was executed on ${momentToDateTimeString(this.rootStore.deal.executionDate.value)}`
            }
        }
    }

    currentStatusHistory: DealWorkflowStatusDto | undefined
    setCurrentStatusHistory = async (status: DealWorkflowStatusDto) => {
        this.currentStatusHistory = status
        await this.updateTaskTree(this.currentStatusHistory)
    }

    initiatedByDescription = (status: DealWorkflowStatusDto) => !!status.dateTimeConfirmed && !!status.initiatedByUserName
        ?
        (status.initiatedByUserName.trim() + ' on ' + momentToDateTimeString(status.dateTimeConfirmed))
        :
        !!status.dateTimeConfirmed
            ?
            (' on ' + momentToDateTimeString(status.dateTimeConfirmed))
            :
            "Not completed"


    assignedToDescription = (status: DealWorkflowStatusDto) => !!status.dateTimeConfirmed
        ?
        (
            status.assigneeUserName
                ?
                status.assigneeUserName
                :
                status.assigneeWorkflowRoleName
        )
        :
        ""

    openAssignments = () => {
        this.assignmentDialogVisible = true
    }

    get assignmentDescription() {
        const assignment = this.assignments.find(a => a.checked)

        let desc
        if (!assignment) {
            desc = <>There is no assignee selected (this is an unexpected behavior).</>
        } else if (assignment.type === WorkflowAssignmentTypeEnum.DealTrader) {
            desc = <>The deal will be assigned to<b>{` ${assignment.name}`}</b>.</>
        } else {
            desc = <>The deal will be assigned to the<b>{` ${assignment.name} `}</b> role. </>
        }

        if (this.assignments.length >= 2) {
            return <>
                {desc}{htmlSpace}<Link onClick={this.openAssignments} style={{ cursor: 'pointer' }}>(Change Assignment)</Link>
            </>
        } else {
            return desc
        }
    }
    assessmentBeingShown = 0
    assignmentShowingTradingPolicy?: WorkflowStatusAssignmentOption
    showTradingPolicyForAssignment = (assignment: WorkflowStatusAssignmentOption) => {
        this.assignmentShowingTradingPolicy = assignment
        this.assessmentBeingShown = 0
    }
    closeTradingPolicyForAssignment = () => {
        this.assignmentShowingTradingPolicy = undefined
    }
    assessmentBeingShownChange = (event: React.ChangeEvent<{}>, value: any) => {
        this.assessmentBeingShown = value
    }

    termInput = createRef<HTMLInputElement | null>()
    termInMonths = 0
    termChanged = false
    termNote = ""
    termOverrideDialogOpen = false
    closeTermOverrideDialog = () => {
        this.termOverrideDialogOpen = false
    }
    get termInMonthsIsOverriden() {
        return (this.rootStore.deal.termInMonthsOverride.value) !== undefined && (this.rootStore.deal.termInMonthsOverride.value) !== null
    }
    setTermChanged = () => {
        this.termChanged = true
    }
    overrideTerm = async () => {
        const hasOverride = this.termInMonthsIsOverriden
        this.termInMonths = (hasOverride ? this.rootStore.deal.termInMonthsOverride.value : 0) ?? 0
        this.termNote = ""
        this.termChanged = false
        this.termOverrideDialogOpen = true
        this.errorHandler.reset()
        setTimeout(() => {
            selectAllTextOnInput(this.termInput.current)
        });
    }

    saveTermOverride = async () => {
        this.errorHandler.reset()
        if (!this.termNote) {
            this.errorHandler.error('termNote', "Note is mandatory")
            return
        }
        this.saveTermOverrideOnDeal(this.termInMonths, this.termNote)
    }

    resetTermOverride = async () => {
        if (await messageYesNo({ title: "Term Override Reset", content: "This action will reset this deal's term to its original value. Would you like to continue?" })) {
            this.saveTermOverrideOnDeal()
        }
    }

    saveTermOverrideOnDeal = async (term?: number, noteContent?: string) => {
        // sets the month override
        updatable(this.rootStore.deal.termInMonthsOverride, term)
        // resets the assignee
        updatable(this.nextStatus.assigneeWorkflowRoleId, undefined)

        if (noteContent) {
            this.rootStore.noteStore.createNoteAndSave(note => {
                updatable(note.noteContent, noteContent)
            })
        }

        if (!await this.rootStore.saveDeal(false)) {
            return
        }

        await executeLoading("Fetching deal...", () => this.rootStore.fetchData(this.rootStore.deal.id))

        this.closeTermOverrideDialog()
        this.closeTradingPolicyForAssignment()
    }

    get overrideActions() {
        let actions: MessageDialogAction[] = []

        if (this.termChanged) {
            actions.push({
                name: "Override",
                callback: this.saveTermOverride,
                title: "Saves the deal with the new term and reasseses trading policy criteria",
                color: 'primary',
            })
        }
        if (this.termInMonthsIsOverriden) {
            actions.push({
                name: "Reset Override",
                callback: this.resetTermOverride,
                title: "Reverts the term to a system-calculated one",
                color: 'primary',
            })
        }
        actions.push({
            name: "Cancel",
            callback: this.closeTermOverrideDialog,
            color: 'primary',
        })
        return actions
    }

    get policyGridDefinition() {
        return [
            {
                field: 'description', title: 'Policy Description', sortable: false, render: row => (
                    <Tooltip title={(
                        <Typography style={{ fontSize: '1.35em' }}>
                            {row.detailedDescription}
                        </Typography>
                    )}>
                        <Typography variant='inherit'>
                            {row.description}
                        </Typography>
                    </Tooltip>
                ),
            }, {
                field: 'policyValue', title: 'Policy Value', sortable: false,
            }, {
                field: 'dealValue', title: 'Deal(s) Value(s)', sortable: false, rendererComponent: observer(({ data }) => {
                    return (
                        <Typography variant='inherit' style={{ color: (data.criteriaMet ? 'green' : 'red') }}>
                            <b>
                                {data.dealValue}{data.isTermCriteria ? (
                                    <>
                                        {htmlSpace}<Link onClick={this.overrideTerm} style={{ cursor: 'pointer' }}>{this.termInMonthsIsOverriden ? '(overriden)' : '(override)'}</Link>
                                    </>
                                ) : null}
                            </b>
                        </Typography>
                    )
                }),
            },
        ] as Column<TraderAuthorityPolicyAssessmentRow>[]
    }
}