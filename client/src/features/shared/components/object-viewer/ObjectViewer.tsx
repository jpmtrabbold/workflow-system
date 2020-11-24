import React from 'react'
import { observer } from "mobx-react-lite";
import { MessageDialog, MessageDialogAction } from "shared-do-not-touch/material-ui-modals/MessageDialog/MessageDialog";
import { CustomTable } from "shared-do-not-touch/material-ui-table";
import { isArray, isDate } from "util";
import { Column } from "shared-do-not-touch/material-ui-table/CustomTable";
import { Link } from "@material-ui/core";
import { momentToDateTimeString } from 'features/shared/helpers/utils';
import moment from 'moment';
import useStore from 'shared-do-not-touch/mobx-utils/useStore';

export interface ObjectViewerField {
    fieldname: string
    childrenFields?: ObjectViewerField[]
    transformer: (value: any) => any
}

interface ObjectViewerProps {
    object: Object
    title: string
    open: boolean
    onClose: () => any
    /** the fields that should be shown */
    fields?: (ObjectViewerField | string)[]
}
export const ObjectViewer = observer(({
    title,
    open = true,
    object,
    onClose,
    fields,
}: ObjectViewerProps) => {

    const store = useStore(sp => {
        const obj = (sp.object && typeof (sp.object) === 'string') ? JSON.parse(sp.object) : sp.object
        const objectArray = (!obj ? [] : isArray(obj) ? obj : [obj]) as Object[]

        return {
            objectArray,
            objectDrilledDown: null as (Object | null),
            objectDrilledDownName: "",
            objectDrilledDownChildrenFields: undefined as (undefined | ObjectViewerField[]),
            get fields() {
                return sp.fields?.map(f => typeof (f) === 'string' ? { fieldname: f } as ObjectViewerField : f)
            },
            openDrillDown: (object: any, objectName: any) => {
                store.objectDrilledDown = object
                store.objectDrilledDownName = objectName
                store.objectDrilledDownChildrenFields = !store.fields ? undefined : store.fields.find(f => f.fieldname === objectName)?.childrenFields
            },
            closeDrillDown: () => {
                store.objectDrilledDown = null
            },
            get columns() {
                if (store.objectArray.length === 0) {
                    return []
                }
                let keys = [] as { key: string, index: number, definition?: ObjectViewerField }[]
                store.objectArray.forEach(i => {
                    const objectKeys = Object.keys(i).map(k => ({
                        fieldname: k,
                        definition: (store.fields ? store.fields.find(f => f.fieldname === k) : undefined)
                    })).filter(o => store.fields ? !!o.definition : true)

                    keys = keys.concat(objectKeys.map((key, index) => ({ key: key.fieldname, index, definition: key.definition }))
                        .filter(j => !keys.find(k => k.key === j.key)))
                })
                keys.sort((a, b) => a.index - b.index)
                return keys.map(({ key, definition }) => {
                    return {
                        title: key,
                        field: key,
                        render: (data) => {
                            const value = data[key]

                            if (definition?.transformer) {
                                return definition?.transformer(value)
                            }

                            if (value === null || value === undefined) {
                                return ""
                            }

                            const type = typeof value

                            if (type === "string" && moment(value, moment.ISO_8601, true).isValid()) {
                                return momentToDateTimeString(moment(value))
                            }

                            if (["string", "number", "bigint", "undefined"].includes(type)) {
                                return value
                            }

                            if (type === 'boolean') {
                                return value ? "Yes" : "No"
                            }

                            if (isDate(value)) {
                                return momentToDateTimeString(moment(value))
                            }
                            if (moment(value, moment.ISO_8601, true).isValid()) {
                                return momentToDateTimeString(moment(value))
                            }

                            if (typeof (value) === 'object' || isArray(value)) {
                                return (
                                    <Link onClick={() => store.openDrillDown(value, key)} style={{ cursor: 'pointer' }}>
                                        View Data
                                    </Link>
                                )
                            }
                            return value
                        }
                    } as Column<any>
                })
            },
            get actions(): MessageDialogAction[] {
                return [
                    {
                        name: 'Close',
                        color: 'primary',
                        callback: sp.onClose,
                    }
                ]
            }
        }
    }, { object, onClose, fields })

    return (
        <MessageDialog open={open} onClose={onClose} maxWidth='xl' actions={store.actions}>
            <CustomTable
                title={title}
                columns={store.columns}
                rows={store.objectArray}
                pagination={store.objectArray.length > 5}
            />
            {!!store.objectDrilledDown && (
                <ObjectViewer
                    open={true}
                    title={"View Data - " + store.objectDrilledDownName}
                    object={store.objectDrilledDown}
                    onClose={store.closeDrillDown}
                    fields={store.objectDrilledDownChildrenFields}
                />
            )}
        </MessageDialog>
    )
})