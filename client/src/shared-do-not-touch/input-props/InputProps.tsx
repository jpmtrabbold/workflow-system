import { useObserver } from "mobx-react-lite"
import React, { useCallback } from 'react'
import { OnValueChangeType, fieldValueProps, InputPropsVariant, InputPropsConfig, OnValueChangedType, isUpdatable } from "./field-props"
import FormErrorHandler from "./form-error-handler"

/**
 * InputProps parameters
 */
interface InputPropsProps<T extends Object, P extends Extract<keyof T, string>, A extends Object> {
    /**
     * the component that you want InputProps to control. InputProps will add the `value`
     * and `onChange` props to the component to automate the state update to 
     * stateObject[propertyName]
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
    /**
     * in case you want to be notified about when a change will take place. Your callback
     * can return a promise that, if fulfilled with a return of false, will preven the 
     * update to happen.
     */
    onValueChange?: OnValueChangeType<A>
    /**
     * in case you want to be notified about when a change has happened
     */
    onValueChanged?: OnValueChangedType<A>
    /**
     * in case you are using a FormErrorHandler to handle the form errors
     */
    errorHandler?: FormErrorHandler<T>
    /**
     * some built-in variants
     */
    variant?: InputPropsVariant
    /**
     * some built-in configurations
     */
    config?: InputPropsConfig
    /** additinal data that will be passed to onValueChange and onValueChanged */
    additionalChangeData?: A
}

/**
 * A React component that provides a two-way data-binding feel to your forms controlled by a mobx state.
 * @param props InputProps props
 * @example
 * 
 * const store = useLocalStore(() => ({
 *    myFieldValue: ''
 * }))
 * 
 * <InputProps stateObject={store} propertyName='myFieldValue'>
 *    <input/>
 * </InputProps>
 * 
 */
export function InputProps<T extends Object, P extends Extract<keyof T, string>, A extends Object>({
    config = {},
    ...props
}: InputPropsProps<T, P, A>) {
    const [
        stateObject,
        propertyName,
        onValueChange,
        onValueChanged,
        isCheckboxProps,
        errorHandler,
        variant,
        obsConfig,
        additionalChangeData,
        value
    ] = useObserver(() => [
        props.stateObject,
        props.propertyName,
        props.onValueChange,
        props.onValueChanged,
        config.isCheckbox,
        props.errorHandler,
        props.variant,
        config,
        props.additionalChangeData,
        props.stateObject[props.propertyName],
        props.errorHandler && props.errorHandler.errors && props.errorHandler.getFieldError(props.propertyName), // this is for MobX to react to errorHandler changes to this field
        (props.stateObject[props.propertyName] as any)?.value, // this is for MobX to react to value changes
    ])

    const isCheckbox = (isCheckboxProps === undefined ? (isUpdatable(value) ? typeof value.value === 'boolean' : typeof value === 'boolean'): isCheckboxProps)

    const newFieldProps = fieldValueProps(
        stateObject,
        propertyName,
        onValueChange,
        variant,
        { ...obsConfig, isCheckbox },
        onValueChanged,
        additionalChangeData
    
        )
    const onChange = useCallback(newFieldProps.onChange, [stateObject])
    

    const errorProps = !!errorHandler && errorHandler.getFieldError(propertyName)
    const newProps = isCheckbox ? { onChange, checked: newFieldProps.value, ...errorProps } : { onChange, value: newFieldProps.value, ...errorProps }

    return useObserver(() => React.cloneElement(props.children, newProps))
}