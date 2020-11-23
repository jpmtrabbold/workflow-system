import React from "react"
import { observer } from "mobx-react-lite"
import Tooltip from "@material-ui/core/Tooltip"
import Typography, { TypographyTypeMap } from "@material-ui/core/Typography"

type TypographyProps = TypographyTypeMap["props"]
type Variant = TypographyProps['variant']

interface TextWithTooltipProps {
    text: string
    variant?: Variant
    tooltip: React.ReactNode | string
}

export const TextWithTooltip = observer(({tooltip, text, variant = 'body2'}: TextWithTooltipProps) => {
    return (
        <Tooltip title={tooltip ?? ""}>
            <Typography variant={variant}>
                {text}
            </Typography>
        </Tooltip>
    )
})