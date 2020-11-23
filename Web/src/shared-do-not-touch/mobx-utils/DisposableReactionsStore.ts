import { autorun, IReactionPublic, reaction } from "mobx"
import { deferAction } from "../utils/utils"

type Disposer = () => any

class DummyReaction implements IReactionPublic {
    dispose(): void {
        throw new Error("Please do not use the disposing reaction with DisposableReactionsStore, as the disposers will be called automatically.")
    }
    trace(enterBreakPoint?: boolean | undefined): void {
        throw new Error("Please do not use 'trace' with DisposableReactionsStore, as the disposers will be called automatically.")
    }
}
const dummyReaction = new DummyReaction()

/** a store that `useStore` uses so you can register autoruns and reactions on your store, and useStore will dispose them automatically on unmount */
export default class DisposableReactionsStore {
    disposers = [] as Disposer[]
    dispose = () => {
        for (const disposer of this.disposers) {
            disposer()
        }
    }
    registerDisposer(disposer: Disposer) {
        this.disposers.push(disposer)
    }

    registerReaction = (...params: Parameters<typeof reaction>) => {
        this.registerDisposer(reaction(...params))
    }
    registerReactionAndRunImmediately = (...params: Parameters<typeof reaction>) => {
        deferAction(() => params[1](params[0](dummyReaction), dummyReaction, dummyReaction))
        this.registerDisposer(reaction(...params))
    }

    registerAutorun = (...params: Parameters<typeof autorun>) => {
        this.registerDisposer(autorun(...params))
    }

    registerAutorunAndRunImmediately = (...params: Parameters<typeof autorun>) => {
        deferAction(() => params[0](dummyReaction))
        this.registerDisposer(autorun(...params))
    }

}