import { AutoCompleteFieldOption, AutoCompleteFieldPaginatedOptions, AutoCompleteFieldRemoteOptionsFunction, LocalLookupData } from "./AutoCompleteField"

export const hasToScrollDownTo = (element?: HTMLElement, container?: HTMLElement, check: boolean = true) => {
    if (element && container) {
        const shouldBe = element.offsetTop + element.offsetHeight
        const current = container.offsetHeight + container.scrollTop
        if (!check || shouldBe > current) {
            return shouldBe - container.offsetHeight
        }
    }
    return undefined
}

export const hasToScrollUpTo = (element?: HTMLElement, container?: HTMLElement, check: boolean = true) => {
    if (element && container) {
        const current = container.offsetHeight + container.scrollTop
        if (!check || element.offsetTop <= current - container.offsetHeight) {
            return element.offsetTop
        }
    }
    return undefined
}

export const isPaginatedResults = (results: AutoCompleteFieldOption[] | AutoCompleteFieldPaginatedOptions): results is AutoCompleteFieldPaginatedOptions =>
    typeof results === 'object' && !!(results as AutoCompleteFieldPaginatedOptions).results

export const isRemoteDataSource = (lookupData: AutoCompleteFieldRemoteOptionsFunction | LocalLookupData): lookupData is AutoCompleteFieldRemoteOptionsFunction =>
    typeof lookupData === 'function'
    
export const isLocalDataSource = (lookupData: AutoCompleteFieldRemoteOptionsFunction | LocalLookupData): lookupData is LocalLookupData =>
    typeof lookupData !== 'function'

