using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Domain.Enum
{
    public enum ConfigurationGroupIdentifiersEnum
    {
        GeneralSettings = 1,
        AbcTradesIntegration = 2,
        Reminders = 3,
    }
    public enum ConfigurationIdentifiersEnum
    {
        TimeOutTransactionScopeIdentifier = 1,
        DealCategoryIdentifier = 2,
        DealTypeIdentifier = 3,
        CounterpartyIdentifier = 4,
        ProductIdentifier = 5,
        WorkflowActionId = 6,
        ShouldReintegrateCancelledDeals = 7,
        EmailAccountsNotifiedOnError = 8,
        ReminderDaysBeforeCounterpartyReviewDate = 9,
        ReminderDaysBeforeDealExpiryDate = 10,
        DealExpiryDatesEmailAccountsNotified = 11,
        CounterpartyExpiryDatesEmailAccountsNotified = 12,
    }
}
