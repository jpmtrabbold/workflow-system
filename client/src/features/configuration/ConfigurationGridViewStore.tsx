import { createContext } from "react"
import { UsersListRequest, ConfigurationGroupsListDto, ConfigurationGroupsListRequest, ConfigurationGroupsListResponse } from "../../clients/deal-system-client-definitions"
import { configurationClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { ConfigurationGridDefinition } from "./ConfigurationGridDefinition"

export default class ConfigurationGridViewStore {

    constructor() {
        this.gridStore = new GridViewStore({
            gridDefinition: ConfigurationGridDefinition,
            listRequestType: UsersListRequest,
            listMethod: (listRequest) => configurationClient.list(listRequest),
            entitiesFromViewModel: (listResponse) => listResponse.configurationGroups,
        })
    }
    gridStore: GridViewStore<ConfigurationGroupsListDto, ConfigurationGroupsListRequest, ConfigurationGroupsListResponse> 
}

export const UserGridViewStoreContext = createContext(new ConfigurationGridViewStore())
