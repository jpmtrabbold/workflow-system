import { AuthenticationContext, AdalConfig, withAdalLogin } from 'react-adal'

export const adalConfig: AdalConfig = {
    tenant: process.env.REACT_APP_ADAL_TENANT as string,
    clientId: process.env.REACT_APP_ADAL_CLIENT_ID as string,
    extraQueryParameter: 'nux=1',
    endpoints: {
        'api': process.env.REACT_APP_ADAL_CLIENT_ID as string
    },
    postLogoutRedirectUri: window.location.origin,
    redirectUri: process.env.REACT_APP_ADAL_REDIRECT_URI,
    cacheLocation: 'sessionStorage',
    expireOffsetSeconds: 600,
    callback: (errorDesc: string | null, token: string | null, error: any) => {
        if (process.env.REACT_APP_ENVIRONMENT_NAME !== "Production") {
            console.log(`Authentication Token acquired - ${new Date()}. 
                errorDesc: ${errorDesc}
                token: ${token}
                error: ${error}
                `)
        }

    }
}

export const authContext = new AuthenticationContext(adalConfig)

export const withAdalLoginApi = withAdalLogin(authContext, adalConfig?.endpoints?.api || "");

export const getToken = () => {
    return authContext.getCachedToken(authContext.config.clientId)
}

export const getAzureUserNameForUser = () => {
    return authContext.getCachedUser().profile.name
}