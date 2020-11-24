import { messageError } from "../material-ui-modals"

export default async function fileToBase64(file: File, message = true) {
    return new Promise<string>((resolve, reject) => {
        const reader = new FileReader()
        reader.onload = () => resolve(reader.result as string)
        reader.onerror = error => {
            if (message) {
                messageError({
                    content: `File could not be read: ${error}`
                })
            }
            reject(error)
        }
        reader.readAsDataURL(file)
    })
}

export async function fileToArrayBuffer(file: File, message = true) {
    return new Promise<ArrayBuffer>((resolve, reject) => {
        const reader = new FileReader()
        reader.onload = () => resolve(reader.result as ArrayBuffer)
        reader.onerror = error => {
            if (message) {
                messageError({
                    content: `File could not be read: ${error}`
                })
            }
            reject(error)
        }
        reader.readAsArrayBuffer(file)
    })
}

export const base64StringToBlob = (b64Data: string, contentType = '', sliceSize = 512) => {
    const byteCharacters = atob(b64Data.replace(/^data:(.+);base64,/, ''));
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        const slice = byteCharacters.slice(offset, offset + sliceSize);

        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i)
        }

        const byteArray = new Uint8Array(byteNumbers)
        byteArrays.push(byteArray)
    }

    const blob = new Blob(byteArrays, { type: contentType })
    return blob
}