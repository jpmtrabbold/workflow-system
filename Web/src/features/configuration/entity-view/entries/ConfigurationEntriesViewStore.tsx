import React from 'react'
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { ConfigurationGroupViewStore } from '../ConfigurationGroupViewStore'
import { ConfigurationEntryDto, ConfigurationEntryContentType, FunctionalityEnum, DealCategoriesListRequest, DealTypesListRequest, CounterpartiesListRequest, ProductsListRequest } from 'clients/deal-system-client-definitions'
import { observer } from 'mobx-react-lite'
import { InputProps } from 'shared-do-not-touch/input-props'
import { AutoCompleteField } from 'shared-do-not-touch/material-ui-auto-complete'
import TextField from '@material-ui/core/TextField'
import { YesNoSelectWithLabel } from 'shared-do-not-touch/material-ui-select-with-label/YesNoSelectWithLabel'
import { InputPropsConfig, InputPropsVariant } from 'shared-do-not-touch/input-props/field-props'
import { AutoCompleteFieldOption } from 'shared-do-not-touch/material-ui-auto-complete/AutoCompleteField'
import { dealCategoryClient, dealTypeClient, counterpartyClient, productClient } from 'clients/deal-system-rest-clients'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface ConfigurationEntriesViewStoreParams {
    rootStore: ConfigurationGroupViewStore
}

interface ConfigurationDataSource {
    functionality: FunctionalityEnum
    data?: AutoCompleteFieldOption[]
}
export class ConfigurationEntriesViewStore {
    constructor(sp: ConfigurationEntriesViewStoreParams) {
        this.sourceProps = sp
        
    }
    sourceProps: ConfigurationEntriesViewStoreParams

    fetchDataSources = async () => {
        const dataSources = [] as ConfigurationDataSource[]
        for (const setting of this.sourceProps.rootStore.entity.entries) {
            if (setting.functionalityForLookup) {
                let dataSource = dataSources.find(ds => ds.functionality === setting.functionalityForLookup)
                if (!dataSource) {
                    dataSource = { functionality: setting.functionalityForLookup }
                    switch (setting.functionalityForLookup) {
                        case FunctionalityEnum.DealCategories:
                            dataSource.data = (await dealCategoryClient.list({ sortField: 'name', sortOrderAscending: true } as DealCategoriesListRequest)).dealCategories
                            break;
                        case FunctionalityEnum.DealTypes:
                            dataSource.data = (await dealTypeClient.list({ sortField: 'name', sortOrderAscending: true } as DealTypesListRequest)).dealTypes
                            break;
                        case FunctionalityEnum.Counterparties:
                            dataSource.data = (await counterpartyClient.list({ sortField: 'name', sortOrderAscending: true } as CounterpartiesListRequest)).counterparties
                            break;
                        case FunctionalityEnum.Products:
                            dataSource.data = (await productClient.list({ sortField: 'name', sortOrderAscending: true } as ProductsListRequest)).products
                            break;
                    }
                }
                dataSource.data?.forEach(s => s.name = `${s.id} - ${s.name}`)
                dataSources.push(dataSource)
            }
        }
        this.dataSources = dataSources
    }
    dataSources?: ConfigurationDataSource[]

    get columns(): Column<ConfigurationEntryDto>[] {
        if (!this.dataSources) {
            return []
        }
        return [
            {
                field: 'name',
                title: 'Field Name',
                render: data => <b>{data.name}</b>
            },
            {
                title: 'Configuration Value',
                rendererComponent: observer(({ data }) => {
                    const localStore = useStore(() => ({
                        changed: () => {
                            data.updated = true
                        }
                    }))
                    let field = null as React.ReactElement | null
                    const config = {} as InputPropsConfig
                    let variant: InputPropsVariant = "string"

                    if (!!data.functionalityForLookup) {
                        const dataSource = this.dataSources!.find(ds => ds.functionality === data.functionalityForLookup)
                        if (!dataSource?.data) {
                            throw new Error(`Please fetch data for functionalityForLookup ${data.functionalityForLookup} on ConfigurationEntriesViewStore.fetchDataSources`)
                        }
                        field = <AutoCompleteField dataSource={dataSource!.data!} fullWidth />
                        config.elementValueModifiers = [
                            (value) => parseInt(value) as any
                        ]
                        config.stateModifiers = [
                            (value) => (value as number).toString()
                        ]

                    } else if (data.contentType === ConfigurationEntryContentType.BooleanType) {
                        config.isCheckbox = false
                        config.elementValueModifiers = [
                            (value) => (value === "True" ? true : false) as any
                        ]
                        config.stateModifiers = [
                            (value) => value ? "True" : "False"
                        ]

                        field = <YesNoSelectWithLabel fullWidth />
                    } else {
                        field = <TextField fullWidth />
                        if (data.contentType === ConfigurationEntryContentType.DecimalType) {
                            variant = 'numeric'
                        }
                        if (data.contentType === ConfigurationEntryContentType.IntType) {
                            variant = 'numericString'
                            config.maxDecimalPlaces = 0
                        }
                    }

                    if (!field) {
                        return null
                    }

                    return <>
                        <InputProps stateObject={data} propertyName='content' variant={variant} config={config} onValueChanged={localStore.changed}>
                            {field}
                        </InputProps >
                    </>
                })
            }
        ]
    }

}