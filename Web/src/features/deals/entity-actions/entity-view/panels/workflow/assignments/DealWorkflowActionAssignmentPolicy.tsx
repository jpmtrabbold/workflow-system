// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { DealViewStoreContext } from '../../../DealViewStore'
import { MessageDialog } from 'shared-do-not-touch/material-ui-modals'
import { CustomTable } from 'shared-do-not-touch/material-ui-table'
import Tabs from '@material-ui/core/Tabs'
import Tab from '@material-ui/core/Tab'

export const DealWorkflowActionAssignmentPolicy = observer(() => {
    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.workflowStore

    if (!store.assignmentShowingTradingPolicy) {
        return null
    }

    const assesments = (store.assignmentShowingTradingPolicy?.assessments ?? [])
    const assesmentRows = assesments.length > 0 ? (assesments[store.assessmentBeingShown].assessmentRows ?? []) : []
    return (
        <>
            <MessageDialog
                fullWidth
                maxWidth='sm'
                scroll='paper'
                title="Policy Criteria x Deal Values"
                actions={[{
                    name: "Close",
                    color: "primary",
                    callback: store.closeTradingPolicyForAssignment,
                }]}
                onClose={store.closeTradingPolicyForAssignment}
                open={true}
            >
                {assesments.length > 1 && (
                    <Tabs
                        value={store.assessmentBeingShown}
                        indicatorColor="primary"
                        textColor="primary"
                        onChange={store.assessmentBeingShownChange}
                    >
                        {assesments.map((item, index) => (
                            <Tab key={index} label={`Policy ${index + 1}`} />
                        ))}
                    </Tabs>
                )}

                {assesments.length === 0 ? (<>No policies apply to this role in this deal.</>) :
                    assesmentRows.length === 0 ? (<>This role has unlimited policy powers.</>) : (
                        <CustomTable
                            rows={assesmentRows}
                            columns={store.policyGridDefinition}
                            searchable={false} pagination={false}
                            overrides={{ toolbar: () => null }}
                        />
                    )}

            </MessageDialog>
        </>
    )
})