import React from 'react'
import { makeAutoObservable } from "mobx"
import { messageError } from '../material-ui-modals'
import { isArray } from '../utils/utils'
import { Column, RemoteDataFetchProps, CustomTableActions, RowActionType } from '../material-ui-table/CustomTable'
import AddIcon from '@material-ui/icons/Add'

export type ListParams = {
    page: number
    pageSize?: number
    sortField: string | undefined
    sortDirection?: "asc" | "desc"
    searchString: string
}

type KeysOfType<T, TProp> = { [P in keyof T]: T[P] extends TProp ? P : never }[keyof T]

export type EntityActionType = 'create' | 'edit' | 'view' | any

export type EntityAction<ListType> = {
    name: string,
    actionType?: EntityActionType,
    callback?: (rowData: ListType) => any
}

export interface GridViewStoreProps<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse> {
    canEditEntity?: (rowData: ListType) => Promise<boolean>
    listRequestType: new () => ListRequestType
    listMethod: (listRequest: ListRequestType) => Promise<ListResponseType>
    entitiesFromViewModel: (listResponse: ListResponseType) => ListType[]
    editListRequest?: (listRequest: ListRequestType, params: ListParams) => any
    gridDefinition: Column<ListType>[]
}

export interface GridViewListRequest {
    pageSize?: number | undefined;
    pageNumber?: number | undefined;
    sortField: string;
    sortOrderAscending: boolean;
    searchString: string;
    onlyActive: boolean;
}

export interface GridViewListResponse {
    totalRecords?: number | undefined;
}

export interface GridViewSetActionsProps<ListType extends Object> {
    actions?: CustomTableActions<ListType>
    hasCreateAction: boolean
    hasEditAction: boolean
    hasViewAction: boolean
    additionalToolbarElements?: React.ReactElement[]
}

export class GridViewStore<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse> {

    constructor({
        canEditEntity = async () => true,
        listRequestType,
        listMethod,
        entitiesFromViewModel,
        editListRequest,
        gridDefinition: entityGridDefinition = [],
    }: GridViewStoreProps<ListType, ListRequestType, ListResponseType>) {

        this.canEditEntity = canEditEntity
        this.listRequestType = listRequestType
        this.listMethod = listMethod
        this.entitiesFromViewModel = entitiesFromViewModel
        this.editListRequest = editListRequest
        this.gridDefinition = [...entityGridDefinition]
        makeAutoObservable(this, {listRequestType: false})
    }

    canEditEntity?: (rowData: ListType) => Promise<boolean> = async () => true
    listRequestType: new () => ListRequestType
    listMethod: (listRequest: ListRequestType) => Promise<ListResponseType>
    entitiesFromViewModel: (listResponse: ListResponseType) => ListType[]
    editListRequest?: (listRequest: ListRequestType, params: ListParams) => any
    gridDefinition: Column<ListType>[]

    entityActionType?: EntityActionType
    entityActionActive = false
    selectedEntity?: ListType
    totalCount = 0
    rows = [] as (ListType)[]
    tableActions: CustomTableActions<ListType> = {}
    additionalToolbarElements?: React.ReactElement[]

    setGridActions = ({
        actions = { freeActions: [], rowActions: [], multipleRowActions: [] },
        hasCreateAction = false,
        hasEditAction = false,
        hasViewAction = false,
        additionalToolbarElements,
    }: GridViewSetActionsProps<ListType>) => {
        
        this.additionalToolbarElements = additionalToolbarElements
        
        this.tableActions = {
            freeActions: [],
            rowActions: [],
            multipleRowActions: [],
        }

        if (hasCreateAction) {
            this.tableActions.freeActions!.push({
                title: "Create",
                icon: () => <AddIcon />,
                callback: this.createEntity,
            })
        }


        if (hasEditAction) {
            this.tableActions.rowActions!.push({
                title: "Edit",
                callback: (data) => (!!data && !isArray(data) ? this.onActionClick("edit", data) : undefined),
            })
        }

        if (hasViewAction) {
            this.tableActions.rowActions!.push({
                title: "View",
                callback: (data) => (!!data && !isArray(data) ? this.onActionClick("view", data) : undefined),
            })
        }

        this.tableActions.freeActions = [...this.tableActions.freeActions, ...(actions.freeActions || [])]
        this.tableActions.rowActions = [...this.tableActions.rowActions, ...(actions.rowActions || [])]
        this.tableActions.multipleRowActions = [...this.tableActions.multipleRowActions, ...(actions.multipleRowActions || [])]
    }

    onActionClick = (actionNameOrRowAction: (string | RowActionType<ListType>), data: ListType) => {
        if (typeof (actionNameOrRowAction) === 'string') {
            this.entityActionType = actionNameOrRowAction
            this.showEntity(data)
        } else {
            actionNameOrRowAction.callback(data)
        }
    }

    setTotalCount = (total: number) => {
        this.totalCount = total
    }

    reloadFunc = () => undefined
    setReloadFunction = (func: () => any) => {
        this.reloadFunc = func
    }
    reloadTableData = () => {
        this.reloadFunc()
    }

    onEntityClose = () => {
        this.closeEntity()
        this.reloadTableData()
    }

    onEntityClick = (rowData: ListType) => {
        if (this.tableActions.rowActions) {
            for (const action of this.tableActions.rowActions) {
                let realAction: RowActionType<ListType> | undefined
                if (typeof(action) === 'function') {
                    realAction = action(rowData)
                } else {
                    realAction = action
                }
                if (realAction) {
                    this.onActionClick(realAction, rowData)
                    break
                }
            }
        }
    }

    createEntity = () => {
        this.entityActionType = 'create'
        this.showEntity()
    }

    closeEntity = () => {
        this.entityActionActive = false
        this.selectedEntity = undefined
    }

    showEntity = (rowData?: ListType) => {
        if (!!rowData) {
            this.openEntity(rowData)
        } else {
            this.newEntity()
        }
    }

    private openEntity = async (rowData: ListType) => {
        if (this.entityActionType === 'edit' && !!this.canEditEntity && !(await this.canEditEntity(rowData))) {
            return
        }

        this.selectedEntity = rowData
        this.entityActionActive = true
    }

    private newEntity = () => {
        this.selectedEntity = undefined
        this.entityActionActive = true
    }

    pageSize?: number
    fetchRequest = async (dataFetchProps: RemoteDataFetchProps<ListType>) => {
        this.pageSize = dataFetchProps.pageSize
        const result = await this.getList({
            pageSize: dataFetchProps.pageSize,
            page: dataFetchProps.page,
            sortField: dataFetchProps.sortField as string,
            sortDirection: dataFetchProps.sortDirection,
            searchString: dataFetchProps.searchString,
        })
        this.rows = result.data
        this.totalCount = result.totalCount
    }

    getList = async (listParams: ListParams) => {

        var tableData = { data: [] as ListType[], page: 0, totalCount: 1 }
        if (!this.listRequestType
            || !this.listMethod
            || !this.entitiesFromViewModel
        ) {
            throw new Error("Objects not correctly defined on GridViewStore")
        }
        let requestType = this.listRequestType
        let listRequest = new requestType()
        listRequest.pageNumber = listParams.page
        listRequest.pageSize = listParams.pageSize

        if (listParams.sortField) {
            listRequest.sortField = listParams.sortField
            listRequest.sortOrderAscending = (listParams.sortDirection === "asc")
        }
        listRequest.searchString = listParams.searchString

        if (!!this.editListRequest) {
            this.editListRequest(listRequest, listParams)
        }

        let viewModel: ListResponseType | undefined

        try {
            viewModel = await this.listMethod(listRequest)
        } catch (error) {
            return tableData
        }

        if (!!viewModel) {
            const entities = this.entitiesFromViewModel(viewModel)
            if (!!entities) {
                tableData.data = entities
                tableData.page = listParams.page
                tableData.totalCount = viewModel.totalRecords!
                this.setTotalCount(viewModel.totalRecords!)
                return tableData
            } else {
                messageError({
                    content: "The server didn't return an instantiated array of entities.", title: "Internal Error"
                })
            }
        } else {
            messageError({
                content: "The server didn't return a instantiated view model.", title: "Internal Error"
            })
        }

        return tableData
    }
}