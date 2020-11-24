import React from 'react'
import { observer } from "mobx-react-lite"
import Link from "@material-ui/core/Link"
import { ObjectViewer } from "features/shared/components/object-viewer/ObjectViewer"
import useStore from 'shared-do-not-touch/mobx-utils/useStore'

interface PayloadClickerProps {
    payload?: string
    linkDescription?: string
}
export const PayloadClicker = observer(({ payload, linkDescription = "View Payload" }: PayloadClickerProps) => {
    const store = useStore(() => ({
        viewPayload: false,
        onClose: () => {
            store.viewPayload = false
        },
        view: () => {
            store.viewPayload = true
        },
    }))
    if (!payload) {
        return null
    }

    return <>
        <Link onClick={() => store.view()} style={{ cursor: 'pointer' }}>
            {linkDescription}
        </Link>
        {!!store.viewPayload && (
            <ObjectViewer open={true} onClose={store.onClose} object={payload} title='Payload' />
        )}
    </>
})