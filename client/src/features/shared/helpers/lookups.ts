
export const positionLookup = [{ id: 1, name: 'Buy' }, { id: 0, name: 'Sell' }]

export const positionName = (id?: number) => {
    const l = positionLookup.find(i => i.id === id)
    return (l ? l.name : "")
}

export const dayTypeLookup = [{ id: 1, name: 'All Days' }, { id: 2, name: 'Week Days' }, { id: 3, name: 'Weekend Days' }]

export const dayTypeName = (id?: number) => {
    if (!id)
        return ""

    const l = dayTypeLookup.find(i => i.id === id)
    return (l ? l.name : "")
}