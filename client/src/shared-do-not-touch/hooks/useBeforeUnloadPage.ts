import { useCallback } from "react"
import useWindowEventListener from "./useWindowEventListener"

const useBeforeUnloadPage = (message: string, shouldPreventPageUnload?: () => boolean) => {
    const listener = useCallback((e: Event) => {
        if (!shouldPreventPageUnload || shouldPreventPageUnload()) {

            e.preventDefault();

            (e.returnValue as any) = message
            return message //Gecko + Webkit, Safari, Chrome etc.
        }
    }, [message, shouldPreventPageUnload])
    useWindowEventListener("beforeunload", listener)
}

export default useBeforeUnloadPage