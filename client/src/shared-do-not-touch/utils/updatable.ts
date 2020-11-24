
interface IUpdatable<T> extends Object { value?: T, updated: boolean }
/**
 * use this function to assign a value to an updatable field
 * @param updatableObject object that contains 'value' and 'updated' properties
 * @param newValue value that will be assigned to the updatable object
 */
export default function updatable<T>(updatableObject: IUpdatable<T>, newValue: T | IUpdatable<T>, forceUpdated = false) {
    let value
    if (newValue !== null &&
        typeof newValue === 'object' &&
        (newValue as IUpdatable<T>).hasOwnProperty('value') &&
        (newValue as IUpdatable<T>).hasOwnProperty('updated')) {

        value = (newValue as IUpdatable<T>).value
    } else {
        value = newValue
    }
    if (forceUpdated || updatableObject.value !== value) {
        updatableObject.value = value as T
        if (!updatableObject.updated)
            updatableObject.updated = true
    }
}

export function initializeUpdatable<T>(updatableObject: IUpdatable<T>, newValue: T) {
    updatable(updatableObject, newValue, true)
}

/**
 * verifies if there is any updatable property in the object that has been updated
 * @param object object that contains updatables (e.g. objects with an 'updated' boolean property)
 */
export function anyPropertyUpdated(object: any): boolean {
    for (var property in object) {
        if (typeof (object[property]) === 'object' && object[property] !== null && object[property].updated) {
            return true
        }
    }
    return false
}

/**
 * forces all updatables to true
 * @param object object that contains updatables (objects with an 'updated' boolean property)
 */
export function forceUpdatables(object: any) {
    for (var property in object) {
        if (typeof (object[property]) === 'object' && object[property] !== null && typeof object[property].updated === 'boolean' && !object[property].updated) {
            object[property].updated = true
        }
    }
}

