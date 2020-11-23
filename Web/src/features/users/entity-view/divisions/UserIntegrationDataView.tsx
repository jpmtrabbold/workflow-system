import { observer } from "mobx-react-lite"
import React, { useContext } from 'react'
import { CustomTable } from "shared-do-not-touch/material-ui-table"
import { UserViewStoreContext } from "../UserViewStore"
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable"
import { UserIntegrationDataDto, IntegrationTypeEnum, UserIntegrationFieldEnum } from "clients/deal-system-client-definitions"
import { InputProps } from "shared-do-not-touch/input-props"
import { YesNoSelectWithLabel } from "shared-do-not-touch/material-ui-select-with-label"
import Input from "@material-ui/core/Input"
import { initializeUpdatable } from "shared-do-not-touch/utils/updatable"
import useStore from "shared-do-not-touch/mobx-utils/useStore"

export const UserIntegrationDataView = observer(() => {
    const store = useContext(UserViewStoreContext)

    const localStore = useStore(() => {

        // for now, there is only one field so it's been added automatically. 
        // When there is more, the whole logic should accommodate a + (plus) button so the user can
        // add parameters of different kinds and select the integration types and fields from dropdown lists
        if (store.user.integrationData.length === 0) {
            const intData = new UserIntegrationDataDto()
            intData.integrationType = IntegrationTypeEnum.EmsTradepoint
            intData.field = UserIntegrationFieldEnum.UserIdAtTheSource
            initializeUpdatable(intData.active, true)
            initializeUpdatable(intData.data, "")
            intData.updated = true

            store.user.integrationData.push(intData)
        }

        return {
            getIntegrationTypeDescription(type: IntegrationTypeEnum) {
                switch (type) {
                    case IntegrationTypeEnum.EmsTradepoint:
                        return "EMS"
                }
                return ""
            },
            getIntegrationFieldDescription(type: UserIntegrationFieldEnum) {
                switch (type) {
                    case UserIntegrationFieldEnum.UserIdAtTheSource:
                        return "User ID at source"
                }
                return ""
            },
            onEdit: (item: UserIntegrationDataDto) => {
                item.updated = true
            },
            get columns() {
                return [
                    {
                        title: 'Integration Type',
                        render: data => localStore.getIntegrationTypeDescription(data.integrationType),
                    },
                    {
                        title: 'Field',
                        render: data => localStore.getIntegrationFieldDescription(data.field),
                    },
                    {
                        title: 'Value',
                        render: data => (
                            <InputProps stateObject={data} propertyName='data' onValueChanged={() => localStore.onEdit(data)}>
                                <Input disabled={store.sp.readOnly} />
                            </InputProps>
                        ),
                    },
                    {
                        title: 'Active?',
                        render: data => (
                            <InputProps stateObject={data} propertyName='active' onValueChanged={() => localStore.onEdit(data)} config={{ isCheckbox: false }}>
                                <YesNoSelectWithLabel disabled={store.sp.readOnly} />
                            </InputProps>
                        ),
                    },
                ] as Column<UserIntegrationDataDto>[]
            }
        }
    })

    return (
        <CustomTable
            title={'Integration Data'}
            columns={localStore.columns}
            rows={store.user.integrationData}
            searchable={false}
            pagination={false}
        />
    )
})