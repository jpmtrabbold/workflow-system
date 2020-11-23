import React from "react"
import { observer } from "mobx-react-lite"
import { SelectWithLabelProps, SelectWithLabel } from "./SelectWithLabel"
import useStore from "shared-do-not-touch/mobx-utils/useStore"

interface YesNoSelectWithLabelProps extends Omit<SelectWithLabelProps, 'value' | 'lookups' | 'lookupView'> {
    value?: boolean
    hasNotSelectedOption?: boolean
    notSelectedOptionLabel?: string
}

export const YesNoSelectWithLabel = observer(({
    hasNotSelectedOption = false,
    notSelectedOptionLabel = "N/A",
    onChange,
    value,
    ...props
}: YesNoSelectWithLabelProps) => {
    const store = useStore(source => ({
        get lookups() {
            if (source.hasNotSelectedOption) {
                return [
                    { id: 2, name: "Yes" },
                    { id: 1, name: "No" },
                    { id: 0, name: source.notSelectedOptionLabel || "N/A" },
                ]
            }
            return [
                { id: 2, name: "Yes" },
                { id: 1, name: "No" },
            ]
        },
        get value() {
            if (typeof(source.value) === 'boolean') {
                return source.value ? 2 : 1
            } else {
                return 0
            }
        },
        onChange(event: React.ChangeEvent<{ name?: string; value: unknown }>, child: React.ReactNode) {
            const val = event.target.value as number
            event.target.value = (val === 0 ? undefined : val === 1 ? false : true)
            source.onChange && source.onChange(event, child)
        },
    }), { ...props, hasNotSelectedOption, notSelectedOptionLabel, onChange, value })

    return (
        <SelectWithLabel {...props} lookups={store.lookups} onChange={store.onChange} value={store.value} />
    )
})