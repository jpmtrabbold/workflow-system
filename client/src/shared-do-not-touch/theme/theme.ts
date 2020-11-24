import { createMuiTheme } from '@material-ui/core/styles'
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme'

export const createTheme = (dark = false) => {
    const secondary = dark ? '#e8baba' : '#7d2121'
    const secondaryLight = dark ? '#e8baba' : '#ad14c8'
    const blue = dark ? '#69a2e5' : '#246bbe'//'#66b0ff' : '#2A7DDD'
    var red = dark ? '#ff9fc1' : '#FA0057'
    const options: ThemeOptions = {
        palette: {
            text: dark ? { primary: '#e3e3e3' } : {},
            type: dark ? 'dark' : 'light',
            primary: {
                light: '#66b0ff',
                main: blue,
            },
            secondary: {
                light: secondaryLight,
                main: secondary,
            },
            error: {
                main: red
            },
        },
        typography: {
            fontFamily: 'Roboto, sans-serif',
            h5: {
                fontWeight: 100,
                color: secondary,
            },
            h6: {
                fontWeight: 400,
                color: secondary,
            },
            subtitle1: {
                fontWeight: 500,
                color: secondary,
            },
        },
        overrides: {
            MuiAppBar: {
                root: {
                    backgroundImage: dark ? '-webkit-linear-gradient(left, #4a151c 0%, #4a151c 50%)' : '-webkit-linear-gradient(left, #7d2121 0%, #7d2121 50%)',
                    '& h6': {
                        color: 'white'
                    },
                    '& button': {
                        color: 'white'
                    },
                },
            },
        }
    }
    return createMuiTheme(options)
}

export const theme = createTheme()