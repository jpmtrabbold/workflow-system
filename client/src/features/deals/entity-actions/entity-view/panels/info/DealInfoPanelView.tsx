// ** Common
import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import Grid from '@material-ui/core/Grid'
import { GridSize } from '@material-ui/core/Grid'
import { DealViewStoreContext } from '../../DealViewStore'
import DealCategoryViewLoader from 'features/deal-categories/entity-view/DealCategoryViewLoader'
import { InputProps } from 'shared-do-not-touch/input-props'
import { KeyboardDatePicker } from '@material-ui/pickers/DatePicker'
import { SelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label'
import { AutoCompleteField } from 'shared-do-not-touch/material-ui-auto-complete'
import { CheckBoxWithLabel } from 'shared-do-not-touch/material-ui-checkbox-with-label'
import CounterpartyView from 'features/counterparties/entity-view/CounterpartyView'
import { DealDelegatedAuthority } from './delegated-authority/DealDelegatedAuthority'
import DealTypeViewLoader from 'features/deal-types/entity-view/DealTypeViewLoader'

const nullChange = () => null

const DealInfoPanelView = () => {

    const colPropsSm = { xxl: 1 as GridSize, xl: 2 as GridSize, md: 2 as GridSize, sm: 3 as GridSize, xs: 6 as GridSize }
    const colPropsMd = { xxl: 1 as GridSize, xl: 2 as GridSize, md: 3 as GridSize, sm: 6 as GridSize, xs: 12 as GridSize }
    const colPropsLg = { xxl: 1 as GridSize, xl: 3 as GridSize, md: 4 as GridSize, sm: 8 as GridSize, xs: 12 as GridSize }

    const rootStore = useContext(DealViewStoreContext)
    const store = rootStore.infoStore

    const { deal } = rootStore

    return (
        <>
            <Grid container spacing={3}>
                <Grid item {...colPropsMd}>
                    <InputProps stateObject={deal} propertyName="dealCategoryId" onValueChange={store.dealCategoryChangeValidation} >
                        <SelectWithLabel
                            required
                            disabled={!rootStore.creation || !rootStore.canEditDealBasicInfo}
                            label='Deal'
                            lookups={store.dealCategoryLookup}
                            fullWidth
                            lookupView={(props) => (
                                <DealCategoryViewLoader {...props} />
                            )}
                        />
                    </InputProps>
                </Grid>
                <Grid item {...colPropsMd}>
                    <InputProps stateObject={deal} propertyName="dealTypeId" onValueChange={store.dealTypeChangeValidation} >
                        <SelectWithLabel
                            required
                            disabled={!deal.dealCategoryId.value || !rootStore.canEditDealBasicInfo}
                            label='Deal Type'
                            lookups={store.dealTypeLookup}
                            fullWidth
                            lookupView={(props) => (
                                <DealTypeViewLoader {...props} />
                            )}
                        />
                    </InputProps>
                </Grid>
                <Grid item {...colPropsLg}>
                    <InputProps stateObject={deal} propertyName="counterpartyId">
                        <AutoCompleteField
                            pageSize={40}
                            required
                            disabled={!deal.dealCategoryId.value || !rootStore.creation || !rootStore.canEditDealBasicInfo}
                            label='Counterparty'
                            dataSource={store.getCounterparties}
                            setStore={store.counterpartyFieldStore.set}
                            initialInputValue={deal.counterpartyName}
                            fullWidth
                            entityView={(props) => (
                                <CounterpartyView {...props} />
                            )}
                        />
                    </InputProps>
                </Grid>
                <Grid item {...colPropsSm}>
                    <InputProps stateObject={deal} propertyName="forceMajeure">
                        <CheckBoxWithLabel
                            label="Force Majeure?"
                            disabled={!rootStore.canEditDealBasicInfo}
                        />
                    </InputProps>
                </Grid>
                {rootStore.dealTypeConfiguration && rootStore.dealTypeConfiguration.hasExpiryDate && (
                    <Grid item {...colPropsMd}>
                        <InputProps stateObject={deal} propertyName="expiryDate" config={{ elementValueForUndefinedOrNull: () => null }} >
                            <KeyboardDatePicker
                                disabled={!rootStore.canEditDealBasicInfo}
                                autoOk
                                format="DD/MM/YYYY"
                                label='Expiry Date'
                                fullWidth value={null} onChange={nullChange} />

                        </InputProps>
                    </Grid>
                )}
                {!!rootStore.dealTypeConfiguration?.hasDelegatedAuthority && (
                    <Grid item {...colPropsLg}>
                        <DealDelegatedAuthority />
                    </Grid>
                )}
            </Grid>
        </>
    )
}

export default observer(DealInfoPanelView)