import { isObservableObject, makeAutoObservable, observable, runInAction } from "mobx";
import { useEffect, useState } from "react";
import DisposableReactionsStore from "shared-do-not-touch/mobx-utils/DisposableReactionsStore";

export type useStoreInitializer<T> = () => T
export type useStoreWithPropsInitializer<T, P> = (sourceProps: P) => T
export type UseStoreReactionsConfigurator<T> = (store: T, disposableStore: DisposableReactionsStore) => any

/**
 * use this hook to instantiate a mobx store, whether it's class-based or a plain object
 * @param initializer method that should instantiate a new instance of a store class, that holds observables, actions and computed properties.
 * if the instance is not an observable object, useStore will execute `makeAutoObservable` on it
 */
export default function useStore<T extends object>(initializer: useStoreInitializer<T>): T;

/**
 * use this hook to instantiate a mobx store, whether it's class-based or a plain object. 
 * @param initializer method that should instantiate a new instance of a store class, that holds observables, actions and computed properties
 * if the instance is not an observable object, useStore will execute `makeAutoObservable` on it
 * @param sourceProps props that you would like to turn into a observable source, which will be passed inside a object as a parameter to the initializer. 
 * This object can be kept as a reference inside your store, and whenever the props change, those observables will change as well
 * @param reactionsConfigurator a function that will receive 2 parameters: 1 - 
 */
export default function useStore<T extends object, P extends object>(initializer: useStoreWithPropsInitializer<T, P>,
    sourceProps: P,
    reactionsConfigurator?: (store: T, disposableStore: DisposableReactionsStore) => any): T;

export default function useStore<T extends object, P extends object>(
    initializer: (useStoreInitializer<T> | useStoreWithPropsInitializer<T, P>),
    sourceProps?: P,
    reactionsConfigurator?: UseStoreReactionsConfigurator<T>): T {

    // instantiates the store that will hold the sourceProps as flat observables.
    const [observableSourceStore] = useState(() => {
        if (sourceProps) {
            return observable(sourceProps, undefined, { deep: false })
        }
    })

    // instantiates the store itself through the initializer
    const [store] = useState(() => {
        const st = ((initializer as any)(!!sourceProps ? observableSourceStore : undefined)) as T
        if (!isObservableObject(st)) {
            makeAutoObservable(st)
        }
        return st
    })

    // instantiates the disposable store, which will hold the reactions and be disposed on unmount
    const [disposableStore] = useState(() => {
        if (reactionsConfigurator) {
            const dt = new DisposableReactionsStore()
            reactionsConfigurator(store, dt)
            return dt
        }
        return undefined
    })

    // monitors any changes on the source props. If something changes, then our observable source is updated.
    useEffect(() => {
        if (sourceProps && observableSourceStore) {
            runInAction(() => {
                for (const key in sourceProps) {
                    if (observableSourceStore.hasOwnProperty(key)
                        && sourceProps.hasOwnProperty(key)
                        && observableSourceStore[key] !== sourceProps[key]) {

                        observableSourceStore[key] = sourceProps[key]
                        
                    }
                }
            })
        }
    }, [sourceProps, observableSourceStore])

    // disposes our disposable store on unmount
    useEffect(() => {
        return () => disposableStore?.dispose()
    }, [disposableStore])

    return store
}