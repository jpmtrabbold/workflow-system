import React from 'react'
import { messageError, messageConfirm } from "../material-ui-modals"
import Accordion from "@material-ui/core/Accordion"
import AccordionSummary from "@material-ui/core/AccordionSummary"
import AccordionDetails from "@material-ui/core/AccordionDetails"
import Typography from "@material-ui/core/Typography"

export interface fetchWithErrorHandlingProps {
    /**
     * inline function that will fetch data and return a Response
     */
    fetchCall: () => Promise<Response> | Response
    /**
     * option array that will contain property names from a json error that can be used as a error message content
     */
    errorMessageContentProperties?: string[]
    /**
     * option array that will contain property names from a json error that can be used as a error message title
     */
    errorMessageTitleProperties?: string[]
    /**
     * option array that will contain property names from a json error that can be used as a error message stack trace
     */
    errorMessageStackTraceProperties?: string[]
    /**
     * function that will be called when the api returned any status different than 2** and 401 (error) so you can
     * handle this error yourself an possibly access custom properties from errorResponseObjectFromJson
     * @param errorResponseObjectFromJson error json object returned by the server
     * @param response http response object
     * @returns if you return true, you are requesting that no UI is to be shown with the error (because you might show it in your code, or it is not necessary at all)
     */
    serverErrorCustomHandling?: (errorResponseObjectFromJson: any, response?: Response) => Promise<boolean | undefined>
    /**
     * function that will be called when the api call throws any other error not handled by serverErrorCustomHandling. This might be for any reason.
     * @returns if you return true, you are requesting that no UI is to be shown with the error (because you might show it in your code, or it is not necessary at all)
     */
    errorCustomHandling?: (errorReason: any) => Promise<boolean | undefined>
}

/**
 * Use this to call apis with proper error handling and visual feedback
 * @param param0 props in fetchWithErrorHandlingProps
 * @example
 * const response = await fetchWithErrorHandling(() => window.fetch(url, options))
 */
export async function fetchWithErrorHandling({
    fetchCall,
    serverErrorCustomHandling,
    errorMessageContentProperties,
    errorMessageTitleProperties,
    errorMessageStackTraceProperties
}: fetchWithErrorHandlingProps): Promise<Response> {
    try {
        const response = await fetchCall()
        if (response) {
            if (response.status === 204) {
                // There's no content with a 204 response, so although it's classed as successful,
                // there is no text that comes back on the reponse object, so we just create a new
                // Response.
                return new Response()
            } else if (response.status === 401) {
                return notAuthenticatedError()
            } else if (response.status.toString()[0] !== '2') {
                try {
                    const textResponse = await response.text()
                    try {
                        let jsonResponse: string
                        try {
                            jsonResponse = !!textResponse ? JSON.parse(textResponse): ""
                        } catch (e) {
                            jsonResponse = textResponse ?? ""
                        }

                        if (serverErrorCustomHandling && (await serverErrorCustomHandling(jsonResponse, response)) === true) {
                            return Promise.reject(jsonResponse)
                        }
                        const messageContent = getFirstValidProperty(jsonResponse, errorMessageContentProperties || ["Message"]) || textResponse
                        const messageTitle = getFirstValidProperty(jsonResponse, errorMessageTitleProperties || ["Title"]) || 'Error'
                        const stackTrace = getFirstValidProperty(jsonResponse, errorMessageStackTraceProperties || ["StackTrace"])

                        return errorHandler(messageContent, messageTitle, stackTrace)

                    } catch (error) {
                        return errorHandler(processError(error))

                    }
                } catch (error) {
                    return errorHandler(processError(error))

                }
            }
        }
        return response
    } catch (errorResponse) {
        if (serverErrorCustomHandling && (await serverErrorCustomHandling(errorResponse)) === true) {
            return Promise.reject(errorResponse)
        }

        if (!!errorResponse) {
            if (!!errorResponse.response && errorResponse.response.status === '401') {
                return notAuthenticatedError()
            }
            if (errorResponse.msg === "interaction_required" && !!errorResponse.message) {
                return notAuthenticatedError(errorResponse.message)
            }
        }
        return errorHandler(`Error with server communication: ${processError(errorResponse)}`)

    }
}

function getFirstValidProperty<T extends Object>(obj: T, propertyNames?: string[]) {
    if (!propertyNames) {
        return undefined
    }
    for (const property of propertyNames) {
        const value = (obj as any)[property]
        if (value) {
            return value
        }
    }
}

const processError = (error: any) => {
    if (typeof error === 'object') {
        if (!!error.message) {
            return error.message + (error.stack ? '; Stack: ' + error.stack : '')
        }
        return JSON.stringify(error).replace(/(?:\\[rn])+/g, "\n")
    } else {
        return error
    }
}


const errorHandler = (content: any, title?: string, stackTrace?: string) => {
    if (!content) {
        content = "Unexpected behavior from application server."
    }
    const contentMessage = processError(content)
    messageError({
        content: (
            <>
                <Typography variant='inherit'>
                    {contentMessage}
                </Typography>
                {!!stackTrace && (
                    <>
                        <br />
                        <br />
                        <Accordion>
                            <AccordionSummary>
                                Error Stack Trace
                            </AccordionSummary>
                            <AccordionDetails>
                                <Typography variant='inherit'>
                                    {stackTrace}
                                </Typography>
                            </AccordionDetails>
                        </Accordion>
                    </>
                )}
            </>
        ), title
    })
    return Promise.reject(content)
}

const notAuthenticatedError = (additionalMsg?: string) => {

    messageConfirm({ content: `User is not authenticated${!!additionalMsg ? ` (error: ${additionalMsg})` : ""}. Would you like to reload the page?`, title: 'Authentication' }).then((confirmed) => {
        if (confirmed) {
            window.location.reload()
        }
    })

    return Promise.reject("User is not authenticated")
}