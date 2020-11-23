using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum WorkflowTaskTypeEnum
    {
        [Description("Simple check")]
        SimpleCheck = 1,
        [Description("Answer to question")]
        AnswerToQuestion = 2,
        [Description("Enter text information")]
        EnterTextInformation = 3,
        [Description("Enter date information")]
        EnterDateInformation = 4,
        [Description("Enter date/time information")]
        EnterDateTimeInformation = 5,
        [Description("Enter month/year information")]
        EnterMonthAndYearInformation = 6,
        [Description("Enter number information")]
        EnterNumberInformation = 7,
        [Description("Enter multiple data")]
        EnterMultipleInformation = 8,
        [Description("Custom - Expiry Date Check")]
        ExpiryDateCheck = 9,
        [Description("Custom - Deal Is Executed")]
        DealExecutedCheck = 10,
        [Description("Custom - Deal Is Not Executed")]
        DealNotExecutedCheck = 11,
        [Description("Has Items")]
        HasItems = 12,
        [Description("Created a note during this status")]
        CreatedNoteDuringStatus = 13,
        [Description("Attached a document during this status")]
        AttachedDocumentDuringStatus = 14,
        [Description("Verifies if deal is within respective trader authority policy levels")]
        DealWithinRespectiveAuthorityLevels = 15,
    }
}
