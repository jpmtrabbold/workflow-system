import React, { createRef } from "react"
import deburr from 'lodash/deburr'

import { isRemoteDataSource, isPaginatedResults, hasToScrollUpTo, hasToScrollDownTo } from "./utils"
import { AutoCompleteFieldOption, AutoCompleteFieldProps } from "./AutoCompleteField"
import MenuItem from "@material-ui/core/MenuItem"

export class AutoCompleteFieldStore {
    constructor(source: AutoCompleteFieldProps) {
        this.source = source
        this.source.setStore && this.source.setStore(this)
    }
    source: AutoCompleteFieldProps

    inputRef = createRef<HTMLInputElement | null>()
    paperRef = createRef<HTMLDivElement | null>()
    selectedRef = createRef<HTMLDivElement | null>()
    lastPageQuery = ""
    totalCount = 0
    currentPage = 0
    timeout = undefined as (undefined | NodeJS.Timeout)

    resultsMessage = ""
    formControl = null as HTMLDivElement | null
    optionsPromise = undefined as Promise<{ options: AutoCompleteFieldOption[], resultsMessage: string, page?: number, newSelectedIndex?: number }> | undefined
    loading = false
    options = [] as AutoCompleteFieldOption[]
    optionsVisible = false
    selectedIndex = 0
    lookupViewVisible = false
    inputValue = ""
    get isMultiple() {
        return !!this.source.multipleSelectedValues
    }
    get loadingMenuItem() {
        if (this.loading) {
            return <MenuItem><i>Loading...</i></MenuItem>
        }

        if (this.source.pageSize !== undefined && this.options.length < this.totalCount && this.filteredOptions.length > 0) {
            return (
                <MenuItem onClick={this.nextPageFetch}>
                    <i>Load more...</i>
                </MenuItem>
            )
        }
        return null
    }
    get filteredOptions() {
        return this.filterOptions(this.options)
    }

    filterOptions = (options: AutoCompleteFieldOption[]) => {
        return options.filter(o => {
            const id = getAutoCompleteItemId(o)
            if (typeof (o.active) === 'boolean') {
                if (!(o.active || id === this.source.value)) {
                    return false
                }
            }
            if (this.isMultiple) {
                const multiple = this.source.multipleSelectedValues!
                if (!!multiple.find(m => {
                    return `${getAutoCompleteItemId(m)}`.trim() === `${id}`.trim()
                })) {
                    return false
                }
            }
            

            return true
        })
    }

    onEntityClose = () => {
        this.lookupViewVisible = false
    }
    onInputBlur = (event: React.FocusEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setTimeout(() => {
            if (this.source.onBlur) {
                this.source.onBlur()
            }
            this.clearInputValueWhenSuitable()
        }, 300);
    }
    entityViewClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        this.lookupViewVisible = true
        event.stopPropagation()
    }

    onDeleteChip = (chip: any, index: number) => {
        this.source.onValueRemove && this.source.onValueRemove(this.source.multipleSelectedValues![index])
    }

    valueClear = (all = false) => {
        if (all) {
            this.source.onValuesClear && this.source.onValuesClear()
        }
        this.source.onChange && this.source.onChange('')
    }

    valueAdd = (value: AutoCompleteFieldOption) => {
        this.source.onChange && this.source.onChange(getAutoCompleteItemId(value))
        this.source.onValueAdd && this.source.onValueAdd(value)
    }

    handleClear = (event?: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        this.valueClear(true)
        this.inputValue = ''
        this.showOptions(false)
        this.loading = false
        event?.preventDefault()
    }
    handleOpen = () => {
        this.inputRef.current!.focus()
        this.showOptions()
    }
    hasFocus = () => {
        return !!this.inputRef.current && this.inputRef.current === document.activeElement
    }
    fetchOptions = async (getNextPageParam: boolean = false) => {
        let getNextPage = getNextPageParam
        let resultsMessage = ""
        const hasPagination = this.source.pageSize !== undefined
        const inputValue = deburr(this.inputValue.trim()).toLowerCase()
        const inputLength = inputValue.length
        let count = 0
        let newSelectedIndex = 0
        const max = !!this.source.maxResults || 999

        let page: number = 0
        if (getNextPage) {
            page = this.currentPage + 1
        }

        let newOptions = [] as AutoCompleteFieldOption[]
        if (isRemoteDataSource(this.source.dataSource)) {
            try {
                var results = await this.source.dataSource({
                    query: this.inputValue,
                    pageNumber: (hasPagination ? page : undefined),
                    pageSize: (hasPagination ? this.source.pageSize : undefined)
                })

                if (isPaginatedResults(results)) {
                    newOptions = results.results
                    this.totalCount = results.totalCount!
                } else {
                    newOptions = results
                }
            } catch (error) {
                newOptions = []
                resultsMessage = `Error: ${error}`
            }
        } else {
            newOptions = this.source.dataSource
                .filter(option => {
                    const keep = count < max && option.name.toUpperCase().includes(inputValue.trim().toUpperCase())

                    if (keep) {
                        count += 1
                    }

                    return keep
                })
            if (inputLength !== 0) {

                newOptions = newOptions.sort((a, b) => {
                    const aFirst = (a.name.slice(0, inputLength).toLowerCase() === inputValue)
                    const bFirst = (b.name.slice(0, inputLength).toLowerCase() === inputValue)
                    return (aFirst && bFirst ? 0 : aFirst ? -1 : 1)
                })
            }
        }

        const options = (getNextPage ? [...this.options, ...newOptions] : newOptions)
        if (getNextPage && options.length > this.options.length) {
            newSelectedIndex = this.filteredOptions.length
        }

        if (this.filterOptions(options).length === 0 && !resultsMessage) {
            resultsMessage = "No results"
        }
        return { options, resultsMessage, page, newSelectedIndex }
    }
    resetSelection = () => {
        this.selectedIndex = 0
    }
    handleOptionsFetchRequested = async (getNextPageParam: boolean = false) => {
        let getNextPage = getNextPageParam
        if (getNextPage) {
            if (this.lastPageQuery !== this.inputValue) {
                getNextPage = false
            }
        }

        this.lastPageQuery = this.inputValue

        if (getNextPage) {
            if (this.totalCount <= this.options.length) {
                return
            }
        } else {
            this.currentPage = 0
            this.options = []
        }

        this.loading = true
        this.optionsPromise = undefined
        this.optionsPromise = this.fetchOptions(getNextPage)
        const options = await this.optionsPromise

        // only updates if that promise is still valid
        if (this.loading && !!this.optionsPromise) {
            this.options = options.options
            this.resultsMessage = options.resultsMessage
            this.currentPage = options.page || 0
            if (!getNextPage) {
                this.resetSelection()
            } else if (options.newSelectedIndex !== undefined) {
                this.selectedIndex = options.newSelectedIndex
            }
            this.loading = false
        }
        this.optionsPromise = undefined
    }
    onInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.inputValue = event.target.value
        this.valueClear()
        if (this.inputValue.length > 0) {
            this.showOptions()
        } else {
            this.showOptions(false)
        }
    }
    showOptions = (show: boolean = true) => {
        this.optionsVisible = show
        if (show) {
            this.debouncedFetch()
        } else {
            this.options = []
            this.currentPage = 0
            this.resultsMessage = ""
        }
    }
    debouncedFetch = async (getNextPage: boolean = false, withDebounce: boolean = true) => {
        if (this.timeout) {
            clearTimeout(this.timeout)
        }
        this.timeout = setTimeout(async () => {
            this.loading = true
            await this.handleOptionsFetchRequested(getNextPage)
            this.loading = false
            this.timeout = undefined
        }, (withDebounce ? this.source.debounceInMilliseconds || 200 : 0))
    }
    clearInputValueWhenSuitable = () => {
        if (!this.source.value && !this.hasFocus()) {
            if (!this.loading) {
                this.inputValue = ""
                this.showOptions(false)
            }
        }
    }
    addOption = (event: React.MouseEvent<HTMLDivElement> | React.KeyboardEvent<HTMLDivElement> | React.KeyboardEvent<HTMLLIElement>) => this.selectOption(event)
    selectOption = (event: React.MouseEvent<HTMLDivElement> | React.KeyboardEvent<HTMLDivElement> | React.KeyboardEvent<HTMLLIElement>, option?: AutoCompleteFieldOption) => {
        if (option) {
            this.valueAdd(option)
            if (!this.isMultiple) {
                this.inputValue = option.name
            } else {
                this.inputValue = ""
            }
            this.showOptions(false)
        } else {
            if (!!this.inputValue && this.isMultiple && !!this.source.canAdd && !this.source.multipleSelectedValues!.find(m => m.name === this.inputValue)) {
                this.valueAdd({ name: this.inputValue })
                this.inputValue = ""
                this.showOptions(false)
            }
        }
        event.stopPropagation()
    }
    onKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
        console.log(event.key)
        switch (event.key) {
            case "Backspace":
                if (!this.inputValue && this.isMultiple) {
                    const last = this.source.multipleSelectedValues!.length - 1
                    if (last >= 0) {
                        this.source.onValueRemove && this.source.onValueRemove(this.source.multipleSelectedValues![last])
                    }
                }
                break;
            case "Escape":
                this.handleClear()
                break;
            case "Enter":
                this.selectOption(event, this.filteredOptions[this.selectedIndex])
                break;
            case "ArrowDown":
                if (this.selectedIndex + 1 <= this.filteredOptions.length - 1) {
                    this.selectedIndex++
                } else if (this.source.pageSize !== undefined && this.options.length < this.totalCount) {
                    this.selectedIndex++
                    this.nextPageFetch()
                }
                break;
            case "ArrowUp":
                if (this.selectedIndex - 1 >= 0) {
                    this.selectedIndex--
                }
                break;
            case "PageDown":
                if (!this.paperRef.current) { break }
                this.paperRef.current!.scroll({ top: hasToScrollUpTo(this.selectedRef.current!, this.paperRef.current, false) })
                let nextSibling = this.selectedRef.current!.nextElementSibling
                let newNextSelectedIndex = this.selectedIndex
                while (!!nextSibling) {
                    newNextSelectedIndex++
                    if (hasToScrollDownTo(nextSibling as HTMLElement, this.paperRef.current) !== undefined) {
                        break
                    }
                    nextSibling = nextSibling.nextElementSibling
                }
                if (this.selectedIndex !== newNextSelectedIndex) {
                    this.selectedIndex = newNextSelectedIndex
                    if (this.selectedIndex + 1 > this.filteredOptions.length - 1) {
                        this.nextPageFetch()
                    }
                }
                break;
            case "PageUp":
                if (!this.paperRef.current) { break }
                this.paperRef.current!.scroll({ top: hasToScrollDownTo(this.selectedRef.current!, this.paperRef.current, false) })
                let prevSibling = this.selectedRef.current!.previousElementSibling
                let newPrevSelectedIndex = this.selectedIndex
                while (!!prevSibling) {
                    newPrevSelectedIndex--
                    if (hasToScrollUpTo(prevSibling as HTMLElement, this.paperRef.current) !== undefined) {
                        break
                    }
                    prevSibling = prevSibling.previousElementSibling
                }
                if (this.selectedIndex !== newPrevSelectedIndex) {
                    this.selectedIndex = newPrevSelectedIndex
                }
                break;
        }
    }
    setFormControl = (instance: HTMLDivElement | null) => this.formControl = instance
    nextPageFetch = (event?: React.MouseEvent<HTMLLIElement>) => {
        this.debouncedFetch(true, false)
        if (event) {
            event.stopPropagation()
            this.inputRef.current!.focus()
        }
    }
}

export const getAutoCompleteItemId = (item: AutoCompleteFieldOption) => item.id !== undefined ? item.id : item.name