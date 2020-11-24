import './roboto.css'
import { createMuiTheme } from '@material-ui/core/styles'
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme'

export const createTheme = (dark = false) => {
  const purple = dark ? '#DFBAE8' : '#6c217d'
  const purpleLight = dark ? '#DFBAE8' : '#ad14c8'
  const blue = dark ? '#498ede' : '#2A7DDD' //'#66b0ff' : '#2A7DDD'
  const grey = dark ? '#8d8d8d' : 'bdbdbd'
  const options: ThemeOptions = {
    palette: {
      text: dark ? { primary: '#e3e3e3' } : {},
      type: dark ? 'dark' : 'light',
      primary: {
        light: '#66b0ff',
        main: blue,
      },
      secondary: {
        light: purpleLight,
        main: purple,
      },
      error: {
        main: '#FA0057',
      },
    },
    typography: {
      fontFamily: 'Roboto, sans-serif',
      h5: {
        fontWeight: 100,
        color: purple,
      },
      h6: {
        fontSize: '1.2rem',
        fontWeight: 400,
        color: dark ? 'white' : 'black',
      },
      subtitle1: {
        fontSize: '1rem',
        fontWeight: 500,
        color: dark ? 'white' : 'grey',
      },
      subtitle2: {
        fontWeight: 500,
        color: grey,
      },
      body1: {
        fontSize: '1.00rem',
      },
      body2: {
        fontSize: '0.9rem',
      },
    },
    overrides: {
      MuiAppBar: {
        root: {
          backgroundImage: dark ? '-webkit-linear-gradient(left, #103259 0%, #40154a 50%)' : '-webkit-linear-gradient(left, #2a7ddd 0%, #6c217d 50%)',
        },
      },
    },
  }
  return createMuiTheme(options)
}

export const theme = createTheme()
