import jsPDF from 'jspdf'
import 'jspdf-autotable'
import { autoTable as AutoTable, UserOptions } from 'jspdf-autotable';

import { DealViewStore } from "../entity-view/DealViewStore";
import { executeLoading } from 'shared-do-not-touch/material-ui-modals/execute-loading';
import { DealWorkflowTaskWithChildren } from '../entity-view/panels/workflow/DealWorkflowStore';
import { WorkflowTaskTypeEnum } from '../../../../clients/deal-system-client-definitions';
import { momentToDateString, momentToDateTimeString, momentToMonthYearString } from 'features/shared/helpers/utils';
import moment from 'moment';
import { messageError } from 'shared-do-not-touch/material-ui-modals';

const table = (doc: jsPDF, options: UserOptions) => ((doc as any).autoTable as AutoTable)(options)
const lastTableY = (doc: jsPDF) => (doc as any).lastAutoTable.finalY as number

export default async function dealSheetPrint(dealId: number, dealNumber: string) {
    executeLoading('Generating report...', async () => {

        const store = new DealViewStore({
            dealId,
            dealNumber,
            visible: false,
        });
        if (!await store.onLoad()) {
            return
        }

        const { deal, infoStore, itemStore, workflowStore } = store
        
        const dealTypeConfiguration = await store.fetchDealTypeConfigurationPromise
        const notesPromise = await store.fetchNotesPromise
        const itemPromise = await store.fetchItemsPromise

        if (!dealTypeConfiguration) { return messageError({ content: "Deal Type configuration was not loaded succesfully." }) }
        if (!notesPromise.loaded) { return messageError({ content: "Deal notes weren not loaded succesfully." }) }
        if (!itemPromise.loaded) { return messageError({ content: "Deal items weren not loaded succesfully." }) }

        const notes = notesPromise.filteredData
        const dealItems = itemPromise.filteredData
        const statuses: string[][] = []
        for (const status of deal.dealWorkflowStatuses) {
            await workflowStore.setCurrentStatusHistory(status)
            let tasks = getTasksDescription(workflowStore.tasks)
            statuses.push([
                status.workflowStatusName.trim(),
                workflowStore.assignedToDescription(status).trim(),
                workflowStore.initiatedByDescription(status).trim() + (!!tasks ? '\n\nTasks:\n\n' : '') + tasks,
            ])
        }

        const { itemFieldsDefinition } = itemStore

        // **** pdf generation start  ****
        const left = 40
        const right = 800
        let top = 50

        var doc = new jsPDF({
            orientation: 'landscape',
            unit: 'pt',
            format: 'a4'
        }) as jsPDF

        // title
        doc.setFontSize(18);
        doc.text("Deal Sheet", left, top);

        doc.setFontSize(8)
        doc.text('Generated on ' + momentToDateTimeString(moment()), right, doc.internal.pageSize.height - 20, undefined, undefined, 'right');
        doc.setFontSize(14);

        // content
        printDealInfo()
        printItems()
        printNotes()
        printWorkflow()

        // generates and downloads the pdf
        doc.save('table.pdf');
        // **** pdf generation end ****

        // ********** data generation functions *************
        function printDealInfo() {
            top += 45;

            doc.text("Deal Info", left, top);
            const fields = [
                'Deal Id',
                'Deal Number',
                'Deal Category',
                'Deal Type',
                'Force Majeure?',
                'Counterparty',
                'Review Date',
            ]

            if (dealTypeConfiguration!.hasExpiryDate) {
                fields.push('Expiry Date')
            }
            top += 10;
            const content = [
                dealId,
                dealNumber,
                infoStore.dealCategoryName,
                infoStore.dealTypeName,
                (deal.forceMajeure.value ? "Yes" : "No"),
                deal.counterpartyName,
                momentToDateTimeString(moment()),
            ]

            if (dealTypeConfiguration!.hasExpiryDate) {
                fields.push(momentToDateString(deal.expiryDate.value))
            }

            table(doc, {
                startY: top,
                head: [fields],
                body: [content]
            });
            top = lastTableY(doc)
        }

        function printItems() {
            top += 30;

            doc.text("Items", left, top);

            top += 10;

            table(doc, {
                startY: top,
                head: [itemFieldsDefinition.map(item => item.title || '')],
                body: dealItems.map(
                    item =>
                        itemFieldsDefinition.map(field => [`${(!!field.render ? field.render(item) : '')}`])
                )
            });

            top = lastTableY(doc)
        }

        function printNotes() {
            top += 30;

            doc.text("Notes", left, top);

            top += 10;
            table(doc, {
                startY: top,
                head: [[
                    'Created',
                    'Note',
                ]],
                body: notes.map(note => [
                    `${note.noteCreatorName} at ${momentToDateTimeString(note.createdDate.value)}`,
                    note.noteContent.value,
                ])
            });
            top = lastTableY(doc)
        }

        function printWorkflow() {
            top += 30;

            doc.text("Deal Life-Cycle", left, top);

            top += 10;

            table(doc, {
                startY: top,
                head: [[
                    'Status',
                    'Assigned To',
                    'Initiated By',
                ]],
                body: statuses
            });

            top = lastTableY(doc)
        }

        function getTasksDescription(tasks: DealWorkflowTaskWithChildren[], prefix = '') {
            let description = ''
            let hasPrefix = false
            for (const taskWithChildren of tasks) {
                const { task } = taskWithChildren
                if (prefix.length > 0) {
                    description += prefix
                    hasPrefix = true
                }
                description += '- ' + task.workflowTaskDescription.value.trimEnd()

                const last = lastChar(description)
                if (last !== '.' && last !== ':' && last !== '?') {
                    description += ':'
                }
                description += ' ';

                switch (task.type) {
                    case WorkflowTaskTypeEnum.EnterDateInformation:
                        description += momentToDateString(task.dateInformation.value)
                        break;
                    case WorkflowTaskTypeEnum.EnterDateTimeInformation:
                        description += momentToDateTimeString(task.dateInformation.value)
                        break;
                    case WorkflowTaskTypeEnum.EnterMonthAndYearInformation:
                        description += momentToMonthYearString(task.dateInformation.value)
                        break;
                    case WorkflowTaskTypeEnum.EnterNumberInformation:
                        description += task.numberInformation.value
                        break;
                    case WorkflowTaskTypeEnum.EnterTextInformation:
                        description += task.textInformation.value
                        break;
                    case WorkflowTaskTypeEnum.ExpiryDateCheck:
                        description += workflowStore.expiryDateCheckDescription()
                        break;
                    case WorkflowTaskTypeEnum.SimpleCheck:
                    case WorkflowTaskTypeEnum.AttachedDocumentDuringStatus:
                    case WorkflowTaskTypeEnum.CreatedNoteDuringStatus:
                    case WorkflowTaskTypeEnum.DealExecutedCheck:
                    case WorkflowTaskTypeEnum.DealNotExecutedCheck:
                        description += (task.done ? 'Done' : 'Not Done')
                        break;
                    case WorkflowTaskTypeEnum.HasItems:
                        description += (task.done ? 'Yes' : 'No')
                        break;
                    case WorkflowTaskTypeEnum.AnswerToQuestion:
                    case WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels:
                        description += task.workflowTaskAnswerText.value
                        break;
                    case WorkflowTaskTypeEnum.EnterMultipleInformation:
                        description += task.textInformation.value
                        break;
                }
                description += '\n'
                description += getTasksDescription(taskWithChildren.children, prefix + '   ')
                if (!hasPrefix) {
                    description += '\n'
                }
            }
            return description
        }
    })
}



const lastChar = (s: string) => s[s.length - 1]