import { makeStyles } from '@material-ui/core/styles'

export const useStylesRowOpenedNoBorder = makeStyles(theme => {
    return {
        cellClosed: {

        },
        cellOpened: {

            borderBottom: '0px',
            borderCollapse: 'separate',
        },
    }
})

export const useStylesRowClosedNoBorder = makeStyles(theme => {
    return {
        cellClosed: {

            borderBottom: '0px',
            borderCollapse: 'separate',
        },
        cellOpened: {

        },
    }
})