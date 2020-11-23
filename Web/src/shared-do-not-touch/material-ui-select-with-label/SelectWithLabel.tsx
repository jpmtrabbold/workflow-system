import React from "react"
import { observer } from "mobx-react-lite"
import InputLabel from "@material-ui/core/InputLabel"
import Select from "@material-ui/core/Select"
import MenuItem from "@material-ui/core/MenuItem"
import FormControl from "@material-ui/core/FormControl"
import FormHelperText from "@material-ui/core/FormHelperText"
import Typography from "@material-ui/core/Typography"
import IconButton from "@material-ui/core/IconButton"
import PageviewIcon from "@material-ui/icons/Pageview"

import ViewLoaderProps from "../interfaces/ViewLoaderProps"

import { v1 } from 'uuid'
import useStore from "shared-do-not-touch/mobx-utils/useStore"

export interface SelectWithLabelProps {
    label?: string
    onChange?: (
        event: React.ChangeEvent<{ name?: string; value: unknown }>,
        child: React.ReactNode,
    ) => void
    value?: any
    lookups?: { id: number | string, name?: string, description?: string, active?: boolean }[]
    disabled?: boolean
    fullWidth?: boolean
    required?: boolean
    error?: boolean
    helperText?: string
    autoFocus?: boolean
    lookupView?(props: ViewLoaderProps): React.ReactElement<any>
    variant?: 'standard' | 'outlined' | 'filled'
    style?: React.CSSProperties
    labelStyle?: React.CSSProperties
    inputStyle?: React.CSSProperties
    className?: string
}

export const SelectWithLabel = observer((props: SelectWithLabelProps) => {
    const store = useStore(source => ({
        uuid: v1(),
        lookupViewVisible: false,
        entityId: 0,
        entityName: "",
        onEntityClose() {
            store.lookupViewVisible = false
        },
        onLookupViewClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
            store.entityId = source.value
            store.entityName = ""
            store.lookupViewVisible = true
            event.stopPropagation()
        },
        get menuItems() {
            return source.lookups?.filter(d => typeof (d.active) === 'boolean' ? (d.active || d.id.toString() === props.value.toString()) : true)
                .map(d => (
                    <MenuItem value={d.id} key={d.id} title={d.description}>
                        <Typography variant="inherit">{d.name || d.id}</Typography>
                    </MenuItem>
                ))
        }
    }), props)

    return (
        <>
            <FormControl fullWidth={props.fullWidth} error={props.error} style={props.style} className={props.className}>
                {!!props.label && (
                    <InputLabel disabled={props.disabled} required={props.required} htmlFor={store.uuid} style={props.labelStyle}>{props.label}</InputLabel>
                )}
                <Select
                    variant={props.variant}
                    required={props.required}
                    inputProps={{ id: store.uuid, style: props.inputStyle }}
                    onChange={props.onChange}
                    value={props.value}
                    disabled={props.disabled}
                    fullWidth={props.fullWidth}
                    autoFocus={props.autoFocus}
                    startAdornment={!!props.lookupView && !!props.value && (
                        <IconButton onClick={store.onLookupViewClick} size="small">
                            <PageviewIcon />
                        </IconButton>
                    )}
                >

                    {store.menuItems}
                </Select>
                {(props.helperText ? <FormHelperText error={props.error}>{props.helperText}</FormHelperText> : null)}

            </FormControl>
            {!!store.lookupViewVisible && !!props.lookupView && !!store.entityId &&
                props.lookupView({ onEntityClose: store.onEntityClose, entityId: store.entityId, entityName: store.entityName, readOnly: true, visible: true })
            }
        </>
    )
})