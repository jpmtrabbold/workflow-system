import React from 'react'
import { DealViewStore, DealViewStoreContext } from './DealViewStore'
import { observer } from 'mobx-react-lite'
import useInitialMount from 'shared-do-not-touch/hooks/useInitialMount'
import Dialog from '@material-ui/core/Dialog'
import CloseIcon from '@material-ui/icons/Close'
import DealWorkflowActionView from './panels/workflow/action/DealWorkflowActionTasksDialog'
import useBeforeUnloadPage from 'shared-do-not-touch/hooks/useBeforeUnloadPage'
import { LoadingModal } from 'shared-do-not-touch/material-ui-modals'
import { AppBarContainer } from 'shared-do-not-touch/material-ui-app-bar-container'
import { DealViewPanels } from './panels/DealViewPanels'
import DealWorkflowActionAssignmentDialog from './panels/workflow/action/DealWorkflowActionAssignmentDialog'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

export interface DealViewProps {
    dealId?: number
    dealNumber?: string
    onEntityClose?: () => any
    readOnly?: boolean
    execution?: boolean
    visible: boolean
}

const DealView = (props: DealViewProps) => {
    const store = useStore(sp => new DealViewStore(sp), props, (store, disposable) => store.disposable = disposable)
    
    useBeforeUnloadPage("Are you sure you want to close this page? Any unsaved changes will be lost.", store.shouldPreventUnloadPage)

    useInitialMount(() => {
        store.onLoad()
    })

    if (!store.loaded)
        return <LoadingModal title={(store.creation ? 'Loading new deal...' : `Loading${(store.sp.dealNumber ? ' deal ' + store.sp.dealNumber : '...')}`)} visible={props.visible} />

    return (
        <Dialog
            fullScreen
            onClose={store.handleClose}
            open={props.visible}
            disableEscapeKeyDown={true}
        >
            <AppBarContainer
                title={store.windowTitle}
                leftButtonOnClick={store.handleClose}
                leftButtonIcon={<CloseIcon />}
                rightButtons={store.mainButtons}
            >
                <DealViewStoreContext.Provider value={store}>
                    <DealViewPanels key={1}/>
                    <DealWorkflowActionView key={2} />
                    <DealWorkflowActionAssignmentDialog key={3} />
                </DealViewStoreContext.Provider>                
            </AppBarContainer>
        </Dialog >
    )
}

export default observer(DealView)