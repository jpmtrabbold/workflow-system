import jsPDF from 'jspdf'
import 'jspdf-autotable'
import { autoTable as AutoTable, UserOptions } from 'jspdf-autotable'
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading'
import { dealSummaryListGridDefinition } from './deal-summary-list-grid-definition'
import { DealListDto } from 'clients/deal-system-client-definitions'
import moment, { Moment } from 'moment'
import { momentToDateString, momentToDateTimeString } from 'features/shared/helpers/utils'

const table = (doc: jsPDF, options: UserOptions) => ((doc as any).autoTable as AutoTable)(options)
const lastTableY = (doc: jsPDF) => (doc as any).lastAutoTable.finalY as number

interface dealSummaryListPrintProps {
    startCreationDate?: Moment
    endCreationDate?: Moment
    rows: DealListDto[]
}

export default async function dealSummaryListPrint({
    startCreationDate,
    endCreationDate,
    rows,
}: dealSummaryListPrintProps) {

    executeLoading('Generating report...', async () => {
        const totalPagesExp = "{total_pages_count_string}";

        // **** pdf generation start  ****
        const left = 40
        const right = 800
        let top = 50

        var doc = new jsPDF({
            orientation: 'landscape',
            unit: 'pt',
            format: 'a4'
        }) as jsPDF
        
        //alert(JSON.stringify(doc.getFontList()))
        // title
        doc.setFontSize(18);
        doc.setFont('helvetica', 'bold')
        doc.text("Deal Summary List", left, top);
        
        // content
        deals()
        // Total page number plugin only available in jspdf v1.0+
        if (typeof doc.putTotalPages === 'function') {
            doc.putTotalPages(totalPagesExp);
        }
        // generates and downloads the pdf
        doc.save('table.pdf');
        // **** pdf generation end ****

        // ********** data generation functions *************
        function deals() {
            top += 30;

            doc.setFontSize(10);
            doc.setFont('helvetica', 'normal')

            if (!!startCreationDate && !!endCreationDate) {
                doc.text(`Creation Date Filter: All Deals from ${momentToDateString(startCreationDate)} to ${momentToDateString(endCreationDate)}`, left, top);
            } else if (!!startCreationDate) {
                doc.text(`Creation Date Filter: All Deals from ${momentToDateString(startCreationDate)} onwards`, left, top);
            } else if (!!endCreationDate) {
                doc.text(`Creation Date Filter: All Deals up to ${momentToDateString(endCreationDate)}`, left, top);
            }

            top += 20;

            table(doc, {
                startY: top,
                showFoot: 'everyPage',
                didDrawPage: (hookData) => {
                    doc.text(`Page ${hookData.pageNumber} of ${totalPagesExp}`, left, doc.internal.pageSize.height - 40, undefined, undefined, 'left')
                    doc.text('Deal Summary List generated on ' + momentToDateTimeString(moment()), right, doc.internal.pageSize.height - 40, undefined, undefined, 'right')
                
                },
                margin: { bottom: 50 },
                head: [dealSummaryListGridDefinition.map(d => d.title as string)],
                body: rows.map(row => dealSummaryListGridDefinition.map(d => {
                    if (d.render) {
                        const render = d.render(row)
                        if (typeof render !== 'object') {
                            return render
                        }
                    }
                    return row[d.field!]
                })),
            } as UserOptions);
            top = lastTableY(doc)
        }

    })
}