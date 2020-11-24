import { CounterpartyDto, LookupRequest } from "../../../clients/deal-system-client-definitions"
import { counterpartyClient, dealClient } from "../../../clients/deal-system-rest-clients"
import { messageNoYes, snackbar } from "shared-do-not-touch/material-ui-modals"
import ViewLoaderProps from "shared-do-not-touch/interfaces/ViewLoaderProps"
import updatable, { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import FormErrorHandler from "shared-do-not-touch/input-props/form-error-handler"
import { executeLoading } from "shared-do-not-touch/material-ui-modals/execute-loading"

export class CounterpartyViewStore {
    constructor(sourceProps: ViewLoaderProps) {
        this.sourceProps = sourceProps
        
    }

    sourceProps: ViewLoaderProps
    entity = new CounterpartyDto()
    errorHandler = new FormErrorHandler<CounterpartyDto>()
    loaded = false
    get creation() {
        return !this.sourceProps.entityId
    }

    onLoad = async () => {
        this.loaded = await (this.creation ? this.setupNewEntity() : this.fetchData())
        if (!this.loaded) {
            this.sourceProps.onEntityClose()
        }
    }

    async setupNewEntity() {
        this.entity = new CounterpartyDto()

        initializeUpdatable(this.entity.name, '')
        initializeUpdatable(this.entity.code, '')
        initializeUpdatable(this.entity.active, false)

        if (!await this.fetchSupportingData())
            return false

        return true
    }

    fetchData = async () => {
        const p1 = this.fetchCounterparty(this.sourceProps.entityId!)
        const p2 = this.fetchSupportingData()

        if (!await p1 || !await p2)
            return false

        this.loaded = true
        return true
    }

    saveCounterparty = async () => {
        return await executeLoading("Saving Counterparty...", async () => {
            let success = true
            try {
                if (!(await this.validation())) {
                    return false
                }
                const result = await counterpartyClient.post(this.entity)
                snackbar({ title: `Counterparty ${result.counterpartyName} saved successfully` })
            } catch {
                success = false
            }
            return success
        })
    }

    validation = async () => {
        this.errorHandler.reset()

        if (this.entity.exposureLimit.value > 0) {
            this.errorHandler.error('exposureLimit', "Can't be positive")
            return false
        }

        if (!await this.checkCodeUsedInDeals()) {
            return false
        }

        if (!await this.duplicateCodeValidation()) {
            return false
        }
        return true
    }


    handleClose = () => {
        this.sourceProps.onEntityClose()
    }

    saveAndClose = async () => {
        if (await this.saveCounterparty()) {
            this.handleClose()
        }
    }

    fetchSupportingData = async () => {
        const p1 = this.fetchDealCategories()
        const p2 = this.fetchCountries()

        if (!await p1 || !await p2)
            return false

        return true
    }

    fetchCounterparty = async (counterpartyId: number): Promise<boolean> => {
        try {
            this.entity = await counterpartyClient.get(counterpartyId)
            this.counterpartyOriginalCode = this.entity.code.value
            return true
        } catch {
            return false
        }
    }

    fetchDealCategories = async () => {
        try {
            this.dealCategories = await dealClient.getDealCategories()
        } catch (error) {
            return false
        }
        return true
    }

    fetchCountries = async () => {
        try {
            this.countries = await counterpartyClient.getCountries()
        } catch (error) {
            return false
        }
        return true
    }

    counterpartyOriginalCode = ''
    dealCategories: LookupRequest[] = []
    countries: LookupRequest[] = []

    onNzemChange = async (nzemParticipant?: boolean) => {
        if (!nzemParticipant) {
            updatable(this.entity.nzemParticipantId, "")
        }
    }

    get NzemIdDisabled() {
        return !this.entity || !this.entity.nzemParticipant.value
    }

    toggleDealCategorySelection = (dealCategory: LookupRequest) => {
        const index = this.entity.dealCategories.findIndex(pt => pt === dealCategory.id)
        if (index >= 0) {
            this.entity.dealCategories.splice(index, 1)
        } else {
            this.entity.dealCategories.push(dealCategory.id)
        }
    }

    spaceAndNumbersOnly = (value: string) => {
        if (value.match(/^[\d ]*$/)) {
            return true
        }
        return false
    }

    checkCodeUsedInDeals = async () => {
        if (!this.entity.code.updated) {
            return true
        }
        if (await counterpartyClient.checkCodeUsedInDeals(this.entity.id!)) {
            const confirmed = await messageNoYes({
                content: `The code '${this.counterpartyOriginalCode}' has already been used on deals. 
                Newer deals will have their Deal Number based on the new code, 
                but the older deals will stay as they are. Do you wish to continue?`, title: 'Counterparty Code used in existing deals'
            })

            if (!confirmed) {
                return false
            }
        }
        return true
    }

    duplicateCodeValidation = async () => {
        if (await counterpartyClient.checkForDuplicateCodes(this.entity.id!, this.entity.code.value) && this.entity.code.updated) {
            const confirmed = await messageNoYes({
                content: `The code '${this.entity.code.value}' is already being used in another Counterparty.  
                Are you sure you want to use the same code?`, title: 'Counterparty Code used in another Counterparty'
            })

            if (!confirmed) {
                return false
            }
        }
        return true
    }
}