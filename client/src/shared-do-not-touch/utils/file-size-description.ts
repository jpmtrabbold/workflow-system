
const byteNamings = ['bytes','KB','MB','TB','PB','EB','ZB','YB']

export default function fileSizeDescription(sizeInBytes: number) {
    let places = 0
    for (const naming of byteNamings) {
        if (sizeInBytes <= 1024) {
            return `${sizeInBytes.toFixed(places)} ${naming}`
        }
        sizeInBytes /= 1024
        places = 2
    }

    throw new Error('Error calculating file size')
}