import React, { useState, useRef, useCallback, useMemo } from 'react'
import { observer } from "mobx-react-lite"
import Button from '@material-ui/core/Button'
import ButtonGroup from '@material-ui/core/ButtonGroup'
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown'
import MenuItem from '@material-ui/core/MenuItem'
import { makeStyles } from "@material-ui/core/styles"
import Menu from '@material-ui/core/Menu'
import clsx from 'clsx'

interface SplitButtonAction {
    title: string
    callback: (event: React.MouseEvent<HTMLElement, MouseEvent>) => any
    hasPriority?: boolean
}

interface SplitButtonProps {
    options: SplitButtonAction[]
    variant?: 'text' | 'outlined' | 'contained'
    size?: 'small' | 'medium' | 'large'
    className?: string
}

const useStyles = makeStyles((theme) => ({
    secondaryButton: {
        minWidth: theme.spacing(3),
        width: theme.spacing(3),
    },
    button: {
        zIndex: -100
    }
}))

export const SplitButton = observer(({ options: actions, variant = 'contained', size = 'small', className }: SplitButtonProps) => {
    const classes = useStyles()

    const [open, setOpen] = useState(false)
    const anchorRef = useRef<HTMLDivElement | null>(null)

    const handleToggle = useCallback((event: React.MouseEvent<HTMLElement>) => {
        event.stopPropagation()
        setOpen(prevOpen => !prevOpen)
    }, [setOpen])

    const handleClose = useCallback((event: React.MouseEvent<HTMLElement>) => {
        event.stopPropagation()
        setOpen(false)
    }, [])

    const sortedActions = useMemo(() => {
        return [...actions.filter(a => !!a.hasPriority), ...actions.filter(a => !a.hasPriority)]
    }, [actions])

    return (
        <>
            <ButtonGroup className={clsx(classes.button, className)} variant={variant} size={size} color="primary" ref={anchorRef} aria-label="split button">
                <Button onClick={sortedActions[0].callback}>{sortedActions[0].title}</Button>
                {sortedActions.length > 1 && (
                    <Button
                        color="primary"
                        size="small"
                        aria-owns={open ? 'menu-list-grow' : undefined}
                        aria-haspopup="true"
                        onClick={handleToggle}
                        className={classes.secondaryButton}
                    >
                        <ArrowDropDownIcon fontSize='small' />
                    </Button>
                )}
            </ButtonGroup>
            <Menu open={open} anchorEl={anchorRef.current} onClose={handleClose}>
                {sortedActions.map((option, index) => (
                    <SplitButtonItem
                        key={index}
                        option={option}
                        handleClose={handleClose}
                    />
                ))}
            </Menu>
        </>
    )
})
interface SplitButtonItemProps {
    option: SplitButtonAction
    handleClose: (event: React.MouseEvent<HTMLElement>) => any
}

const SplitButtonItem = React.forwardRef<HTMLLIElement, SplitButtonItemProps>(({ option, handleClose }, ref) => {
    const { title, callback } = option

    const handleClick = useCallback((event: React.MouseEvent<HTMLElement, MouseEvent>) => {
        handleClose(event)
        callback(event)
    }, [handleClose, callback])

    return (
        <MenuItem ref={ref} onClick={handleClick}> {title} </MenuItem>
    )
}) 