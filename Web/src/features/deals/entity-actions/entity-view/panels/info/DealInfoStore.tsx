import { DealViewStore } from "../../DealViewStore"
import { makeAutoObservable } from "mobx"
import { dealClient } from "../../../../../../clients/deal-system-rest-clients"
import updatable, { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import { LookupRequest, CounterpartiesListRequest } from "../../../../../../clients/deal-system-client-definitions"
import { AutoCompleteFieldRemoteOptionsFunction } from "shared-do-not-touch/material-ui-auto-complete/AutoCompleteField"
import StoreHolder from "features/shared/helpers/StoreHolder"
import { AutoCompleteFieldStore } from "shared-do-not-touch/material-ui-auto-complete/AutoCompleteFieldStore"

export class DealInfoStore {

    constructor(rootStore: DealViewStore) {
        this.rootStore = rootStore
        makeAutoObservable(this)
    }

    rootStore: DealViewStore

    counterpartyFieldStore = new StoreHolder<AutoCompleteFieldStore>()
    
    dealTypeLookup: LookupRequest[] = [new LookupRequest({ name: "None", id: 0, description: "" })]
    get dealTypeName() {
        const dealTypeId = this.rootStore.deal.dealTypeId.value
        if (!dealTypeId) {
            return ""
        }
        const dealType = this.dealTypeLookup.find(item => item.id === dealTypeId)
        if (dealType) {
            return dealType.name
        }
        return ""
    }

    dealCategoryLookup: LookupRequest[] = [new LookupRequest({ name: "None", id: 0, description: "" })]
    get dealCategoryName() {
        const dealCategoryId = this.rootStore.deal.dealCategoryId.value
        if (!dealCategoryId) {
            return ""
        }
        const dealCategory = this.dealCategoryLookup.find(item => item.id === dealCategoryId)
        if (dealCategory) {
            return dealCategory.name
        }
        return ""
    }
    counterpartyTypeLookup: LookupRequest[] = [new LookupRequest({ name: "None", id: 0, description: "" })]

    setupNewDeal = () => {
        const { deal } = this.rootStore
        initializeUpdatable(deal.counterpartyId, 0)
        initializeUpdatable(deal.dealTypeId, 0)
        initializeUpdatable(deal.dealCategoryId, 0)
        initializeUpdatable(deal.forceMajeure, false)

        deal.counterpartyName = ""
        deal.userParticipatedOnThisDeal = true
    }

    dealTypeChangeValidation = async (newValue: number) => {
        if (!await this.rootStore.itemStore.existingItemsValidation(newValue)) {
            return false
        }
        return true
    }

    dealCategoryChangeValidation = async (newValue: number) => {
        if (!await this.rootStore.itemStore.existingItemsValidation(newValue)) {
            return false
        }
        return true
    }


    dealCategoryChanged = () => {
        this.fetchDealTypes()

        updatable(this.rootStore.deal.dealTypeId, 0)
        updatable(this.rootStore.deal.counterpartyId, 0)

        this.rootStore.deal.counterpartyName = ""
    }

    fetchDealCategories = async (): Promise<boolean> => {
        try {
            this.dealCategoryLookup = await dealClient.getDealCategories()
            if (this.rootStore.deal.dealCategoryId.value && this.rootStore.deal.dealCategoryId.value > 0) {
                if (!await this.fetchDealTypes()) {
                    return false
                }
            }
            return true
        } catch (error) {
            return false
        }
    }

    fetchDealTypes = async (): Promise<boolean> => {
        try {
            const ret = await dealClient.getDealTypes(this.rootStore.deal.dealCategoryId.value)
            this.dealTypeLookup = ret
            const dealTypes = this.dealTypeLookup.filter(dt => dt.active)
            if (dealTypes.length === 1)
                updatable(this.rootStore.deal.dealTypeId, dealTypes[0].id)

            return true
        } catch (error) {
            return false
        }
    }
    
    getCounterparties: AutoCompleteFieldRemoteOptionsFunction = async ({ query, pageSize, pageNumber }) => {
        return dealClient.getCounterparties(CounterpartiesListRequest.fromJS({
            pageSize,
            pageNumber,
            onlyActive: false,
            searchString: query,
            dealId: this.rootStore.deal.dealCategoryId.value,
            onlyNonExpiredAndApproved: false,
            sortField: 'name',
            sortOrderAscending: true
        }))
    }
}

