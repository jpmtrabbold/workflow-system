import React, { useContext, useLayoutEffect } from 'react'
import { observer } from 'mobx-react-lite'
import { ConfigurationGroupViewStoreContext } from '../ConfigurationGroupViewStore'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import { ConfigurationEntriesViewStore } from './ConfigurationEntriesViewStore'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export const ConfigurationEntriesView = observer(() => {
    const rootStore = useContext(ConfigurationGroupViewStoreContext)
    const store = useStore(sp => new ConfigurationEntriesViewStore(sp), { rootStore })

    useLayoutEffect(() => {
        store.fetchDataSources()
    }, [store])
    
    return (
        <CustomTable
            title='Settings'
            rows={rootStore.entity.entries}
            columns={store.columns}
            pagination={false}
            forceIsLoading={store.columns.length === 0}
        />
    )
})