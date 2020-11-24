import { createContext } from "react"
import { UserListDto, UsersListRequest, UsersListResponse } from "../../clients/deal-system-client-definitions"
import { userClient } from "../../clients/deal-system-rest-clients"
import { GridViewStore } from "shared-do-not-touch/grid-view"
import { userGridDefinition } from './UserGridDefinition'
import { UserGridViewProps } from "./UserGridView"

export default class UserGridViewStore {
    
    constructor(sp: UserGridViewProps) {
        this.gridStore = new GridViewStore({
            gridDefinition: userGridDefinition,
            listRequestType: UsersListRequest,
            listMethod: async (listRequest) => {
                if (sp.selectionOnly) {
                    listRequest.onlyActive = true
                }
                if (sp.onlyUsersWithLevel) {
                    listRequest.onlyUsersWithLevel = true
                }
                return await userClient.list(listRequest)
            },
            entitiesFromViewModel: (listResponse) => listResponse.users,
        })
        this.sp = sp
    }
    gridStore: GridViewStore<UserListDto, UsersListRequest, UsersListResponse>
    sp: UserGridViewProps
}

export const UserGridViewStoreContext = createContext(new UserGridViewStore({}))
