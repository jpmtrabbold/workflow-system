// ** Common
import React from "react"
import { observer } from "mobx-react-lite"
import Checkbox from "@material-ui/core/Checkbox"
import TableRow from "@material-ui/core/TableRow"
import TableCell from "@material-ui/core/TableCell"
import { LookupRequest } from "clients/deal-system-client-definitions"
import useStore from "shared-do-not-touch/mobx-utils/useStore"

interface CounterpartyDealCategoryView {
    row: LookupRequest
    readOnly?: boolean
    dealCategories: number[]
    toggleDealCategorySelection: (dealCategory: LookupRequest) => any
}
export const CounterpartyDealCategoryView = observer((props: CounterpartyDealCategoryView) => {

    const store = useStore(source => ({
        get checked() {
            return !!source.dealCategories.find(pt => pt === source.row.id)
        },
        toggle() {
            source.toggleDealCategorySelection(source.row)
        },
    }), props)

    return (
        <TableRow>
            <TableCell style={{ width: '30px' }} align='center'>
                <Checkbox
                    disabled={props.readOnly}
                    checked={store.checked}
                    onChange={store.toggle}
                />
            </TableCell>
            <TableCell>{props.row.name}</TableCell>
        </TableRow >
    )
})
