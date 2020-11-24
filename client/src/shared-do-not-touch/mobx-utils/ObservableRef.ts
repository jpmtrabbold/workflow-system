import { makeAutoObservable } from "mobx"

/**
 * class to be used in order to be a drop in replacement for react's Ref, but with observability for when current actually receives a ref
 */
export default class ObservableRef<T> {
    constructor(initializer?: T) {
        this.current = initializer || null
        makeAutoObservable(this)
    }
    current: T | null
    set = (element: T) => {
        this.current = element
    }
}