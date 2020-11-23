import { action } from "mobx";
import moment from "moment";
import { DealListDto, DealsListRequest } from "clients/deal-system-client-definitions";
import { dealClient } from "clients/deal-system-rest-clients";
import { CustomTableStore } from "shared-do-not-touch/material-ui-table/CustomTableStore";
import dealSummaryListPrint from "./deal-summary-list-print";

export class DealSummaryListStore {

    startCreationDate?= moment(new Date()).add(-1, 'month')
    endCreationDate?= moment()
    rows = [] as DealListDto[]
    loading = false

    tableStore?: CustomTableStore<DealListDto>

    dataFetch = async () => {
        this.loading = true

        setTimeout(action(async () => {
            const listRequest = new DealsListRequest()

            listRequest.startCreationDate = this.startCreationDate
            listRequest.endCreationDate = this.endCreationDate
            listRequest.includeFinalizedDeals = true

            try {
                var response = await dealClient.list(listRequest)
            } catch (error) {
                this.loading = false
                return
            }

            this.rows = response.deals
            this.loading = false
        }))
    }

    generatePDF = async () => {
        dealSummaryListPrint({
            startCreationDate: this.startCreationDate,
            endCreationDate: this.endCreationDate,
            rows: this.tableStore!.filteredAndSortedRows.map(fs => fs.row),
        })
    }

    setTableStoreReference = (storeReference: CustomTableStore<DealListDto>) => {
        this.tableStore = storeReference
    }

    nullChange() { return }
}