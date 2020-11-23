import React from 'react'
import { observer } from 'mobx-react-lite'
/**
 * ConditionalRenderer parameters
 */
interface ConditionalRendererProps<T extends Object, P extends Extract<keyof T, string>> {
    /**
     * the component that you want ConditionalRenderer to render
     */
    children: React.ReactElement
    /**
     * object that holds the property that will be bound to this component. That property
     * has to be a mobx observable
     */
    stateObject: T
    /**
     * name of the property inside the stateObject that will be bound to this component. 
     * That property has to be a mobx observable
     */
    propertyName: P
}

/**
 * A React component that renders its children based on the mobx observable property. This is helpful for performance tuning, as it 
 * creates a self-rendering section of the component
 * @param props ConditionalRenderer props
 */
export const ConditionalRenderer = observer(<T extends Object, P extends Extract<keyof T, string>>(props: ConditionalRendererProps<T, P>) => {
    if (!props.stateObject[props.propertyName]) {
        return null
    }
    return props.children
})