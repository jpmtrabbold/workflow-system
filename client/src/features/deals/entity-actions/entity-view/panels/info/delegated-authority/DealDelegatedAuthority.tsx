import React, { useContext } from 'react'
import TextField from "@material-ui/core/TextField"
import { observer } from "mobx-react-lite"
import { DealViewStoreContext } from '../../../DealViewStore'
import DealDelegatedAuthorityStore from './DealDelegatedAuthorityStore'
import UserView from 'features/users/entity-view/UserView'
import { UserGridViewForSelection } from 'features/users/UserGridViewForSelection'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const DealDelegatedAuthority = observer(() => {
    const rootStore = useContext(DealViewStoreContext)
    const store = useStore(sp => new DealDelegatedAuthorityStore(sp), { rootStore })

    return <>
        <TextField
            fullWidth
            label="Delegated Authority User"
            value={rootStore.deal.delegatedAuthorityUserName || "Not Defined"}
            disabled
            InputProps={{
                endAdornment: store.endAdornment,
                startAdornment: store.startAdornment,
            }}
        />
        {store.showingUser.visible && (
            <UserView
                readOnly={true}
                visible={true}
                onEntityClose={store.showingUser.close}
                entityId={store.sp.rootStore.deal.delegatedAuthorityUserId.value}
                entityName={store.sp.rootStore.deal.delegatedAuthorityUserName}
            />
        )}
        {store.selectingUser.visible && (
            <UserGridViewForSelection
                selectionCallback={store.userSelected}
                title='Please select the user that delegated authority to the trader:'
                onlyUsersWithLevel
            />
        )}
    </>
})