import { observer } from "mobx-react-lite"
import React, { useState, useMemo, useCallback } from 'react'
import { WorkflowActionReadDto } from "../../../../clients/deal-system-client-definitions"
import Button from "@material-ui/core/Button"
import Paper from "@material-ui/core/Paper"
import Popper from '@material-ui/core/Popper'
import ClickAwayListener from '@material-ui/core/ClickAwayListener'
import MenuItem from '@material-ui/core/MenuItem'
import MenuList from '@material-ui/core/MenuList'
import makeStyles from "@material-ui/core/styles/makeStyles"
import { htmlDashSeparator } from "shared-do-not-touch/utils/utils"

const useStyles = makeStyles(theme => ({
    popper: {
        zIndex: 9999999,
    },
}))

export type CarryOutActionType = (workflowAction: WorkflowActionReadDto, alternativeAction?: boolean) => Promise<void>

interface DealWorkflowActionButtonsProps {
    actions: WorkflowActionReadDto[]
    carryOutAction: CarryOutActionType
}

export const DealWorkflowActionButtons = observer(({ actions, carryOutAction }: DealWorkflowActionButtonsProps) => {
    const classes = useStyles()
    const anchorRef = React.useRef<HTMLButtonElement>(null)
    const [open, setOpen] = useState(false)

    const actionList = useMemo(() => {
        return actions.map((workflowAction, index) => {
            return <DealWorkflowActionButton key={index} action={workflowAction} carryOutAction={carryOutAction} setOpen={setOpen} />
        })
    }, [actions, carryOutAction])

    if (actions.length === 0) {
        return null
    }

    return (
        <>
            <Button
                ref={anchorRef}
                color="inherit"
                onClick={() => setOpen(true)} >

                Workflow Actions
            </Button>

            <Popper open={open} anchorEl={anchorRef.current} className={classes.popper}>
                <Paper>
                    <ClickAwayListener onClickAway={() => setOpen(false)}>
                        <MenuList>
                            {actionList}
                        </MenuList>
                    </ClickAwayListener>
                </Paper>

            </Popper>
        </>
    )
})

interface DealWorkflowActionButtonProps {
    action: WorkflowActionReadDto
    carryOutAction: CarryOutActionType
    setOpen: (open: boolean) => void
}

const DealWorkflowActionButton = observer(({ action, carryOutAction, setOpen }: DealWorkflowActionButtonProps) => {

    const onClick = useCallback(() => {
        carryOutAction(action)
        setOpen(false)
    }, [setOpen, action, carryOutAction])

    return (
        <MenuItem key={action.name} onClick={onClick}>
            <>
                <span style={{ fontWeight: 500 }}>{action.name}</span>{htmlDashSeparator + action.description}
            </>
        </MenuItem>
    )
})