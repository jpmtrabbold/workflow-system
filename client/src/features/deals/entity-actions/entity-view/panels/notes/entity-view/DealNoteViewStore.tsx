import React from 'react'
import useButtonWithKeyDownListener from 'shared-do-not-touch/hooks/useButtonWithKeyDownListener'
import { makeAutoObservable } from "mobx"
import { DealViewStore } from '../../../DealViewStore'
import Tooltip from "@material-ui/core/Tooltip"
import Button from "@material-ui/core/Button"
import { messageConfirm } from 'shared-do-not-touch/material-ui-modals'
import updatable from 'shared-do-not-touch/utils/updatable'
import { NoteReminderTypeEnum, UsersListRequest } from 'clients/deal-system-client-definitions'
import { AutoCompleteFieldOption, AutoCompleteFieldRemoteOptionsFunction } from 'features/shared/components/chip-input-auto-suggest/AutoCompleteField'
import { removeFromArray } from 'shared-do-not-touch/utils/utils'
import { userClient } from 'clients/deal-system-rest-clients'
import { getAzureUserNameForUser } from 'clients/azure-authentication'
import { momentToDateTimeString } from 'features/shared/helpers/utils'

export interface DealNoteViewStoreProps {
    rootStore: DealViewStore
}
export class DealNoteViewStore {
    constructor(sp: DealNoteViewStoreProps) {
        this.sp = sp
        makeAutoObservable(this)
    }
    sp: DealNoteViewStoreProps
    get noteStore() { return this.sp.rootStore.noteStore }
    get currentNote() { return this.noteStore.currentNote }

    handleOk = () => {
        this.noteStore.closeNote("save")
    }

    handleOkAdd = () => {
        this.noteStore.closeNote("save_add_another")
    }

    handleClose = async () => {
        if (this.noteHasContent && this.userEditThisNote) {
            if (await messageConfirm({ content: "Are you sure you want to close? All unsaved note content will be lost.", title: "Unsaved changes" })) {
                this.noteStore.closeNote("close")
            }
        } else {
            this.noteStore.closeNote("close")
        }
    }

    get noteHasContent() {
        return !!this.currentNote?.noteContent.value
    }

    handleDelete = () => {
        this.noteStore.closeNote("delete")
    }

    get userEditThisNote() {
        if (!this.currentNote) { return false }

        if (this.sp.rootStore.sp.readOnly) {
            return false
        }

        // a locked note can't be edited
        if (this.currentNote.isLocked) {
            return false
        }

        // user only edits his own notes
        if (!!this.currentNote.noteCreatorId && this.currentNote.noteCreatorId !== this.sp.rootStore.deal.currentUserId) {
            return false
        }
        return true
    }

    get isCurrentNoteNew() {
        return this.noteStore.currentNoteIndex < 0
    }

    get saveAddAnotherButtonProps(): Parameters<typeof useButtonWithKeyDownListener>[0] {
        return {
            action: () => this.noteStore.closeNote("save_add_another"),
            keyboardCondition: e => e.key === "Enter" && e.shiftKey && !e.ctrlKey,
            conditionToApplyListener: () => this.isCurrentNoteNew,
            button: (
                <Tooltip title="Confirms this note and open new window to add another right away (SHIFT + ENTER)">
                    <span>
                        <Button
                            onClick={this.handleOkAdd}
                            disabled={!this.noteHasContent}
                            color="primary"
                        >
                            Ok +
                        </Button>
                    </span>
                </Tooltip>

            )
        }
    }

    get saveButtonProps(): Parameters<typeof useButtonWithKeyDownListener>[0] {
        return {
            action: () => this.noteStore.closeNote("save"),
            keyboardCondition: e => e.key === "Enter" && !e.shiftKey && e.ctrlKey,
            conditionToApplyListener: () => this.userEditThisNote,
            button: (
                <Tooltip title="Confirms this note (CTRL + ENTER)">
                    <span>
                        <Button
                            onClick={this.handleOk}
                            disabled={!this.noteHasContent}
                            color="primary"
                        >
                            Ok
                        </Button>
                    </span>
                </Tooltip>
            )
        }
    }

    get deleteButtonProps(): Parameters<typeof useButtonWithKeyDownListener>[0] {
        return {
            action: () => this.handleDelete(),
            keyboardCondition: e => e.key === "Delete" && e.shiftKey && !e.ctrlKey,
            conditionToApplyListener: () => !this.isCurrentNoteNew && this.userEditThisNote,
            button: (
                <Tooltip title="Deletes this note (SHIFT + DELETE)">
                    <span>
                        <Button onClick={this.handleDelete} disabled={this.currentNote?.isLocked} color="secondary">
                            Delete
                        </Button>
                    </span>
                </Tooltip>
            )
        }
    }

    get canAddReminder() {
        return !this.hasReminder && this.userEditThisNote
    }
    get canDeleteReminder() {
        return this.hasReminder && this.userEditThisNote
    }
    get hasReminder() {
        return !!this.currentNote?.reminderType.value
    }
    addReminder = () => {
        updatable(this.currentNote!.reminderType, NoteReminderTypeEnum.Me)
    }
    deleteReminder = () => {
        const note = this.currentNote!
        updatable(note.reminderType, undefined)
        updatable(note.reminderDateTime, undefined)
        updatable(note.reminderEmailAccounts, undefined)
    }
    getUsers: AutoCompleteFieldRemoteOptionsFunction = async ({
        query,
        pageNumber,
        pageSize,
    }) => {
        const request = new UsersListRequest()
        request.pageNumber = pageNumber
        request.pageSize = pageSize
        request.searchString = query
        const response = await userClient.list(request)
        return {
            totalCount: response.totalRecords,
            results: response.users.map(u => ({ name: u.username })),
        }
    }
    get emailAccountsLabel() {
        return (this.currentNote && this.currentNote.reminderType.value === NoteReminderTypeEnum.Emails)
            ? "E-mail accounts to be notified"
            : "Additional e-mail accounts to be notified"
    }
    get selected() {
        const accounts = this.currentNote?.reminderEmailAccounts.value
        if (!accounts) {
            return []
        }
        const selected = (accounts).split(";").map(e => ({ name: e }))
        console.log(selected)
        return selected
    }
    addEmail = (added: AutoCompleteFieldOption) => {
        const selected = this.selected
        selected.push(added)
        updatable(this.currentNote!.reminderEmailAccounts, selected.map(s => s.name).join("; "))
    }
    removeEmail = (removed: AutoCompleteFieldOption) => {
        const selected = this.selected
        removeFromArray(selected, i => i === removed)
        updatable(this.currentNote!.reminderEmailAccounts, selected.map(s => s.name).join("; "))
    }
    clearEmails = () => {
        updatable(this.currentNote!.reminderEmailAccounts, "")
    }
    get noteIsFromCurrentUser() {
        return !!this.noteUserRoles?.currentUser
    }

    get noteUserRoles() {
        const userId = this.currentNote?.noteCreatorId
        return this.sp.rootStore.userWorkflowRoles
            .find(h => !!userId ? h.id === userId : h.currentUser)
    }
    get userRoles() {
        return this.noteUserRoles
            ?.results.map(r => r.name).join('; ')
    }
    get myRoleReminderTypeName() {
        const roles = this.noteUserRoles
        if (!roles) {
            return "My Roles"
        }
        const caption =
            this.noteIsFromCurrentUser
                ? ((roles.results.length ?? 0) > 1 ? "My Roles" : "My Role")
                : "User " + ((roles.results.length ?? 0) > 1 ? "Roles" : "Role")
        return caption + ` (${this.userRoles})`
    }
    get reminderLookups() {
        return this.sp.rootStore.dealTypeConfiguration?.noteReminderTypeLookups.map(l => {
            if (l.id === NoteReminderTypeEnum.MyRole) {
                return { ...l, name: this.myRoleReminderTypeName }
            }
            return l
        })
    }

    get createdBy() {
        const user = (this.currentNote?.noteCreatorName ?? getAzureUserNameForUser())
        return `${user} ${this.currentNote?.createdDate.value ? "at " + momentToDateTimeString(this.currentNote?.createdDate.value) : ""}`
    }
}