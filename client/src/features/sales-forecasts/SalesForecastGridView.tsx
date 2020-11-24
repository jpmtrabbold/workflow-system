import React from 'react'
import { observer } from 'mobx-react-lite'
import SalesForecastGridViewStore from './SalesForecastGridViewStore'
import { GridView } from 'shared-do-not-touch/grid-view'
import SalesForecastViewLoader from './entity-view/SalesForecastViewLoader'
import useSetUserPermissions from '../../custom-hooks/useSetUserPermissions'
import { FunctionalityEnum, SubFunctionalityEnum } from '../../clients/deal-system-client-definitions'
import { AuditEntryGridView } from 'features/audit-entries/AuditEntriesView'
import CloudUploadIcon from '@material-ui/icons/CloudUpload'
import CloudDownloadIcon from '@material-ui/icons/CloudDownload'
import { SalesForecastUploadView } from './upload/SalesForecastUploadView'
import { SalesForecastUploadStore, SalesForecastUploadStoreContext } from './upload/SalesForecastUploadStore'
import { momentToMonthYearString } from 'features/shared/helpers/utils'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

const SalesForecastGridView = () => {

    const store = useStore(() => new SalesForecastGridViewStore())
    const uploadStore = useStore(store => new SalesForecastUploadStore(store), store)

    useSetUserPermissions({
        gridViewStore: store.gridStore,
        functionalityEnum: FunctionalityEnum.SalesForecasts,
        setActionsProps: (actionProps, functionality) => {
            actionProps.actions = { rowActions: [], freeActions: [] }
            if (functionality.hasSubFunctionality(SubFunctionalityEnum.AuditLogsView)) {
                actionProps.actions!.rowActions!.push({
                    title: 'Audit Log',
                    callback: (data) => store.gridStore.onActionClick("audit-log", data)
                })
            }

            if (functionality.hasSubFunctionality(SubFunctionalityEnum.Create) &&
                functionality.hasSubFunctionality(SubFunctionalityEnum.Edit)) {
                
                actionProps.actions?.freeActions?.push({
                    title: 'Download template for bulk upload',
                    callback: uploadStore.downloadTemplate,
                    icon: () => <CloudDownloadIcon />
                })
                actionProps.actions?.freeActions?.push({
                    title: 'Upload Sales Forecast from template',
                    callback: uploadStore.dropzoneModal.open,
                    icon: () => <CloudUploadIcon />
                })
                store.canAddForecast = true
            }
        }
    })

    return (
        <div onDragEnter={uploadStore.onDragEnter}>
            <GridView
                store={store.gridStore}
                title='Sales Forecasts'
                searchable={false}
            />

            {!!store.gridStore.entityActionActive && (
                store.gridStore.entityActionType === "audit-log"
                    ?
                    <AuditEntryGridView
                        onClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity!.id!}
                        entityName={momentToMonthYearString(store.gridStore.selectedEntity!.monthYear!)}
                        functionality={FunctionalityEnum.SalesForecasts}
                    />
                    :
                    <SalesForecastViewLoader
                        readOnly={store.gridStore.entityActionType === 'view'}
                        visible={store.gridStore.entityActionActive}
                        onEntityClose={store.gridStore.onEntityClose}
                        entityId={store.gridStore.selectedEntity && store.gridStore.selectedEntity!.id!}
                        entityName={store.gridStore.selectedEntity && momentToMonthYearString(store.gridStore.selectedEntity!.monthYear!)}
                    />
            )}
            
            <SalesForecastUploadStoreContext.Provider value={uploadStore}>
                <SalesForecastUploadView />
            </SalesForecastUploadStoreContext.Provider>
        </div>

    )
}
export default observer(SalesForecastGridView)