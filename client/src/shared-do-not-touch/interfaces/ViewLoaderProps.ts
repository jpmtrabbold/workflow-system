export default interface ViewLoaderProps {
    onEntityClose: () => any
    entityId?: number
    entityName?: string
    visible: boolean
    readOnly?: boolean
}