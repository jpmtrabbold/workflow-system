import React from 'react'
import { makeAutoObservable } from "mobx"
import { DealAttachmentsGridDefinition } from "./DealAttachmentsGridDefinition"
import { saveAs } from 'file-saver'
import { DealAttachmentDto, DealAttachmentVersionDto, AttachmentTypeLookupRequest, DealAttachmentLinkType } from "../../../../../../clients/deal-system-client-definitions"
import { DealViewStore } from "../../DealViewStore"
import extendObjectKeepingReference from "shared-do-not-touch/utils/change-object-type"
import { dealClient } from "../../../../../../clients/deal-system-rest-clients"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import fileToBase64, { base64StringToBlob } from "shared-do-not-touch/utils/file-operations"
import { getAzureUserNameForUser } from "../../../../../../clients/azure-authentication"
import fileSizeDescription from "shared-do-not-touch/utils/file-size-description"
import updatable, { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import moment from "moment"
import AddIcon from "@material-ui/icons/Add"
import { CustomTableActions } from 'shared-do-not-touch/material-ui-table/CustomTable'
import CloudDownloadIcon from '@material-ui/icons/CloudDownload'
import JSZip from 'jszip'

export type AttachmentAction = "delete" | "save" | "close" | "save_add_another" | "add_version"
export class DealAttachmentDtoWithLastVersion {
    constructor(dealAttachment: DealAttachmentDto, dealAttachmentIndex?: number, dealAttachmentWithLastVersionIndex?: number) {
        this.dealAttachment = dealAttachment
        this.attachmentIndex = dealAttachmentIndex
        this.dealAttachmentIndex = dealAttachmentWithLastVersionIndex
        let last: DealAttachmentVersionDto | undefined
        this.lastVersionNumber = 0
        this.dealAttachment.versions.forEach((element) => {
            if (!element.deleted) {
                last = element
                this.lastVersionNumber!++
            }
        })

        if (!last) {
            throw Error("Internal error: there is an attachment without any version within")
        }
        this.lastVersion = last
    }

    dealAttachment: DealAttachmentDto
    attachmentIndex?: number
    dealAttachmentIndex?: number
    lastVersion: DealAttachmentVersionDto
    lastVersionNumber: number

    clone = () => {
        const attachmentWithLastVersion = new DealAttachmentDtoWithLastVersion(this.dealAttachment.clone(), this.attachmentIndex, this.dealAttachmentIndex)
        attachmentWithLastVersion.lastVersion = this.lastVersion?.clone()
        attachmentWithLastVersion.lastVersionNumber = this.lastVersionNumber
        return attachmentWithLastVersion
    }
}

export class DealAttachmentVersionDtoWithVersionNumber extends DealAttachmentVersionDto {
    versionNumber: number = 0
    
}

export class DealAttachmentStore {
    constructor(rootStore: DealViewStore) {
        this.rootStore = rootStore
        makeAutoObservable(this)
    }

    rootStore: DealViewStore

    get dealAttachments() {
        const { deal } = this.rootStore
        const attachments = deal && deal.attachments && deal.attachments.data.filter((a => {
            if (a.deleted) { // doesn't show in the table view if deleted
                return false
            } else if (!a.versions.find(v => !v.deleted)) { // doesn't show in the table view if doesn't have active versions
                return false
            }
            return true
        }))

        return attachments.map((a, i) => new DealAttachmentDtoWithLastVersion(a, deal.attachments.data.indexOf(a), i))
    }

    get attachmentVersions() {
        if (!this.currentAttachment)
            return []

        let versionNumber = 0
        return this.currentAttachment.dealAttachment.versions
            .filter(v => !v.deleted)
            .map(v => {
                versionNumber++
                return extendObjectKeepingReference<DealAttachmentVersionDto, DealAttachmentVersionDtoWithVersionNumber>(v, { versionNumber })
            })
            .sort((a, b) => b.versionNumber - a.versionNumber)
    }

    attachmentsGridDefinition = DealAttachmentsGridDefinition(this)

    currentAttachment?: DealAttachmentDtoWithLastVersion
    actionType?: 'add-attachment' | 'add-version' | 'edit-attachment' | 'view-attachment'

    saveAttachmentAndClose = () => this.closeAttachmentWithAction('save')
    saveAttachmentAndAddAnother = () => this.closeAttachmentWithAction('save_add_another')
    deleteAttachmentAndClose = () => this.closeAttachmentWithAction('delete')
    addAttachmentVersion = () => this.closeAttachmentWithAction('add_version')
    closeAttachment = () => this.closeAttachmentWithAction('close')

    private closeAttachmentWithAction = async (action?: AttachmentAction) => {
        let baseAttachmentForNewVersion = this.currentAttachment
        switch (action) {
            case "save":
            case "save_add_another":
                this.saveCurrentAttachment()
                break
            case "delete":
                this.deleteCurrentAttachment()
                break
        }

        this.unsetCurrentAttachment()

        if (action === "save_add_another") {
            setTimeout(() => {
                this.setCurrentAttachment()
            })
        } else if (action === "add_version") {
            setTimeout(() => {
                this.setCurrentAttachment({ baseAttachmentForNewVersion })
            })
        }
    }

    findAttachmentByLinkType = (linkType: DealAttachmentLinkType) => {
        return this.dealAttachments.find(da => da.dealAttachment.linkType === linkType)
    }
    attachmentDoneCallback?: (successful: boolean) => any
    canAddVersionAdditionalCriteria?: () => any

    setCurrentAttachment = ({
        rowData,
        baseAttachmentForNewVersion,
        attachmentDoneCallback,
        attachmentTypeOtherDescription,
        linkType,
        canAddVersionAdditionalCriteria
    }: {
        rowData?: DealAttachmentDtoWithLastVersion,
        baseAttachmentForNewVersion?: DealAttachmentDtoWithLastVersion,
        attachmentDoneCallback?: (successful: boolean) => any,
        attachmentTypeOtherDescription?: string,
        linkType?: DealAttachmentLinkType,
        canAddVersionAdditionalCriteria?: () => any
    } = {}) => {
        this.attachmentDoneCallback = attachmentDoneCallback
        this.canAddVersionAdditionalCriteria = canAddVersionAdditionalCriteria

        if (!!rowData) {
            this.currentAttachment = rowData.clone()
            if (this.attachmentIsFromCurrentUser && !this.anyLockedVersions && !this.rootStore.sp.readOnly && !rowData.dealAttachment.linkType) {
                this.actionType = 'edit-attachment'
            } else {
                this.actionType = 'view-attachment'
            }
        } else {
            const currentVersion = new DealAttachmentVersionDto()

            if (baseAttachmentForNewVersion !== undefined) {
                this.actionType = 'add-version'
                this.currentAttachment = baseAttachmentForNewVersion.clone()
                this.currentAttachment.dealAttachment.versions.push(currentVersion)
                this.currentAttachment.lastVersion = currentVersion
            } else {
                this.actionType = 'add-attachment'
                const newAttachment = new DealAttachmentDto()
                newAttachment.versions.push(currentVersion)

                if (!!attachmentTypeOtherDescription) {
                    initializeUpdatable(newAttachment.attachmentTypeId, this.attachmentTypesOther[0].id)
                    initializeUpdatable(newAttachment.attachmentTypeOtherText, attachmentTypeOtherDescription)
                } else {
                    initializeUpdatable(newAttachment.attachmentTypeId, undefined)
                    initializeUpdatable(newAttachment.attachmentTypeOtherText, undefined)
                }
                newAttachment.linkType = linkType

                this.currentAttachment = new DealAttachmentDtoWithLastVersion(newAttachment)
            }

        }
    }

    unsetCurrentAttachment() {
        this.currentAttachment = undefined
        if (this.attachmentDoneCallback) {
            this.attachmentDoneCallback(false)
            this.attachmentDoneCallback = undefined
        }
    }

    async saveCurrentAttachment() {
        if (!this.currentAttachment) {
            throw Error('Internal error: this.currentAttachment is not set on DealAttachmentStore.saveAttachment')
        }
        if (this.actionType === 'edit-attachment' || this.actionType === 'add-attachment') {
            if (!this.hasOtherAttachmentType) {
                updatable(this.currentAttachment.dealAttachment.attachmentTypeOtherText, undefined)
            }
        }
        if (this.actionType === 'edit-attachment') {
            this.rootStore.deal.attachments.data[this.currentAttachment.attachmentIndex!] =
                this.currentAttachment.dealAttachment.clone()
            this.rootStore.deal.attachments.data[this.currentAttachment.attachmentIndex!].updated = true
        } else if (this.actionType === 'add-version') {
            this.rootStore.deal.attachments.data[this.currentAttachment.attachmentIndex!].versions.push(this.currentAttachment!.lastVersion!)
            this.rootStore.deal.attachments.data[this.currentAttachment.attachmentIndex!].updated = true
        } else if (this.actionType === 'add-attachment') {
            this.rootStore.deal.attachments.data.push(this.currentAttachment.dealAttachment)
        }

        if (this.attachmentDoneCallback) {
            this.attachmentDoneCallback(true)
            this.attachmentDoneCallback = undefined
        }
    }

    deleteCurrentAttachment() {
        if (!this.currentAttachment || this.currentAttachment.attachmentIndex === undefined || this.currentAttachment.dealAttachmentIndex === undefined)
            throw new Error("deleteAttachment function was called but this.currentAttachment or this.currentAttachment.dealAttachmentIndex wasn't set")

        this.deleteAttachment(this.currentAttachment)
    }

    deleteAttachment = (attachment: DealAttachmentDtoWithLastVersion) => {
        if (attachment.attachmentIndex === undefined || attachment.dealAttachmentIndex === undefined)
            throw new Error("deleteAttachment function was called but this.currentAttachment or this.currentAttachment.dealAttachmentIndex wasn't set")

        this.rootStore.deal.attachments.data[attachment.attachmentIndex].updated = true
        this.dealAttachments[attachment.dealAttachmentIndex].lastVersion.deleted = true
    }

    attachmentTypes = [] as AttachmentTypeLookupRequest[]
    fetchAttachmentTypes = async () => {
        try {
            this.attachmentTypes = await dealClient.getAttachmentTypes()
        } catch (error) {
            return false
        }

        return true
    }

    get attachmentTypesOther() {
        return this.attachmentTypes.filter(at => at.other)
    }

    isAttachmentTypeOther(id?: number) {
        return !!this.attachmentTypesOther.find(ato => ato.id === id)
    }

    handleFiles = (files: File[]) => {
        this.setFileToCurrentVersion(files[0])
    }

    setFileToCurrentVersion = async (file: File) => {
        if (!file) {
            return
        }
        const parts = file.name.split('.')

        executeLoading('Working on it...', async () => {
            const currentVersion = this.currentAttachment!.lastVersion!
            if (!currentVersion) return

            currentVersion.fileName = parts.slice(0, -1).join('.')
            currentVersion.fileExtension = (parts.pop() || "").toUpperCase()
            currentVersion.fileSizeInBytes = file.size
            currentVersion.fileBase64 = await fileToBase64(file)
            currentVersion.creationUserName = getAzureUserNameForUser()
            currentVersion.createdDate = moment()
        })
    }

    downloadVersion = async (version: DealAttachmentVersionDto) => {
        executeLoading('Downloading attachment', async () => {
            const blob = await this.getBlobForVersion(version)
            const fileName = this.getFilenameForVersion(version)
            saveAs(blob, fileName)
        })
    }
    getFilenameForVersion = (version: DealAttachmentVersionDto) => {
        let fileName = version.fileName + '.' + version.fileExtension
        const { dealNumber } = this.rootStore.deal
        if (!fileName.startsWith(dealNumber)) {
            fileName = dealNumber + " - " + fileName
        }
        return fileName
    }
    getBlobForVersion = async (version: DealAttachmentVersionDto) => {
        if (version.id) {
            return (await dealClient.downloadAttachmentVersion(version.id))!.data
        } else {
            return base64StringToBlob(version.fileBase64)
        }
    }

    getAttachmentTypeDescription = (id?: number, caterForNotSpecified = true) => {
        return (!!id ? this.attachmentTypes.find(at => at.id === id)!.name : caterForNotSpecified ? "Not specified" : "")
    }

    get attachmentIsFromCurrentUser() {
        const { currentUserId } = this.rootStore.deal
        const creationUserId = this.currentAttachment?.lastVersion?.creationUserId
        return (!currentUserId || !creationUserId || currentUserId === creationUserId)
    }

    get existingAttachment() {
        return this.actionType === 'edit-attachment' || this.actionType === 'view-attachment'
    }

    get attachmentIsLinked() {
        return !!this.currentAttachment?.dealAttachment.linkType
    }

    get canDeleteAttachment() {
        if (this.attachmentIsLinked) {
            return false
        }

        const isLocked = this.currentAttachment?.lastVersion?.isLocked || false
        return (!this.rootStore.sp.readOnly
            && (this.existingAttachment)
            && this.attachmentIsFromCurrentUser
            && !isLocked)
    }
    get shouldShowVersionHistory() {
        const currentAttachment = this.currentAttachment!
        return (this.existingAttachment && !!currentAttachment.lastVersionNumber && currentAttachment.lastVersionNumber > 1)
    }
    get canAddVersion() {
        if (this.attachmentIsLinked) {
            if (!this.canAddVersionAdditionalCriteria || !this.canAddVersionAdditionalCriteria()) {
                return false
            }
        }

        return !this.rootStore.sp.readOnly
            && this.existingAttachment
            && this.rootStore.deal.userParticipatedOnThisDeal
    }
    get hasSaveAndAddAnotherButton() {
        if (this.attachmentIsLinked) {
            return false
        }
        const hasFile = !!this.currentAttachment?.lastVersion?.createdDate
        if (!hasFile) {
            return false
        }
        return this.actionType === 'add-attachment'
    }
    get hasSaveButton() {
        const hasFile = !!this.currentAttachment?.lastVersion?.createdDate
        if (!hasFile) {
            return false
        }
        return this.actionType === 'add-attachment' || this.actionType === 'edit-attachment' || this.actionType === 'add-version'
    }

    get anyLockedVersions() {
        return this.currentAttachment?.dealAttachment.versions.find(v => v.isLocked)
    }

    get attachmentTypeEnabled() {
        if (!!this.currentAttachment?.dealAttachment?.linkType) {
            return false
        } else if (this.actionType === 'add-attachment') {
            return true
        } else if (this.actionType === 'add-version' || this.actionType === 'view-attachment') {
            return false
        } else if (this.actionType === 'edit-attachment') {
            if (this.anyLockedVersions) {
                return false
            } else {
                return true
            }
        }
        return false
    }

    get hasOtherAttachmentType() {
        return this.isAttachmentTypeOther(this.currentAttachment?.dealAttachment.attachmentTypeId.value)
    }

    get nameFieldEnabled() {
        return this.actionType === 'add-attachment' || this.actionType === 'add-version'
    }

    get fileSizeDescription() {
        return fileSizeDescription(this.currentAttachment!.lastVersion!.fileSizeInBytes)
    }

    versionHistoryOpen = false
    openVersionHistory = () => this.versionHistoryOpen = true
    closeVersionHistory = () => this.versionHistoryOpen = false

    get actions() {
        const actions: CustomTableActions<DealAttachmentDtoWithLastVersion> = {
            freeActions: [], rowActions: [], multipleRowActions: []
        }
        if (this.rootStore.deal.userParticipatedOnThisDeal && !this.rootStore.sp.readOnly) {
            actions.freeActions!.push({
                icon: () => <AddIcon />,
                title: "Add Attachment",
                callback: () => this.setCurrentAttachment(),
            })
        }

        actions.multipleRowActions?.push({
            title: 'Download Selected Attachments',
            icon: () => <CloudDownloadIcon />,
            callback: this.downloadSelectedAttachments
        })
        return actions
    }
    downloadSelectedAttachments = async (attachments: DealAttachmentDtoWithLastVersion[]) => {
        if (attachments.length === 1) {
            this.downloadVersion(attachments[0].lastVersion!)
            return
        }
        executeLoading("Downloading Attachments...", async () => {
            var zip = new JSZip()
            for (const attachment of attachments) {
                const blob = await this.getBlobForVersion(attachment.lastVersion!)
                const filename = this.getFilenameForVersion(attachment.lastVersion!)
                zip.file(filename, blob)
            }
            const content = await zip.generateAsync({ type: 'blob' })
            saveAs(content, `${this.rootStore.deal.dealNumber ?? "New Deal"} - Attachments.zip`)
        })
    }

    onDragEnter = () => {
        !this.currentAttachment && this.setCurrentAttachment()
    }
    onRowClick = (rowData?: DealAttachmentDtoWithLastVersion) => !!rowData && this.setCurrentAttachment({ rowData })
}

