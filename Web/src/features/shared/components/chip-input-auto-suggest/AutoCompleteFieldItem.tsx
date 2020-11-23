import React, { useCallback } from "react"
import match from 'autosuggest-highlight/match'
import parse from 'autosuggest-highlight/parse'
import MenuItem from "@material-ui/core/MenuItem"
import { AutoCompleteFieldOption } from "./AutoCompleteField"

interface AutoCompleteFieldItemProps {
    selectedIndex: number
    index: number
    option: AutoCompleteFieldOption
    inputValue: string
    onClick: (event: React.MouseEvent<HTMLDivElement>, option: AutoCompleteFieldOption) => any
    selectedRef: React.MutableRefObject<HTMLDivElement | null>
}

export const AutoCompleteFieldItem = ({
    selectedIndex,
    index,
    option,
    inputValue,
    onClick,
    selectedRef,
}: AutoCompleteFieldItemProps) => {

    const isSelected = selectedIndex === index
    const matches = match(option.name, inputValue)
    const parts = parse(option.name, matches)
    const setRef = useCallback((instance: HTMLDivElement | null) => (isSelected && instance ? selectedRef.current = instance : null), [isSelected, selectedRef])
    const onOptionClick = useCallback((event) => onClick(event, option), [onClick, option])

    return (
        <MenuItem ref={setRef} selected={isSelected} component="div" onClick={onOptionClick}>
            <div>
                {parts.map(part => (
                    <span key={part.text} style={{ fontWeight: part.highlight ? 500 : 400 }}>
                        {part.text}
                    </span>
                ))}
            </div>
        </MenuItem>
    )
}