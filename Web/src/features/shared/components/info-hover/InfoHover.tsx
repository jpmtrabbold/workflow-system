import React from 'react'
import { observer } from "mobx-react-lite"
import Link from "@material-ui/core/Link"
import Tooltip from '@material-ui/core/Tooltip/Tooltip'
import { messageInfo } from 'shared-do-not-touch/material-ui-modals/message-info'
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface InfoHoverProps {
    title?: string | number
    tooltip?: string | number
    onClick?: () => any
}
export const InfoHover = observer(({ title, tooltip, onClick }: InfoHoverProps) => {
    const store = useStore(sp => ({
        view: () => {
            if (sp.onClick) {
                sp.onClick()
            } else {
                messageInfo({ content: `${tooltip}` })
            }
        },
    }), { onClick })
    if (!title) {
        return null
    }
    const link = (
        <Link onClick={store.view} style={{ cursor: 'pointer' }}>
            {title}
        </Link>
    )

    if (tooltip) {
        return (
            <Tooltip title={tooltip}>
                {link}
            </Tooltip>
        )
    } else {
        return link
    }
})