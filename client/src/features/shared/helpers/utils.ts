import { Moment } from 'moment'
import { isValidDate } from 'shared-do-not-touch/utils/helper-functions';
import { messageError } from 'shared-do-not-touch/material-ui-modals';
import Papa from 'papaparse';

export function compareDates(dateTimeA?: Moment, dateTimeB?: Moment) {
    if (!dateTimeA && !!dateTimeB) return -1;
    if (!!dateTimeA && !dateTimeB) return 1;
    var dtMomentA = dateTimeA!.clone().startOf('day')
    var dtMomentB = dateTimeB!.clone().startOf('day')
    if (dtMomentA > dtMomentB) return 1;
    else if (dtMomentA < dtMomentB) return -1;
    else return 0;
}

export type DateRange = {
    minDate?: Moment, maxDate?: Moment
}

export function areDateRangesClashing(rangeA: DateRange, rangeB: DateRange) {

    return (compareDates(rangeA.minDate, rangeB.minDate) >= 0 && compareDates(rangeA.minDate, rangeB.maxDate) <= 0)
        ||
        (compareDates(rangeA.maxDate, rangeB.minDate) >= 0 && compareDates(rangeA.maxDate, rangeB.maxDate) <= 0)
        ||
        (compareDates(rangeB.minDate, rangeA.minDate) >= 0 && compareDates(rangeB.minDate, rangeA.maxDate) <= 0)
        ||
        (compareDates(rangeB.maxDate, rangeA.minDate) >= 0 && compareDates(rangeB.maxDate, rangeA.maxDate) <= 0)
}

export function startDateHigherThanEndDate(startDate?: Moment, endDate?: Moment) {
    return !!startDate &&
        !!endDate &&
        compareDates(startDate, endDate) === 1
}


export function selectAllTextOnInput(ctrl: HTMLInputElement | null) {
    if (!ctrl) {
        return
    }

    // Modern browsers
    if (ctrl.setSelectionRange) {
        ctrl.focus();
        ctrl.setSelectionRange(0, ctrl.value.length);

        // IE8 and below
    } else if ((ctrl as any).createTextRange) {
        ctrl.focus();
        var range = (ctrl as any).createTextRange();
        range.collapse(true);
        range.moveEnd('character', ctrl.value.length);
        range.moveStart('character', 0);
        range.select();
    }
}

export function momentToDateTimeString(moment?: Moment) {
    return moment?.local().format("DD/MM/YYYY hh:mm:ss A") ?? ""
}
export function momentToDateString(moment?: Moment) {
    return moment?.local().format("DD/MM/YYYY") ?? ""
}
export function momentToMonthYearString(moment?: Moment) {
    return moment?.local().format("MM/YYYY") ?? ""
}

export function isValidMoment(moment?: Moment) {
    return isValidDate(moment?.toDate())
}


export async function csvFileToObjectTyped<T extends Object>(file: File, message = true) {
    return new Promise<{ data: T[], fields: string[] }>(async (resolve, reject) => {
        try {
            var content = await readFileContent(file)
        } catch (error) {
            reject(error)
            return
        }
        Papa.parse<T>(content.trim(), {
            transformHeader: (header) => header.trim(),
            header: true,
            skipEmptyLines: true,
            complete: (result) => {
                if (result.errors.length) {
                    const errorMessage = result.errors.map(e => `Message: ${e.message}, Row: ${e.row}`).join('; ')
                    if (message) {
                        messageError({
                            content: `File could not be read: ${errorMessage}`
                        })
                    }
                    reject(errorMessage)
                } else {
                    resolve({ data: result.data, fields: result.meta.fields.map(f => f.trim()) })
                }
            },
        })
    })
}

export async function csvFileToObject(file: File, message = true) {
    return new Promise<string[][]>(async (resolve, reject) => {
        try {
            var content = await readFileContent(file)
        } catch (error) {
            reject(error)
            return
        }
        Papa.parse(content.trim(), {
            skipEmptyLines: true,
            complete: (result) => {
                if (result.errors.length) {
                    const errorMessage = result.errors.map(e => `Message: ${e.message}, Row: ${e.row}`).join('; ')
                    if (message) {
                        messageError({
                            content: `File could not be read: ${errorMessage}`
                        })
                    }
                    reject(errorMessage)
                } else {
                    resolve(result.data as any)
                }
            },
        })
    })
}

export async function readFileContent(file: File, message = true) {
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
        reader.readAsText(file)
    })
}

export const messageErrorWithException = async (content: string) => {
    messageError({ content })
    setTimeout(() => {
        throw new Error(content)
    }, 5000);

}