
/**
 * use this when you need to add properties to an existing object and make it type safe
 * @param obj object that needs to have its type changed
 */
export default function extendObjectKeepingReference<OldType, NewType extends OldType>
    (obj: OldType, newProperties: Omit<NewType, Exclude<keyof OldType, NewType>> ) : NewType {
    
        Object.assign(obj, newProperties)

        return obj as NewType
}
