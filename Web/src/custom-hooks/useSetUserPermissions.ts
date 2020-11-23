import { useContext, useLayoutEffect } from "react";
import { GlobalStoreContext, FunctionalityHelper } from "../features/shared/stores/GlobalStore";
import { FunctionalityEnum, SubFunctionalityEnum } from "../clients/deal-system-client-definitions";
import { GridViewStore, GridViewListResponse, GridViewListRequest, GridViewSetActionsProps } from "shared-do-not-touch/grid-view/GridViewStore";

interface useSetUserPermissionsProps<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse> {
    gridViewStore: GridViewStore<ListType, ListRequestType, ListResponseType>,
    functionalityEnum: FunctionalityEnum,
    setActionsProps?: (actionsProps: GridViewSetActionsProps<ListType>, functionality: FunctionalityHelper) => void,
    selectionOnly?: boolean
}

export default function useSetUserPermissions<ListType extends Object, ListRequestType extends GridViewListRequest, ListResponseType extends GridViewListResponse>
    ({
        gridViewStore,
        functionalityEnum,
        setActionsProps = undefined,
        selectionOnly = false,
    }: useSetUserPermissionsProps<ListType, ListRequestType, ListResponseType>) {

    const globalStore = useContext(GlobalStoreContext)
    const { functionality } = globalStore

    useLayoutEffect(() => {
        const funcHelper = functionality(functionalityEnum)

        let actionsProps: GridViewSetActionsProps<ListType> = {
            hasCreateAction: !selectionOnly && funcHelper.hasSubFunctionality(SubFunctionalityEnum.Create),
            hasEditAction: !selectionOnly && funcHelper.hasSubFunctionality(SubFunctionalityEnum.Edit),
            hasViewAction: funcHelper.hasSubFunctionality(SubFunctionalityEnum.View),
        }

        if (setActionsProps) {
            setActionsProps(actionsProps, funcHelper)
        }

        gridViewStore.setGridActions(actionsProps)
        
    }, [functionalityEnum, setActionsProps, gridViewStore, functionality, selectionOnly])
}