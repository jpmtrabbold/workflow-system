import { runWithAdal } from 'react-adal'
import { authContext } from './clients/azure-authentication'
const doNotLogin = true

runWithAdal(authContext,
    () => {
        require('./indexApp.tsx')
    },
    doNotLogin)