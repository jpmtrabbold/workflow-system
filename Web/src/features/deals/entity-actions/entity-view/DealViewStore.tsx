import React from 'react'
import { DealDto, DealTypeConfigurationResponse, LazyLoadedDataStateEnum, LookupRequestHeader } from "../../../../clients/deal-system-client-definitions"
import { dealClient, userClient } from "../../../../clients/deal-system-rest-clients"
import { action, makeAutoObservable } from "mobx"
import DealInfoPanelView from "./panels/info/DealInfoPanelView"
import { createContext } from "react"
import DealItemsPanelView from "./panels/deal-items/DealItemsPanelView"
import DealWorkflowPanelView from "./panels/workflow/DealWorkflowPanelView"
import { DealItemStore } from "./panels/deal-items/DealItemStore"
import { DealInfoStore } from "./panels/info/DealInfoStore"
import DealNotesPanelView from "./panels/notes/DealNotesPanelView"
import { DealNoteStore } from "./panels/notes/DealNoteStore"
import { DealWorkflowStore } from "./panels/workflow/DealWorkflowStore"
import { DealAttachmentStore } from "./panels/attachments/DealAttachmentStore"
import DealAttachmentsPanelView from "./panels/attachments/DealAttachmentsPanelView"
import { messageConfirm, snackbar, messageYesNo } from "shared-do-not-touch/material-ui-modals"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"
import { DealWorkflowActionButtons } from "./DealWorkflowActionButtons"
import Button from "@material-ui/core/Button"
import Tooltip from '@material-ui/core/Tooltip'
import updatable from 'shared-do-not-touch/utils/updatable'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import moment from 'moment'
import { observer } from 'mobx-react-lite'
import CircularProgress from '@material-ui/core/CircularProgress'
import IconButton from '@material-ui/core/IconButton'
import { messageError } from 'shared-do-not-touch/material-ui-modals'
import { DealViewProps } from './DealView'
import DisposableReactionsStore from 'shared-do-not-touch/mobx-utils/DisposableReactionsStore'

export type DealPanelIdentifier = 'dealInfo' | 'dealItems' | 'notes' | 'attachments' | 'lifeCycle'
export interface PanelComponentProps {
    fullscreen?: boolean
    fullscreenExited?: () => any
}

export type DealPanel = {
    identifier: DealPanelIdentifier
    title: string
    titleAdornment?: React.FunctionComponent
    expanded: boolean
    component: React.ComponentType<PanelComponentProps>
    visible: boolean
    fullscreen?: boolean
    hasOwnFullscreenMethod?: boolean
}

const loadingIcon = (
    <Tooltip title='Loading...'>
        <IconButton>
            <CircularProgress size={20} />
        </IconButton>
    </Tooltip>
)

export class DealViewStore{
    constructor(sp: DealViewProps) {
        this.sp = sp
        makeAutoObservable(this)
    }

    sp

    itemStore = new DealItemStore(this)
    infoStore = new DealInfoStore(this)
    noteStore = new DealNoteStore(this)
    workflowStore = new DealWorkflowStore(this)
    attachmentStore = new DealAttachmentStore(this)

    creation = true

    deal = new DealDto()

    dealBackup = ""
    dealTypeConfiguration: DealTypeConfigurationResponse | undefined

    loaded = false

    panels: DealPanel[] = [
        {
            identifier: 'dealInfo',
            title: 'Deal Info',
            expanded: true,
            component: DealInfoPanelView,
            visible: true,
            hasOwnFullscreenMethod: false,
        },
        {
            identifier: 'dealItems',
            title: 'Items',
            titleAdornment: observer(() => {
                return this.deal.items.state === LazyLoadedDataStateEnum.Loading ? loadingIcon : null
            }),
            expanded: false,
            component: DealItemsPanelView,
            visible: true,
            hasOwnFullscreenMethod: true,
        },
        {
            identifier: 'notes',
            title: 'Notes',
            titleAdornment: observer(() => {
                return this.deal.notes.state === LazyLoadedDataStateEnum.Loading ? loadingIcon : null
            }),
            expanded: false,
            component: DealNotesPanelView,
            visible: true,
            hasOwnFullscreenMethod: true,
        },
        {
            identifier: 'attachments',
            title: 'Attachments',
            titleAdornment: observer(() => {
                return this.deal.attachments.state === LazyLoadedDataStateEnum.Loading ? loadingIcon : null
            }),
            expanded: false,
            component: DealAttachmentsPanelView,
            visible: true,
            hasOwnFullscreenMethod: true,
        },
        {
            identifier: 'lifeCycle',
            title: 'Deal Life-Cycle',
            expanded: false,
            component: DealWorkflowPanelView,
            visible: true,
            hasOwnFullscreenMethod: false,
        },
    ]

    onLoad = async () => {
        if (this.sp.dealId) {
            if (!await this.fetchData(this.sp.dealId)) {
                this.sp.onEntityClose && this.sp.onEntityClose()
                return false
            }

        } else {
            if (!await this.setupNewDeal()) {
                this.sp.onEntityClose && this.sp.onEntityClose()
                return false
            }
            this.loaded = true
        }
        return true
    }

    get mainButtons() {
        const buttons: JSX.Element[] = []
        if (!this.sp.readOnly) {
            buttons.push(<DealWorkflowActionButtons key={0} actions={this.workflowStore.currentActions} carryOutAction={this.workflowStore.carryOutAction} />)
            buttons.push(
                <Tooltip key={1} title='Save all the changes made so far and closes the deal.'>
                    <Button color="inherit" onClick={this.handleSaveAndClose}>
                        save and close
                    </Button>
                </Tooltip>
            )
            buttons.push(
                <Tooltip key={2} title='Save all the changes made so far.'>
                    <Button color="inherit" onClick={this.handleSave}>
                        save
                    </Button>
                </Tooltip>
            )
            if (this.sp.execution) {
                buttons.push((
                    <Tooltip key={2} title='Save the deal and commit the executed deal items.'>
                        <Button color="inherit" onClick={this.handleExecute}>
                            Commit Execution
                        </Button>
                    </Tooltip>
                ))
            }
        }
        return buttons
    }

    handleExecute = async () => {
        if (await messageYesNo({ title: "Deal Execution", content: "Do you confirm this deal's execution?" })) {
            updatable(this.deal.executionDate, moment())
            if (await this.saveDeal(true, `Deal ${this.deal.dealNumber} was executed successfully.`)) {
                this.sp.onEntityClose && this.sp.onEntityClose()
            }
        }
    }

    handleSaveAndClose = async () => {
        if (await this.saveDeal()) {
            this.sp.onEntityClose && this.sp.onEntityClose()
        }
    }

    handleSave = async () => {
        if (await this.saveDeal(false)) {
            if (!(await executeLoading("Fetching data after saving...", () => this.fetchData(this.deal.id)))) {
                this.sp.onEntityClose && this.sp.onEntityClose()
                messageInfo({
                    title: 'Deal Fetching Error',
                    content: 'The deal was saved successfully but threw an error when fetching the up-to-date data.'
                })
            } else {
                snackbar({ title: `Deal saved successfully`, anchorOrigin: { horizontal: 'right', vertical: 'bottom' } })
            }
        }
    }

    handleClose = async () => {
        if (this.isDealUpdated()) {
            if (await messageConfirm({
                content: "Are you sure you want to close? There are unsaved changes.",
                title: 'Unsaved changes'
            })) {
                this.sp.onEntityClose && this.sp.onEntityClose()
            }
        } else {
            this.sp.onEntityClose && this.sp.onEntityClose()
        }
    }

    get canChangeDelegatedAuthority() {
        if (this.sp.readOnly) { return false }

        if (!this.deal?.assignedToSelf) { return false }

        if (!this.dealTypeConfiguration?.currentWorkflowStatusConfig?.allowsEditDelegatedAuthority) { return false }

        return true
    }

    get canEditDealBasicInfo() {
        if (this.sp.readOnly) { return false }

        if (!this.deal?.assignedToSelf) { return false }

        if (!this.dealTypeConfiguration || !this.dealTypeConfiguration?.currentWorkflowStatusConfig) { return true }

        if (!this.dealTypeConfiguration?.currentWorkflowStatusConfig?.allowsDealEditing) { return false }

        return true
    }

    setPanelVisibility = (identifier: DealPanelIdentifier, visible: boolean) => {
        const panel = this.panels.find(p => p.identifier === identifier)
        if (!panel) {
            throw new Error('Panel not found')
        }
        panel.visible = visible
    }

    shouldPreventUnloadPage = () => {
        if (this.isDealUpdated()) {
            return true
        }
        return false
    }

    isDealUpdated() {
        if (this.sp.readOnly) {
            return false
        }
        this.preDealSave()
        if (this.dealBackup !== JSON.stringify(this.deal))
            return true
        return false
    }
    
    disposable?: DisposableReactionsStore

    setDealObservations() {
        // set new observations here
        setTimeout(() => {
            this.disposable?.registerAutorun(async () => {
                if (!!this.deal.dealTypeId.value) {
                    await this.assignDealTypeConfiguration(true)
                }
                else {
                    this.itemStore.cleanUpGridDefinition()
                }
            })

            this.disposable?.registerReaction(() => this.deal.dealCategoryId.value, this.infoStore.dealCategoryChanged)
            this.disposable?.registerReaction(() => this.deal.dealCategoryId.value, this.itemStore.dealCategoryChanged)
        })
    }

    preDealSave = () => {

    }

    validate = async () => {
        if (this.itemStore.executionStore.dealHasExecutionErrors()) {
            await messageError({ content: "Deal contains errors on the executions. Please correct them before proceeding." })
            return false
        }
        return true
    }

    async saveDeal(showSnackbar = true, alternativeMessage: (string | undefined) = undefined) {
        if (!await this.validate()) {
            return false
        }
        return await executeLoading("Saving...", async () => {
            try {
                this.preDealSave()
                var result = await dealClient.post(this.deal)
                if (!this.deal.id) {
                    this.deal.id = result.dealId
                }
                if (showSnackbar) {
                    snackbar({ title: alternativeMessage || `Deal ${result.dealNumber} saved successfully` })
                }
                if (result.warningMessages?.length > 0) {
                    messageInfo({
                        title: "Warning",
                        content: "Deal was saved succesfully, but the e-mail notifications could not be sent. " +
                            "Reason: " + result.warningMessages.reduce((acm, value) => acm + "\n" + value)
                    })
                }
            } catch {
                return false
            }
            return true
        })
    }

    async fetchData(dealId?: number) {
        if (!dealId)
            throw new Error('Internal error: dealId does not have a value')

        this.creation = false
        if (!await this.fetchDeal(dealId))
            return false

        if (!await this.fetchRelatedData()) {
            return false
        }

        this.loaded = true

        this.lazyLoad()
        return true
    }

    lazyLoad = async () => {
        this.setDealObservations()
        this.fetchItems()
        this.fetchDealTypeConfiguration()
        this.workflowStore.setupExistingDeal()
        this.fetchNotes()
        this.fetchAttachments()
        this.fetchAdditionalData()
    }

    fetchDealTypeConfigurationPromise: ReturnType<typeof dealViewStoreInstance.assignDealTypeConfiguration> = new Promise(() => undefined)
    fetchDealTypeConfiguration = () => {
        this.fetchDealTypeConfigurationPromise = this.assignDealTypeConfiguration()
    }

    fetchItemsPromise: ReturnType<typeof dealViewStoreInstance.fetchItemsAux> = new Promise(() => undefined)
    async fetchItems() {
        this.deal.items.state = LazyLoadedDataStateEnum.Loading
        this.fetchItemsPromise = this.fetchItemsAux()
    }

    fetchItemsAux = async () => {
        await action(async () => {
            try {
                const dealId = this.deal.id
                const productsPromise = this.itemStore.fetchProducts()
                if (dealId) {
                    this.deal.items.data = await dealClient.getItems(dealId)
                }
                if (await productsPromise) {
                    this.deal.items.state = LazyLoadedDataStateEnum.Ready
                } else {
                    this.deal.items.state = LazyLoadedDataStateEnum.NotLoaded
                }
            } catch (error) {
                this.deal.items.state = LazyLoadedDataStateEnum.NotLoaded
            }
            this.dealBackup = JSON.stringify(this.deal)
        })()

        const that = this
        return {
            get loaded() { return that.deal.items.state === LazyLoadedDataStateEnum.Ready },
            get filteredData() { return that.itemStore.dealItems },
            get data() { return that.deal.items.data }
        }
    }

    fetchNotesPromise: ReturnType<typeof dealViewStoreInstance.fetchNotesAux> = new Promise(() => undefined)
    async fetchNotes() {
        this.deal.notes.state = LazyLoadedDataStateEnum.Loading
        this.fetchNotesPromise = this.fetchNotesAux()
    }

    fetchNotesAux = async () => {
        await action(async () => {
            try {
                const dealId = this.deal.id
                if (dealId) {
                    this.deal.notes.data = await dealClient.getNotes(dealId)
                }
                this.deal.notes.state = LazyLoadedDataStateEnum.Ready
            } catch (error) {
                this.deal.notes.state = LazyLoadedDataStateEnum.NotLoaded
            }
            this.dealBackup = JSON.stringify(this.deal)
        })()
        const that = this
        return {
            get loaded() { return that.deal.notes.state === LazyLoadedDataStateEnum.Ready },
            get filteredData() { return that.noteStore.dealNotes },
            get data() { return that.deal.notes.data }
        }
    }

    fetchAttachmentsPromise: ReturnType<typeof dealViewStoreInstance.fetchAttachmentsAux> = new Promise(() => undefined)
    async fetchAttachments() {
        this.deal.attachments.state = LazyLoadedDataStateEnum.Loading
        this.fetchAttachmentsPromise = this.fetchAttachmentsAux()
    }

    fetchAttachmentsAux = async () => {
        await action(async () => {
            try {
                const attTypesPromise = this.attachmentStore.fetchAttachmentTypes()
                const dealId = this.deal.id
                if (dealId) {
                    this.deal.attachments.data = await dealClient.getAttachments(dealId)
                }
                if (await attTypesPromise) {
                    this.deal.attachments.state = LazyLoadedDataStateEnum.Ready
                } else {
                    this.deal.attachments.state = LazyLoadedDataStateEnum.NotLoaded
                }
            } catch (error) {
                this.deal.attachments.state = LazyLoadedDataStateEnum.NotLoaded
            }
            this.dealBackup = JSON.stringify(this.deal)
        })()
        const that = this
        return {
            get loaded() { return that.deal.attachments.state === LazyLoadedDataStateEnum.Ready },
            get filteredData() { return that.attachmentStore.dealAttachments },
            get data() { return that.deal.attachments.data }
        }
    }

    fetchAdditionalData = async () => {
        const { filteredData } = await this.fetchNotesPromise
        const userIds = [...new Set(filteredData.map(n => n.noteCreatorId))] // using Set to remove duplicates
        this.userWorkflowRoles = await userClient.getWorkflowRolesForUsersAndCurrent(userIds)
    }
    userWorkflowRoles: LookupRequestHeader[] = []

    async setupNewDeal() {
        this.creation = true
        this.infoStore.setupNewDeal()
        this.workflowStore.setupNewDeal()

        if (!await this.fetchRelatedData()) {
            return false
        }
        this.setDealObservations()
        this.dealBackup = JSON.stringify(this.deal)

        this.lazyLoad()
        return true
    }

    fetchRelatedData = async () => {
        const dealCategoriesPromise = this.infoStore.fetchDealCategories()
        if (!await dealCategoriesPromise) {
            return false
        }

        return true
    }

    async fetchDeal(id: number): Promise<boolean> {
        try {
            this.deal = await dealClient.get(id)
            this.dealBackup = JSON.stringify(this.deal)
            return true
        } catch {
            return false
        }
    }

    static fetchDealTypeConfiguration = async (deal: DealDto, fromDealTypeChanging: boolean = false) => {
        const fieldSetId = ((fromDealTypeChanging || !deal.dealItemFieldsetId.value) ? 0 : deal.dealItemFieldsetId.value)
        const workflowSetId = ((fromDealTypeChanging || !deal.workflowSetId.value) ? 0 : deal.workflowSetId.value)
        try {
            return await dealClient.getDealTypeConfiguration(deal.dealTypeId.value!, fieldSetId, workflowSetId, (!!deal.currentDealWorkflowStatusId ? deal.currentDealWorkflowStatusId : undefined))
        } catch (error) {

        }
        return undefined
    }

    assignDealTypeConfiguration = async (fromDealTypeChanging?: boolean) => {
        if (!!this.deal.dealTypeId.value) {
            const response = await DealViewStore.fetchDealTypeConfiguration(this.deal, fromDealTypeChanging)
            if (!response) {
                return
            }
            this.dealTypeConfiguration = response
            await this.itemStore.gotDealTypeConfiguration(fromDealTypeChanging)
            this.workflowStore.gotDealTypeConfiguration(fromDealTypeChanging)
            return response
        }
        return
    }
    get windowTitle() {
        let dealDescription = (this.creation ? "New Deal" : `Deal ${this.deal && this.deal.dealNumber}`)
        let added = false

        const addWithSeparator = (s?: string) => {
            if (!s) {
                return
            }
            if (!added) {
                dealDescription += " - "
            } else {
                dealDescription += " | "
            }
            dealDescription += s
            added = true
        }

        addWithSeparator(this.infoStore.dealCategoryName)
        addWithSeparator(this.infoStore.dealTypeName)
        addWithSeparator(this.infoStore.counterpartyFieldStore.store?.inputValue || this.deal.counterpartyName)

        if (this.sp.execution) {
            return `Execution - ${dealDescription}`
        }
        return dealDescription
    }

    get dealCreationDate() {
        return this.deal.creationDate || moment().startOf('day')
    }
}
var dealViewStoreInstance = new DealViewStore({ visible: false })
export const DealViewStoreContext = createContext(dealViewStoreInstance)