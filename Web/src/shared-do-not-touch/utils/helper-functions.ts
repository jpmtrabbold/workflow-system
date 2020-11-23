/**
 * converts date object to string formatted as DD/MM/YYYY
 * @param inputFormat date object
 */
export function convertMonthYearForDisplay(inputFormat?: Date) {
  if (!inputFormat)
    return ""

  const d = new Date(inputFormat)
  return [padDateTime(d.getMonth() + 1), d.getFullYear()].join('/')
}

export function convertDateForDisplay(inputFormat?: Date) {
  if (!inputFormat)
    return ""

  const d = new Date(inputFormat)
  return [padDateTime(d.getDate()), padDateTime(d.getMonth() + 1), d.getFullYear()].join('/')
}

function padDateTime(s: number) {
  return (s < 10) ? '0' + s : s
}

export function convertDateTimeForDisplay(inputFormat?: Date) {
  if (!inputFormat)
    return ""
  const dateDisplay = convertDateForDisplay(inputFormat)
  const d = new Date(inputFormat)
  function convertHoursTo12HourFormat(h: number) { return h > 12 ? h -= 12 : h }
  function determineAmOrPm(h: number) { return (h >= 12 ? "PM" : "AM") }

  const time = {
    hours: padDateTime(convertHoursTo12HourFormat(d.getHours())),
    minutes: padDateTime(d.getMinutes()),
    seconds: padDateTime(d.getSeconds()),
    amPm: determineAmOrPm(d.getHours())
  }
  return [dateDisplay, `${time.hours}:${time.minutes}:${time.seconds}`, time.amPm].join(' ')
}

export function isValidDate(d?: Date) {
  if (!d)
    return false

  return d instanceof Date && !isNaN(d.getMonth())
}

export function removeDuplicates<T>(array: T[]) {
  return [...(new Set(array))] as T[]
}

export function stringIncludes(str: string | number | undefined, str2: string) {
  if (!str) {
    return true
  }
  return str.toString().toLowerCase().includes(str2.toLowerCase())
}

