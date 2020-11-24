import React from 'react'
import { DealViewStore } from "../../../DealViewStore"
import { makeAutoObservable } from "mobx"
import AddBoxIcon from '@material-ui/icons/AddBox'
import CancelIcon from '@material-ui/icons/Cancel'
import PageviewIcon from '@material-ui/icons/Pageview'
import AttachmentIcon from '@material-ui/icons/Attachment'
import IconButton from '@material-ui/core/IconButton'
import Tooltip from '@material-ui/core/Tooltip'
import { UserListDto, DealAttachmentLinkType } from 'clients/deal-system-client-definitions'
import updatable from 'shared-do-not-touch/utils/updatable'
import ModalState from 'features/shared/helpers/ModalState'

interface DealDelegatedAuthorityStoreProps {
    rootStore: DealViewStore
}
export default class DealDelegatedAuthorityStore {
    constructor(sp: DealDelegatedAuthorityStoreProps) {
        this.sp = sp
        makeAutoObservable(this)
    }
    sp: DealDelegatedAuthorityStoreProps

    get endAdornment() {
        if (!this.sp.rootStore.canChangeDelegatedAuthority) { return null }

        if (!!this.sp.rootStore.deal.delegatedAuthorityUserId.value) {
            return (
                <Tooltip title="Remove Delegated Trading Authority">
                    <IconButton size="small" onClick={this.removeDelegatedAuthority}>
                        <CancelIcon />
                    </IconButton>
                </Tooltip>
            )
        } else {
            return (
                <Tooltip title="Assign Delegated Trading Authority">
                    <IconButton size="small" onClick={this.selectingUser.open}>
                        <AddBoxIcon />
                    </IconButton>
                </Tooltip>
            )
        }
    }

    get attachment() {
        return this.sp.rootStore.attachmentStore.findAttachmentByLinkType(DealAttachmentLinkType.DelegatedTradingAuthorityEvidence)
    }
    showAttachment = () => {
        this.sp.rootStore.attachmentStore.setCurrentAttachment({ rowData: this.attachment })
    }

    selectingUser = new ModalState()
    selectedUser?: UserListDto

    userSelected = (user?: UserListDto) => {
        this.selectingUser.close()
        if (!!user) {
            this.selectedUser = user
            const currentAttachment = this.attachment
            this.sp.rootStore.attachmentStore.setCurrentAttachment({
                attachmentDoneCallback: this.commitDelegatedAuthorityAssignment,
                baseAttachmentForNewVersion: currentAttachment,
                linkType: DealAttachmentLinkType.DelegatedTradingAuthorityEvidence,
                attachmentTypeOtherDescription: "Delegated Trading Authority Evidence",
                canAddVersionAdditionalCriteria: () => true
            })
        }
    }

    removeDelegatedAuthority = () => {
        updatable(this.sp.rootStore.deal.delegatedAuthorityUserId, undefined)
        this.sp.rootStore.deal.delegatedAuthorityUserName = ""
        const currentAttachment = this.sp.rootStore.attachmentStore.findAttachmentByLinkType(DealAttachmentLinkType.DelegatedTradingAuthorityEvidence)
        if (currentAttachment && !currentAttachment.lastVersion?.isLocked) {
            this.sp.rootStore.attachmentStore.deleteAttachment(currentAttachment)
        }
    }

    commitDelegatedAuthorityAssignment = (successful: boolean) => {
        if (successful) {
            updatable(this.sp.rootStore.deal.delegatedAuthorityUserId, this.selectedUser?.id)
            this.sp.rootStore.deal.delegatedAuthorityUserName = this.selectedUser?.name || "Unexpected Behavior"
        } else {
            this.selectedUser = undefined
        }
    }

    get startAdornment() {
        if (!!this.sp.rootStore.deal.delegatedAuthorityUserId.value) {
            return <>
                <Tooltip title="View user that delegated his authority">
                    <IconButton size="small" onClick={this.showingUser.open}>
                        <PageviewIcon />
                    </IconButton>
                </Tooltip>
                {!!this.attachment && (
                    <Tooltip title="View evidence for delegation of authority">
                        <IconButton size="small" onClick={this.showAttachment}>
                            <AttachmentIcon />
                        </IconButton>
                    </Tooltip>
                )}
            </>
        }

        return null
    }

    showingUser = new ModalState()

}