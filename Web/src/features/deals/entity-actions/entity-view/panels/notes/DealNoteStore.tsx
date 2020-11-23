import React from 'react'
import { DealViewStore } from "../../DealViewStore"
import { makeAutoObservable } from "mobx"
import { Column } from 'shared-do-not-touch/material-ui-table/CustomTable'
import { DealNoteDto } from "../../../../../../clients/deal-system-client-definitions"
import updatable from "shared-do-not-touch/utils/updatable"
import { getAzureUserNameForUser } from "../../../../../../clients/azure-authentication"
import moment from "moment"
import AddIcon from "@material-ui/icons/Add"
import { NoteAction } from "./entity-view/DealNoteView"
import { deferAction } from 'shared-do-not-touch/utils/utils'

export class DealNoteStore {
    constructor(rootStore: DealViewStore) {
        this.rootStore = rootStore
        makeAutoObservable(this)
    }

    rootStore: DealViewStore

    get dealNotes() {
        const { deal } = this.rootStore
        return deal && deal.notes && deal.notes.data.filter((t => !t.deleted))
    }

    noteGridDefinition: Column<DealNoteDto>[] = []
    currentNote?: DealNoteDto

    currentNoteIndex = -1

    noteHasContent = false

    createNoteAndSave = async (funcThatSetsFields: (note: DealNoteDto) => any) => {
        await this.setCurrentNote()
        funcThatSetsFields(this.currentNote!)
        await this.saveNote()
        this.unsetCurrentNote()
    }

    setCurrentNote = async (rowData?: DealNoteDto) => {
        const promised = await this.rootStore.fetchNotesPromise
        if (rowData) {
            this.currentNoteIndex = promised.data.indexOf(rowData)
            if (this.currentNoteIndex < 0)
                throw new Error("rowData couldn't be found on store.deal.notes")
            this.currentNote = rowData!.clone()
            this.currentNote.noteContent.updated = false
        } else {
            this.currentNote = new DealNoteDto()
            this.currentNote.noteContent.value = ""
        }
    }

    unsetCurrentNote() {
        this.currentNote = undefined
        this.currentNoteIndex = -1
    }

    async saveNote() {
        if (this.currentNoteIndex >= 0) {

            this.rootStore.deal.notes.data[this.currentNoteIndex] = this.currentNote!
        } else {
            updatable(this.currentNote!.createdDate, moment())
            this.currentNote!.noteCreatorName = getAzureUserNameForUser()
            this.rootStore.deal.notes.data.push(this.currentNote!)
        }
    }

    deleteNote() {
        if (!this.currentNote || this.currentNoteIndex < 0)
            throw new Error("deleteNote function was called but this.currentNote or this.currentNoteIndex wasn't set")

        if (this.currentNote.id && this.currentNote.id > 0) {
            // sets as deleted, so it can be deleted on the back-end
            this.rootStore.deal.notes.data[this.currentNoteIndex].deleted = true
        } else {
            this.rootStore.deal.notes.data.splice(this.currentNoteIndex, 1)
        }
    }

    newNote = () => this.setCurrentNote()

    closeNote = async (action: NoteAction) => {
        switch (action) {
            case "save":
            case "save_add_another":
                this.saveNote()
                break
            case "delete":
                this.deleteNote()
                break
        }

        this.unsetCurrentNote()

        if (action === "save_add_another") {
            deferAction(() => {
                this.newNote()
            })
        }
    }

    get actions() {
        if (this.rootStore.deal.userParticipatedOnThisDeal && !this.rootStore.sp.readOnly) {
            return {
                freeActions: [
                    {
                        icon: () => <AddIcon />,
                        title: "Create Note",
                        callback: this.newNote,
                    }
                ]
            }
        } else {
            return {}
        }
    }

    
}

