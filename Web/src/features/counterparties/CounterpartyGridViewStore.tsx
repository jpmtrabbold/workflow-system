import { createContext } from "react"
import { CounterpartiesListRequest, CounterpartyListDto, CounterpartiesListResponse } from "../../clients/deal-system-client-definitions"
import { counterpartyClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { counterpartyGridDefinition } from './CounterpartyGridDefinition'
import { History } from 'history'

interface CounterpartyGridViewStoreProps {
    history: History
    id: string | undefined
}
export default class CounterpartyGridViewStore {

    constructor(sp: CounterpartyGridViewStoreProps) {
        this.gridStore = new GridViewStore({
            gridDefinition: counterpartyGridDefinition,
            listRequestType: CounterpartiesListRequest,
            listMethod: (listRequest) => counterpartyClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => {
                this.processSpecific(listResponse.counterparties)
                return listResponse.counterparties
            },
            editListRequest: (listRequest, params) => {
                if (this.isSpecific) {
                    listRequest.id = parseInt(this.sourceProps.id || "")
                }
            },

        })
        this.sourceProps = sp
    }
    gridStore: GridViewStore<CounterpartyListDto, CounterpartiesListRequest, CounterpartiesListResponse>

    sourceProps: CounterpartyGridViewStoreProps

    specificCounterpartyName = ""

    get specificCounterpartyIdentifier() {
        return this.specificCounterpartyName || this.sourceProps.id
    }
    get isSpecific() {
        return !!this.sourceProps.id
    }
    removeSpecific = () => {
        const arr = this.sourceProps.history.location.pathname.split('/')
        arr.pop()
        this.sourceProps.history.replace(arr.join('/'))
    }
    processSpecific(entities: CounterpartyListDto[]) {
        if (this.isSpecific) {
            if (entities.length === 1) {
                const row = entities[0]
                this.specificCounterpartyName = row.name
            }
        }
    }

}

export const CounterpartyGridViewStoreContext = createContext(new CounterpartyGridViewStore({ id: "", history: {} as History }))
