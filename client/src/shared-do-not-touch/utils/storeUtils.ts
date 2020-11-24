import React, { useContext } from 'react'
export const createUseStore = <T>(context: React.Context<T | undefined>, name: string) => {
    context.displayName = name
    return (): T => {
        const store = useContext(context)
        if (!store) {
            throw new Error(`Please pass down the store first using a React.Context Provider (${context.displayName})`)
        }
        return store
    }
}