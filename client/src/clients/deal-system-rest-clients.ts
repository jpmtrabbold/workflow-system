import {
    DealClient,
    LoginClient,
    UserClient,
    CounterpartyClient,
    DealCategoryClient,
    ProductClient,
    DealTypeClient,
    DealItemFieldsetClient,
    AuditClient,
    PublicClient,
    SalesForecastClient,
    DealIntegrationClient,
    IntegrationClient,
    ConfigurationClient
} from "./deal-system-client-definitions"
import { authContext, adalConfig } from "./azure-authentication"
import { adalFetch } from "react-adal"
import { fetchWithErrorHandling } from "shared-do-not-touch/fetching"

const isProduction = process.env.REACT_APP_ENVIRONMENT_NAME === "Production"

const fetchObject = {
    async fetch(url: RequestInfo, options?: RequestInit | undefined): Promise<Response> {
        options = options || { headers: {} };

        options.mode = 'cors'
        
        return fetchWithErrorHandling({
            fetchCall: () => adalFetch(authContext, adalConfig.endpoints!.api, window.fetch, url as string, options),
            errorMessageContentProperties: ['Message'],
            errorMessageTitleProperties: ['Title'],
            serverErrorCustomHandling: async (errorResponseObjectFromJson) => {
                if (errorResponseObjectFromJson.InvalidLogin) {
                    setTimeout(() => {
                        const errorMessage = (errorResponseObjectFromJson && errorResponseObjectFromJson.Message) ? errorResponseObjectFromJson.Message : "Invalid Login";
                        (window as any)['setInvalidLoginMessage'](errorMessage)
                    })
                    return true // this tells fetchWithErrorHandling that it doesn't need to provide any UI feedback as I'm providing my own
                }
            }
        })
    }
};

const nonAuthFetchObject = {
    async fetch(url: RequestInfo, options?: RequestInit | undefined): Promise<Response> {
        options = options || { headers: {} };

        options.mode = 'cors'
        
        return fetchWithErrorHandling({
            fetchCall: () => window.fetch(url as string, options),
            errorMessageContentProperties: ['Message'],
            errorMessageTitleProperties: ['Title'],
        })
    }
};

if (!isProduction) {
    (window as any).Logging.log = function (message: any) {
        console.log(message); // this enables logging to the console
    };
    (window as any).Logging.level = 2;
}

const url = process.env.REACT_APP_API_BASE_URL

export const dealClient = new DealClient(url, fetchObject)
export const dealIntegrationClient = new DealIntegrationClient(url, fetchObject)
export const loginClient = new LoginClient(url, fetchObject)
export const userClient = new UserClient(url, fetchObject)
export const counterpartyClient = new CounterpartyClient(url, fetchObject)
export const dealCategoryClient = new DealCategoryClient(url, fetchObject)
export const productClient = new ProductClient(url, fetchObject)
export const dealTypeClient = new DealTypeClient(url, fetchObject)
export const itemFieldsetClient = new DealItemFieldsetClient(url, fetchObject)
export const auditClient = new AuditClient(url, fetchObject)
export const publicClient = new PublicClient(url, nonAuthFetchObject)
export const salesForecastClient = new SalesForecastClient(url, fetchObject)
export const integrationClient = new IntegrationClient(url, fetchObject)
export const configurationClient = new ConfigurationClient(url, fetchObject)