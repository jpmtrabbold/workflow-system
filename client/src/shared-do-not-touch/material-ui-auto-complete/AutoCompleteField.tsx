import React, { useRef, useEffect, useLayoutEffect } from "react"
import { observer } from "mobx-react-lite"

import { AutoCompleteFieldStore } from "./AutoCompleteFieldStore"
import InputLabel from "@material-ui/core/InputLabel"
import Input from "@material-ui/core/Input"
import InputAdornment from "@material-ui/core/InputAdornment"
import IconButton from "@material-ui/core/IconButton"
import Paper from "@material-ui/core/Paper"
import MenuItem from "@material-ui/core/MenuItem"
import ClearIcon from '@material-ui/icons/Clear'
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown'
import Popper from "@material-ui/core/Popper"
import FormControl from "@material-ui/core/FormControl"
import FormHelperText from "@material-ui/core/FormHelperText"
import PageviewIcon from "@material-ui/icons/Pageview"
import { makeStyles } from "@material-ui/core/styles"
import CircularProgress from "@material-ui/core/CircularProgress"
import useElementSize from "./useElementSize"
import useElementScroll from "./useElementScroll"
import { AutoCompleteFieldItem } from "./AutoCompleteFieldItem"
import { isLocalDataSource, hasToScrollUpTo, hasToScrollDownTo } from "./utils"
import { v1 } from 'uuid'
import useStore from "shared-do-not-touch/mobx-utils/useStore"

export default interface AutoCompleteFieldViewLoaderProps {
    onEntityClose: () => any
    entityId?: number
    entityName?: string
    visible: boolean
    readOnly?: boolean
}

export interface AutoCompleteFieldOption {
    id: number | string
    name: string
    description?: string
    active?: boolean
}

export interface AutoCompleteFieldPaginatedOptions {
    totalCount?: number
    results: AutoCompleteFieldOption[]
}

export type AutoCompleteFieldRemoteOptionsFunction = (lookupProps: { query: string, pageSize?: number, pageNumber?: number }) => Promise<AutoCompleteFieldOption[] | AutoCompleteFieldPaginatedOptions>;
export type LocalLookupData = AutoCompleteFieldOption[];

const useStyles = makeStyles(theme => ({
    popper: {
        zIndex: theme.zIndex.modal + 200,
        marginTop: theme.spacing(1),
    },
    paper: {
        maxHeight: theme.spacing(32),
    }
}))

export interface AutoCompleteFieldProps {
    label?: string
    placeholder?: string
    value?: any
    initialInputValue?: string
    onChange?: any
    onBlur?: () => any
    dataSource: AutoCompleteFieldRemoteOptionsFunction | LocalLookupData
    disabled?: boolean
    fullWidth?: boolean
    maxResults?: number
    required?: boolean
    error?: boolean
    helperText?: string
    autoFocus?: boolean
    entityView?(props: AutoCompleteFieldViewLoaderProps): React.ReactElement<any>
    pageSize?: number
    debounceInMilliseconds?: number
    maxHeight?: number
    style?: React.CSSProperties
    setStore?: (store: AutoCompleteFieldStore) => any
    className?: string
}

export const AutoCompleteField = observer((props: AutoCompleteFieldProps) => {
    const uuid = useRef(v1())
    
    const classes = useStyles()

    const store = useStore(source => new AutoCompleteFieldStore(source), props)

    const formControlSize = useElementSize(store.formControl)
    const scrollInfo = useElementScroll(store.paperRef.current)

    useEffect(() => {
        if (!!props.initialInputValue) {
            store.inputValue = props.initialInputValue
        } else {
            store.inputValue = ""
        }
    }, [props.initialInputValue, store])

    useEffect(() => {
        if (!props.initialInputValue) {
            if (props.value && isLocalDataSource(props.dataSource)) {
                const val = props.dataSource.find(item => item.id === props.value)
                store.inputValue = (val ? val.name : "")
            }
        }

    }, [props.initialInputValue, props.dataSource, store.inputValue, props.value])

    useEffect(() => {
        store.clearInputValueWhenSuitable()
    }, [props.value, store])

    useLayoutEffect(() => {
        const paper = store.paperRef.current
        const selectedElement = store.selectedRef.current
        if (selectedElement && paper) {
            const topUp = hasToScrollUpTo(selectedElement, paper)
            if (topUp !== undefined) {
                paper.scroll({ top: topUp })
            }
            const topDown = hasToScrollDownTo(selectedElement, paper)
            if (topDown !== undefined) {
                paper.scroll({ top: topDown })
            }
        }
    }, [store.selectedIndex, store.options, store])

    useEffect(() => {
        if (props.pageSize !== undefined && (scrollInfo.reachedBottom)) {
            store.nextPageFetch()
        }
    }, [scrollInfo.reachedBottom, props.pageSize, store])

    const localStore = useStore(source => ({
        get popper() {
            let paperStyle: React.CSSProperties = {
                width: source.formControlWidth,
                overflow: 'auto'
            }
    
            if (source.maxHeight !== undefined) {
                paperStyle.maxHeight = source.maxHeight
            }
            return (
                < Popper
                    className={classes.popper}
                    anchorEl={store.formControl}
                    placement='bottom-end'
                    open={store.optionsVisible}
                    style={{ width: source.formControlWidth }}
                >
                    <Paper
                        square
                        className={classes.paper}
                        ref={store.paperRef}
                        style={paperStyle}>
                        {store.filteredOptions.map((option, index) =>
                            <AutoCompleteFieldItem
                                key={index}
                                selectedIndex={store.selectedIndex}
                                index={index}
                                option={option}
                                inputValue={store.inputValue}
                                onClick={store.selectOption}
                                selectedRef={store.selectedRef}
                            />)}
                        {store.loadingMenuItem}
                        {store.resultsMessage && (
                            <MenuItem><i>{store.resultsMessage}</i></MenuItem>
                        )}
                    </Paper>
                </Popper >
            )
        }
    }), {...props, formControlWidth: formControlSize.width })
    
    return (
        <>
            <FormControl ref={store.setFormControl} fullWidth={props.fullWidth} style={props.style} className={props.className}>
                {!!props.label && (
                    <InputLabel disabled={props.disabled} required={props.required} disableAnimation={false} htmlFor={"autocomplete-textfield" + uuid}>{props.label}</InputLabel>
                )}
                <Input
                    onKeyDown={store.onKeyDown}
                    value={store.inputValue}
                    disabled={props.disabled}
                    onChange={store.onInputChange}
                    onBlur={store.onInputBlur}
                    inputRef={store.inputRef}
                    id={"autocomplete-textfield" + uuid}
                    startAdornment={!!props.entityView && !!props.value && (
                        <IconButton onClick={store.entityViewClick} size="small">
                            <PageviewIcon />
                        </IconButton>
                    )}
                    endAdornment={

                        <InputAdornment position="end">
                            {store.loading && (
                                <CircularProgress size={24} />
                            )}
                            {!props.disabled && (
                                (props.value || store.inputValue || store.optionsVisible)
                                    ?
                                    <IconButton aria-label="clear" onClick={store.handleClear}>
                                        <ClearIcon />
                                    </IconButton>
                                    :
                                    <IconButton aria-label="clear" onClick={store.handleOpen}>
                                        <ArrowDropDownIcon />
                                    </IconButton>
                            )}
                        </InputAdornment>
                    }
                />
                {(props.helperText ? <FormHelperText error={props.error}>{props.helperText}</FormHelperText> : null)}
            </FormControl>
            {!!store.lookupViewVisible && !!props.entityView && !!props.value &&
                props.entityView({ onEntityClose: store.onEntityClose, entityId: props.value, entityName: store.inputValue, readOnly: true, visible: true })
            }
            {localStore.popper}
        </>
    )
})
