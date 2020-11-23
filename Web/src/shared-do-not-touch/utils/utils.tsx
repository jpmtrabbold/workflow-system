import { action } from "mobx"

export const htmlSpace = '\u00A0'
export const htmlDash = '\u2013'
export const htmlDashSeparator = htmlSpace + htmlDash + htmlSpace

export function cloneObject<T extends object>(obj: T) {
    return JSON.parse(JSON.stringify(obj)) as T
}

export function removeItemFromArray<T>(array: T[], item: T) {
    const index = array.indexOf(item)
    if (index >= 0) {
        array.splice(index, 1)
    }
}

export function removeFromArray<T>(array: T[], predicate: (item: T) => boolean ) {
    const index = array.findIndex(predicate)
    if (index >= 0) {
        array.splice(index, 1)
        return true
    }
    return false
}

export function isArray(v: any): v is Array<any> {
    // works for both simple arrays and mobx observables as well
    return !!v && !!v.map && !!v.slice && !!v.splice
}

export const deferAction = (x: () => any, ms?: number) => setTimeout(action(x), ms)