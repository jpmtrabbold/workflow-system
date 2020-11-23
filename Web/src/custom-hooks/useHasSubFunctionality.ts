import { useContext, useMemo } from "react";
import { GlobalStoreContext } from "../features/shared/stores/GlobalStore";
import { FunctionalityEnum, SubFunctionalityEnum } from "../clients/deal-system-client-definitions";

export default function useHasSubFunctionality(functionalityEnum: FunctionalityEnum, subFunctionalityEnum: SubFunctionalityEnum) {

    const globalStore = useContext(GlobalStoreContext)
    const { functionality } = globalStore

    return useMemo(() => {
        const funcHelper = functionality(functionalityEnum)
        return funcHelper.hasSubFunctionality(subFunctionalityEnum)
    }, [functionalityEnum, subFunctionalityEnum, functionality])
}