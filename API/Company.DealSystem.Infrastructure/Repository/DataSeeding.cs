using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.ExtensionMethods;
using Company.DealSystem.Domain.Entities.Configuration;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Infrastructure.Context
{
    public class DataSeeding
    {
        internal static void Seed(ModelBuilder mb)
        {
            Configuration(mb);
            Countries(mb);
            DealCategoriesAndCounterparties(mb);
            ItemFields(mb);
            Workflow(mb);
            TraderAuthorityPolicies(mb);
            FunctionalitiesAndRoles(mb);
            Products(mb);
            AttachmentTypes(mb);

            mb.HasData(new User { Id = 1, Name = "Generic User", Active = true, Username = "user@user.com", UserRoleId = 2 });
            mb.HasData(new UserInWorkflowRole { Active = true, Id = 1, UserId = 1, WorkflowRoleId = middleOfficeRoleId});
            mb.HasData(new UserInWorkflowRole { Active = true, Id = 2, UserId = 1, WorkflowRoleId = TraderLevel1WorkflowRole });
            mb.HasData(new UserInWorkflowRole { Active = true, Id = 3, UserId = 1, WorkflowRoleId = backOfficeRoleId });
        }

        private static void Configuration(ModelBuilder mb)
        {
            int groupId;
            groupId = mb.HasData(new ConfigurationGroup { Id = 1, Name = "General Settings", Description = "Configuration settings that have system-wide effects", Identifier = ConfigurationGroupIdentifiersEnum.GeneralSettings });
            mb.HasData(new ConfigurationEntry
            {
                Id = 1,
                Name = "Time-Out In Minutes",
                Identifier = ConfigurationIdentifiersEnum.TimeOutTransactionScopeIdentifier,
                ConfigurationGroupId = groupId,
                Content = "15",
                ContentType = ConfigurationEntryContentType.IntType,
            });

            groupId = mb.HasData(new ConfigurationGroup { Id = 2, Name = "ABC Integration", Description = "Data related to the integration of ABC Trades into DealSystem", Identifier = ConfigurationGroupIdentifiersEnum.AbcTradesIntegration });
            mb.HasData(new ConfigurationEntry
            {
                Id = 2,
                Name = "Deal Category",
                Identifier = ConfigurationIdentifiersEnum.DealCategoryIdentifier,
                ConfigurationGroupId = groupId,
                Content = ((int)DealCategoryEnum.Gas).ToString(),
                ContentType = ConfigurationEntryContentType.IntType,
                FunctionalityForLookup = FunctionalityEnum.DealCategories,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 3,
                Name = "Deal Type",
                Identifier = ConfigurationIdentifiersEnum.DealTypeIdentifier,
                ConfigurationGroupId = groupId,
                Content = AbcTradesDealType.ToString(),
                ContentType = ConfigurationEntryContentType.IntType,
                FunctionalityForLookup = FunctionalityEnum.DealTypes,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 4,
                Name = "Counterparty",
                Identifier = ConfigurationIdentifiersEnum.CounterpartyIdentifier,
                ConfigurationGroupId = groupId,
                Content = "94",
                ContentType = ConfigurationEntryContentType.IntType,
                FunctionalityForLookup = FunctionalityEnum.Counterparties,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 5,
                Name = "Product",
                Identifier = ConfigurationIdentifiersEnum.ProductIdentifier,
                ConfigurationGroupId = groupId,
                Content = "60",
                ContentType = ConfigurationEntryContentType.IntType,
                FunctionalityForLookup = FunctionalityEnum.Products,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 6,
                Name = "WorkflowActionId for 'Submit' workflow action",
                Identifier = ConfigurationIdentifiersEnum.WorkflowActionId,
                ConfigurationGroupId = groupId,
                Content = preExecutedDealsSubmitActionId.ToString(),
                ContentType = ConfigurationEntryContentType.IntType,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 7,
                Name = "Reintegrate cancelled deals?",
                Identifier = ConfigurationIdentifiersEnum.ShouldReintegrateCancelledDeals,
                ConfigurationGroupId = groupId,
                Content = true.ToString(),
                ContentType = ConfigurationEntryContentType.BooleanType,
            });
            mb.HasData(new ConfigurationEntry
            {
                Id = 8,
                Name = "E-mail accounts that will be notified on integration warnings/errors - use ; (semicolon) for multiple",
                Identifier = ConfigurationIdentifiersEnum.EmailAccountsNotifiedOnError,
                ConfigurationGroupId = groupId,
                Content = "user@user.com",
                ContentType = ConfigurationEntryContentType.StringType,
            });

            groupId = mb.HasData(new ConfigurationGroup { Id = 3, Name = "Reminders Settings", Description = "Configuration for reminder notifications", Identifier = ConfigurationGroupIdentifiersEnum.Reminders });
            mb.HasData(new ConfigurationEntry
            {
                Id = 9,
                Name = "days before counterparty review date (separated by ;)",
                Identifier = ConfigurationIdentifiersEnum.ReminderDaysBeforeCounterpartyReviewDate,
                ConfigurationGroupId = groupId,
                Content = "-28; 0",
                ContentType = ConfigurationEntryContentType.StringType,
            });

            mb.HasData(new ConfigurationEntry
            {
                Id = 10,
                Name = "E-mail accounts to be notified about counterparty review dates (separated by ;)",
                Identifier = ConfigurationIdentifiersEnum.CounterpartyExpiryDatesEmailAccountsNotified,
                ConfigurationGroupId = groupId,
                Content = "user@user.com",
                ContentType = ConfigurationEntryContentType.StringType,
            });

            mb.HasData(new ConfigurationEntry
            {
                Id = 11,
                Name = "days before deal expiry date (separated by ;)",
                Identifier = ConfigurationIdentifiersEnum.ReminderDaysBeforeDealExpiryDate,
                ConfigurationGroupId = groupId,
                Content = "-7; 0",
                ContentType = ConfigurationEntryContentType.StringType,
            });

            mb.HasData(new ConfigurationEntry
            {
                Id = 12,
                Name = "E-mail accounts to be notified about deal expiry dates (separated by ;)",
                Identifier = ConfigurationIdentifiersEnum.DealExpiryDatesEmailAccountsNotified,
                ConfigurationGroupId = groupId,
                Content = "user@user.com",
                ContentType = ConfigurationEntryContentType.StringType,
            });


        }

        private static void Countries(ModelBuilder mb)
        {
            mb.HasData(new Country { Id = 1, Code = "NZ", Name = "New Zealand" });
            mb.HasData(new Country { Id = 2, Code = "AU", Name = "Australia" });
        }

        static int pricingSummaryAttachmentType = 1;
        static int validationSummaryAttachmentType = 2;
        static int interimVolumesAttachmentType = 3;
        static int finalVolumesAttachmentType = 4;
        static int signedPPAAttachmentType = 5;
        static int solicitorsCertificateAttachmentType = 6;
        static int counterpartyExposureAttachmentType = 7;
        static int signedConfirmationAttachmentType = 8;
        static int bidFileAttachmentType = 9;
        static int validationResultsCheckWorkbookAttachmentType = 10;
        static int FTRCSVFileAttachmentType = 11;
        static int contractsAwardedSummaryAttachmentType = 12;
        static int signedContractAttachmentType = 13;
        static int contractNoteAttachmentType = 14;
        static int invoiceAttachmentType = 15;
        static int otherAttachmentType = 16;
        static int abcDealNotificationsAttachmentType = 17;

        private static void AttachmentTypes(ModelBuilder mb)
        {
            mb.HasData(new AttachmentType { Id = pricingSummaryAttachmentType, Name = "Pricing Summary", });
            mb.HasData(new AttachmentType { Id = validationSummaryAttachmentType, Name = "Validation Summary", });
            mb.HasData(new AttachmentType { Id = interimVolumesAttachmentType, Name = "Interim Volumes", });
            mb.HasData(new AttachmentType { Id = finalVolumesAttachmentType, Name = "Final Volumes", });
            mb.HasData(new AttachmentType { Id = signedPPAAttachmentType, Name = "Signed Agreement", });
            mb.HasData(new AttachmentType { Id = solicitorsCertificateAttachmentType, Name = "Solicitor's Certificate", });
            mb.HasData(new AttachmentType { Id = counterpartyExposureAttachmentType, Name = "Counterparty Exposure", });
            mb.HasData(new AttachmentType { Id = signedConfirmationAttachmentType, Name = "Signed Confirmation", });
            mb.HasData(new AttachmentType { Id = bidFileAttachmentType, Name = "Bid File", });
            mb.HasData(new AttachmentType { Id = validationResultsCheckWorkbookAttachmentType, Name = "Validation Workbook", });
            mb.HasData(new AttachmentType { Id = FTRCSVFileAttachmentType, Name = "Results CSV File", });
            mb.HasData(new AttachmentType { Id = contractsAwardedSummaryAttachmentType, Name = "Contracts Awarded Summary", });
            mb.HasData(new AttachmentType { Id = signedContractAttachmentType, Name = "Signed Contract", });
            mb.HasData(new AttachmentType { Id = contractNoteAttachmentType, Name = "Contract Note", });
            mb.HasData(new AttachmentType { Id = invoiceAttachmentType, Name = "Invoice", });
            mb.HasData(new AttachmentType { Id = otherAttachmentType, Name = "Other", Other = true });
            mb.HasData(new AttachmentType { Id = abcDealNotificationsAttachmentType, Name = "ABC Deal Notification" });
        }

        private static void Products(ModelBuilder mb)
        {
            mb.HasData(new Product { Id = 1, DealCategoryId = 1, Name = "Meat", });
            mb.HasData(new Product { Id = 2, DealCategoryId = 1, Name = "Lettuce", });
            mb.HasData(new Product { Id = 3, DealCategoryId = 1, Name = "Banana", });
            mb.HasData(new Product { Id = 4, DealCategoryId = 1, Name = "Tomato", });
            mb.HasData(new Product { Id = 5, DealCategoryId = 1, Name = "Lemon", });
            mb.HasData(new Product { Id = 6, DealCategoryId = 1, Name = "Chicken", });
            mb.HasData(new Product { Id = 7, DealCategoryId = 1, Name = "Pork", });
            mb.HasData(new Product { Id = 8, DealCategoryId = 1, Name = "Rice", });
            mb.HasData(new Product { Id = 9, DealCategoryId = 1, Name = "Beans", });
            mb.HasData(new Product { Id = 10, DealCategoryId = 1, Name = "Water", });
            mb.HasData(new Product { Id = 11, DealCategoryId = 1, Name = "Bread", });
            mb.HasData(new Product { Id = 12, DealCategoryId = 1, Name = "Cupcake", });
            mb.HasData(new Product { Id = 13, DealCategoryId = 1, Name = "Cake", });
            mb.HasData(new Product { Id = 14, DealCategoryId = 1, Name = "Mudcake", });
            mb.HasData(new Product { Id = 15, DealCategoryId = 1, Name = "Cheesecake", });
        }

        private static void FunctionalitiesAndRoles(ModelBuilder mb)
        {
            var deal = mb.HasData(new Functionality { Id = 1, FunctionalityEnum = FunctionalityEnum.Deals, Name = "Deals" });
            var dealCreate = SubFunctionality(mb, deal, 1, "Create", SubFunctionalityEnum.Create);
            var dealEdit = SubFunctionality(mb, deal, 2, "Edit", SubFunctionalityEnum.Edit);
            var dealView = SubFunctionality(mb, deal, 3, "View", SubFunctionalityEnum.View);
            var dealPDFExport = SubFunctionality(mb, deal, 4, "PDF Export", SubFunctionalityEnum.PDFExport);
            var dealAuditLogsView = SubFunctionality(mb, deal, 25, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);
            var dealExecute = SubFunctionality(mb, deal, 32, "Execute Deal", SubFunctionalityEnum.ExecuteDeal);

            var users = mb.HasData(new Functionality { Id = 3, FunctionalityEnum = FunctionalityEnum.Users, Name = "Users" });
            var usersCreate = SubFunctionality(mb, users, 7, "Create", SubFunctionalityEnum.Create);
            var usersEdit = SubFunctionality(mb, users, 8, "Edit", SubFunctionalityEnum.Edit);
            var usersView = SubFunctionality(mb, users, 9, "View", SubFunctionalityEnum.View);
            var usersAuditLogsView = SubFunctionality(mb, users, 26, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var counterparties = mb.HasData(new Functionality { Id = 4, FunctionalityEnum = FunctionalityEnum.Counterparties, Name = "Counterparties" });
            var counterpartiesCreate = SubFunctionality(mb, counterparties, 10, "Create", SubFunctionalityEnum.Create);
            var counterpartiesEdit = SubFunctionality(mb, counterparties, 11, "Edit", SubFunctionalityEnum.Edit);
            var counterpartiesView = SubFunctionality(mb, counterparties, 12, "View", SubFunctionalityEnum.View);
            var counterpartiesAuditLogsView = SubFunctionality(mb, counterparties, 27, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var dealCategories = mb.HasData(new Functionality { Id = 5, FunctionalityEnum = FunctionalityEnum.DealCategories, Name = "Deal Categories" });
            var dealCategoriesCreate = SubFunctionality(mb, dealCategories, 13, "Create", SubFunctionalityEnum.Create);
            var dealCategoriesEdit = SubFunctionality(mb, dealCategories, 14, "Edit", SubFunctionalityEnum.Edit);
            var dealCategoriesView = SubFunctionality(mb, dealCategories, 15, "View", SubFunctionalityEnum.View);
            var dealCategoriesAuditLogsView = SubFunctionality(mb, dealCategories, 28, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var dealTypes = mb.HasData(new Functionality { Id = 6, FunctionalityEnum = FunctionalityEnum.DealTypes, Name = "Deal Types" });
            var dealTypesCreate = SubFunctionality(mb, dealTypes, 16, "Create", SubFunctionalityEnum.Create);
            var dealTypesEdit = SubFunctionality(mb, dealTypes, 17, "Edit", SubFunctionalityEnum.Edit);
            var dealTypesView = SubFunctionality(mb, dealTypes, 18, "View", SubFunctionalityEnum.View);
            var dealTypesAuditLogsView = SubFunctionality(mb, dealTypes, 29, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var itemFieldsets = mb.HasData(new Functionality { Id = 7, FunctionalityEnum = FunctionalityEnum.DealItemFieldsets, Name = "Deal Item Fieldsets" });
            var itemFieldsetsCreate = SubFunctionality(mb, itemFieldsets, 19, "Create", SubFunctionalityEnum.Create);
            var itemFieldsetsEdit = SubFunctionality(mb, itemFieldsets, 20, "Edit", SubFunctionalityEnum.Edit);
            var itemFieldsetsView = SubFunctionality(mb, itemFieldsets, 21, "View", SubFunctionalityEnum.View);
            var itemFieldsetsAuditLogsView = SubFunctionality(mb, itemFieldsets, 30, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var nodes = mb.HasData(new Functionality { Id = 8, FunctionalityEnum = FunctionalityEnum.Products, Name = "Products" });
            var nodesCreate = SubFunctionality(mb, nodes, 22, "Create", SubFunctionalityEnum.Create);
            var nodesEdit = SubFunctionality(mb, nodes, 23, "Edit", SubFunctionalityEnum.Edit);
            var nodesView = SubFunctionality(mb, nodes, 24, "View", SubFunctionalityEnum.View);
            var nodesAuditLogsView = SubFunctionality(mb, nodes, 31, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var dealSummaryList = mb.HasData(new Functionality { Id = 9, FunctionalityEnum = FunctionalityEnum.DealSummaryList, Name = "Deal Summary List" });

            var configuration = mb.HasData(new Functionality { Id = 12, FunctionalityEnum = FunctionalityEnum.Configuration, Name = "System Configuration" });
            var configurationEdit = SubFunctionality(mb, configuration, 39, "Edit", SubFunctionalityEnum.Edit);
            var configurationView = SubFunctionality(mb, configuration, 40, "View", SubFunctionalityEnum.View);
            var configurationAuditLogsView = SubFunctionality(mb, configuration, 41, "View Audit Logs", SubFunctionalityEnum.AuditLogsView);

            var roleId = mb.HasData(new UserRole { Id = 1, Name = "Trader/EPM/GMSG/FO" });
            var funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 1, UserRoleId = roleId, FunctionalityId = deal });
            mb.HasData(new SubFunctionalityInUserRole { Id = 1, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 2, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 3, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 4, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealPDFExport });
            mb.HasData(new SubFunctionalityInUserRole { Id = 61, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealAuditLogsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 84, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealExecute });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 2, UserRoleId = roleId, FunctionalityId = users });
            mb.HasData(new SubFunctionalityInUserRole { Id = 6, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 55, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 3, UserRoleId = roleId, FunctionalityId = counterparties });
            mb.HasData(new SubFunctionalityInUserRole { Id = 7, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 56, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 4, UserRoleId = roleId, FunctionalityId = dealCategories });
            mb.HasData(new SubFunctionalityInUserRole { Id = 8, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 57, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 5, UserRoleId = roleId, FunctionalityId = dealTypes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 9, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 58, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 6, UserRoleId = roleId, FunctionalityId = itemFieldsets });
            mb.HasData(new SubFunctionalityInUserRole { Id = 10, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 59, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 7, UserRoleId = roleId, FunctionalityId = nodes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 11, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 60, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 30, UserRoleId = roleId, FunctionalityId = dealSummaryList });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 41, UserRoleId = roleId, FunctionalityId = configuration });
            mb.HasData(new SubFunctionalityInUserRole { Id = 100, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 101, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationAuditLogsView });

            roleId = mb.HasData(new UserRole { Id = 2, Name = "MO", Description = "Middle Office" });
            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 8, UserRoleId = roleId, FunctionalityId = deal });
            mb.HasData(new SubFunctionalityInUserRole { Id = 12, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 13, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 14, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 15, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealPDFExport });
            mb.HasData(new SubFunctionalityInUserRole { Id = 62, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealAuditLogsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 85, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealExecute });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 9, UserRoleId = roleId, FunctionalityId = users });
            mb.HasData(new SubFunctionalityInUserRole { Id = 18, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 19, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 20, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 66, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 10, UserRoleId = roleId, FunctionalityId = counterparties });
            mb.HasData(new SubFunctionalityInUserRole { Id = 21, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 22, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 23, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 69, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 11, UserRoleId = roleId, FunctionalityId = dealCategories });
            mb.HasData(new SubFunctionalityInUserRole { Id = 24, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 25, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 26, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 72, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 12, UserRoleId = roleId, FunctionalityId = dealTypes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 27, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 28, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 29, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 75, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 13, UserRoleId = roleId, FunctionalityId = itemFieldsets });
            mb.HasData(new SubFunctionalityInUserRole { Id = 30, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 31, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 32, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 78, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 14, UserRoleId = roleId, FunctionalityId = nodes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 33, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 34, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 35, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesCreate });
            mb.HasData(new SubFunctionalityInUserRole { Id = 81, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 31, UserRoleId = roleId, FunctionalityId = dealSummaryList });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 42, UserRoleId = roleId, FunctionalityId = configuration });
            mb.HasData(new SubFunctionalityInUserRole { Id = 102, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 103, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 104, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationAuditLogsView });

            roleId = mb.HasData(new UserRole { Id = 3, Name = "Approver" });
            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 15, UserRoleId = roleId, FunctionalityId = deal });
            mb.HasData(new SubFunctionalityInUserRole { Id = 36, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 37, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 38, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealPDFExport });
            mb.HasData(new SubFunctionalityInUserRole { Id = 63, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 16, UserRoleId = roleId, FunctionalityId = users });
            mb.HasData(new SubFunctionalityInUserRole { Id = 39, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 67, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 17, UserRoleId = roleId, FunctionalityId = counterparties });
            mb.HasData(new SubFunctionalityInUserRole { Id = 40, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 70, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 18, UserRoleId = roleId, FunctionalityId = dealCategories });
            mb.HasData(new SubFunctionalityInUserRole { Id = 41, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 73, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 19, UserRoleId = roleId, FunctionalityId = dealTypes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 42, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 76, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 20, UserRoleId = roleId, FunctionalityId = itemFieldsets });
            mb.HasData(new SubFunctionalityInUserRole { Id = 43, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 79, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 21, UserRoleId = roleId, FunctionalityId = nodes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 44, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 82, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 32, UserRoleId = roleId, FunctionalityId = dealSummaryList });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 43, UserRoleId = roleId, FunctionalityId = configuration });
            mb.HasData(new SubFunctionalityInUserRole { Id = 105, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 106, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = configurationAuditLogsView });

            roleId = mb.HasData(new UserRole { Id = 4, Name = "BO", Description = "Back Office" });
            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 22, UserRoleId = roleId, FunctionalityId = deal });
            mb.HasData(new SubFunctionalityInUserRole { Id = 45, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 46, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealEdit });
            mb.HasData(new SubFunctionalityInUserRole { Id = 47, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealPDFExport });
            mb.HasData(new SubFunctionalityInUserRole { Id = 64, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 23, UserRoleId = roleId, FunctionalityId = users });
            mb.HasData(new SubFunctionalityInUserRole { Id = 48, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 68, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = usersAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 24, UserRoleId = roleId, FunctionalityId = counterparties });
            mb.HasData(new SubFunctionalityInUserRole { Id = 49, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 71, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = counterpartiesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 25, UserRoleId = roleId, FunctionalityId = dealCategories });
            mb.HasData(new SubFunctionalityInUserRole { Id = 50, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 74, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealCategoriesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 26, UserRoleId = roleId, FunctionalityId = dealTypes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 51, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 77, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealTypesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 27, UserRoleId = roleId, FunctionalityId = itemFieldsets });
            mb.HasData(new SubFunctionalityInUserRole { Id = 52, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 80, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = itemFieldsetsAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 28, UserRoleId = roleId, FunctionalityId = nodes });
            mb.HasData(new SubFunctionalityInUserRole { Id = 53, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 83, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = nodesAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 33, UserRoleId = roleId, FunctionalityId = dealSummaryList });

            roleId = mb.HasData(new UserRole { Id = 5, Name = "Other", Description = "Can only view deals" });
            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 29, UserRoleId = roleId, FunctionalityId = deal });
            mb.HasData(new SubFunctionalityInUserRole { Id = 54, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealView });
            mb.HasData(new SubFunctionalityInUserRole { Id = 65, FunctionalityInUserRoleId = funcInUser, SubFunctionalityId = dealAuditLogsView });

            funcInUser = mb.HasData(new FunctionalityInUserRole { Id = 34, UserRoleId = roleId, FunctionalityId = dealSummaryList });
        }

        private static int SubFunctionality(ModelBuilder mb, int funcId, int id, string name, SubFunctionalityEnum subEnum, int? parentSubFunc = null)
        {
            return mb.HasData(new SubFunctionality { FunctionalityId = funcId, Id = id, Name = name, SubFunctionalityEnum = subEnum, ParentSubFunctionalityId = parentSubFunc });
        }
        static int[] InternalTransferDealTypes = { 06 };
        static int[] ElectricityOTCPhysicalDealTypes = { 02 };
        static int[] ElectricityOTCHedgesDealTypes = { 04, 05, 24, 29, 30 };
        static int[] FTROptionDealTypes = { 22 };
        static int[] FTRObligationDealTypes = { 20 };
        static int[] GasPhysicalDealTypes = { 23 };
        static int[] CarbonDealTypes = { 16, 19 };

        private static void DealTypes(ModelBuilder mb)
        {
            // Internal Transfer
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityInternalTransferPolicyId,
                Id = 06,
                DealItemFieldsetId = itemFieldInternalTransfer,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "pc",
                Position = PositionEnum.Sell,
                Name = "Branch to Branch Sales",
                HasLossFactors = true,
                HasExpiryDate = true,
                CanChangeProductOnExecution = true,
                ItemExecutionImportTemplateType = DealItemExecutionImportTemplateTypeEnum.InternalSales,
            });
            
            // Electricity OTC Physical
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityPhysicalPolicyId,
                Id = 02,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "pc",
                Position = PositionEnum.Buy,
                Name = "General"
            });
            // Electricity OTC Hedges
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityDerivativesSwapsOTCTradesPolicyId,
                Id = 04,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "pc",
                Position = PositionEnum.Buy,
                Name = "Financial Future - Buy"
            });
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityDerivativesSwapsOTCTradesPolicyId,
                Id = 05,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Buy,
                Name = "Financial"
            });
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityOptionsBuyPolicyId,
                Id = 24,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Buy,
                Name = "Options - Buy",
                ForcePosition = true
            });
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityOptionsSellPolicyId,
                Id = 29,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Sell,
                Name = "Options - Sell",
                ForcePosition = true
            });
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityDerivativesSwapsOTCTradesPolicyId,
                Id = 30,
                DealItemFieldsetId = itemFieldSetElectricityGeneral,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Sell,
                Name = "Financial Futures - Sell"
            });

            // FTR Option
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityFTRsOptionsPolicyId,
                Id = 22,
                DealItemFieldsetId = itemFieldSetFTR,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Buy,
                Name = "Future Option",
                ItemExecutionImportTemplateType = DealItemExecutionImportTemplateTypeEnum.FTRs,
            });
            // FTR Obligation
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = ElectricityFTRsObligationsPolicyId,
                Id = 20,
                DealItemFieldsetId = itemFieldSetFTR,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "MW",
                Position = PositionEnum.Buy,
                Name = "Future Obligation",
                ItemExecutionImportTemplateType = DealItemExecutionImportTemplateTypeEnum.FTRs,
            });

            // Gas Physical
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = GasPhysicalPolicyId,
                Id = 23,
                DealItemFieldsetId = itemFieldSetGas,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "GJ",
                Position = PositionEnum.Buy,
                Name = "General - Retail"
            });

            // Carbon
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = CarbonExistingCarbonRightPolicyId,
                Id = 16,
                DealItemFieldsetId = itemFieldSetCarbon,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "TCO2e",
                Position = PositionEnum.Buy,
                Name = "Import - Buy",
                ForcePosition = true
            });
            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = CarbonExistingCarbonRightPolicyId,
                Id = 19,
                DealItemFieldsetId = itemFieldSetCarbon,
                WorkflowSetId = MainWorkflowSetId,
                Active = true,
                UnitOfMeasure = "TCO2e",
                Position = PositionEnum.Sell,
                Name = "Export - Sell",
                ForcePosition = true
            });

            mb.HasData(new DealType
            {
                TraderAuthorityPolicyId = GasPhysicalPolicyId,
                Id = AbcTradesDealType,
                DealItemFieldsetId = itemFieldSetABCTrades,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Active = true,
                Name = "ABC Gas Trade",
                HasDelegatedAuthority = true,
            });

            // no category
            mb.HasData(new DealType { Id = 01, DealItemFieldsetId = itemFieldSetElectricityGeneral, WorkflowSetId = MainWorkflowSetId, Active = false, UnitOfMeasure = "MW", Name = "ISDA" });
            mb.HasData(new DealType { Id = 03, DealItemFieldsetId = itemFieldSetElectricityGeneral, WorkflowSetId = MainWorkflowSetId, Active = false, UnitOfMeasure = "MW", Name = "Futures" });

            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 2 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 4 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 5 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 6 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 2, DealTypeId = 16 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 2, DealTypeId = 19 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 20 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 22 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 3, DealTypeId = 23 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 24 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 29 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = 1, DealTypeId = 30 });
            mb.HasData(new DealTypeInDealCategory { DealCategoryId = (int)DealCategoryEnum.Gas, DealTypeId = AbcTradesDealType });
        }

        private static void DealCategoriesAndCounterparties(ModelBuilder mb)
        {
            mb.HasData(new DealCategory
            {
                Id = (int)DealCategoryEnum.Electricity,
                Name = "General",
                UnitOfMeasure = "MW"
            });
            mb.HasData(new DealCategory
            {
                Id = (int)DealCategoryEnum.Carbon,
                Name = "Foreign Trade",
                UnitOfMeasure = null
            });
            mb.HasData(new DealCategory
            {
                Id = (int)DealCategoryEnum.Gas,
                Name = "Retail",
                UnitOfMeasure = "GJ"
            });
        }
        static int itemFieldSetElectricityGeneral = 1;
        static int itemFieldSetFTR = 2;
        static int itemFieldSetGas = 3;
        static int itemFieldSetCarbon = 4;
        static int itemFieldSetABCTrades = 5;
        static int itemFieldInternalTransfer = 6;

        private static void ItemFields(ModelBuilder mb)
        {
            int id = 0;
            id = mb.HasData(new DealItemFieldset { Id = itemFieldSetElectricityGeneral, Name = "General", Description = "Collection of fields for item input on deal types in general" });
            
            mb.HasData(new DealItemField { Id = 1, DealItemFieldsetId = id, DisplayOrder = 10, Name = "Product", Field = "ProductId" });
            mb.HasData(new DealItemField { Id = 5, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 6, DealItemFieldsetId = id, DisplayOrder = 60, Name = "Day Type", Field = "DayType" });
            mb.HasData(new DealItemField { Id = 7, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate", Execution = true });
            mb.HasData(new DealItemField { Id = 8, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate", Execution = true });
            mb.HasData(new DealItemField { Id = 9, DealItemFieldsetId = id, DisplayOrder = 90, Name = "TP Start", Field = "HalfHourTradingPeriodStart", Execution = true });
            mb.HasData(new DealItemField { Id = 10, DealItemFieldsetId = id, DisplayOrder = 100, Name = "TP End", Field = "HalfHourTradingPeriodEnd", Execution = true });
            mb.HasData(new DealItemField { Id = 11, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity", Execution = true });
            mb.HasData(new DealItemField { Id = 12, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price", Execution = true });
            mb.HasData(new DealItemField { Id = 13, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });

            id = mb.HasData(new DealItemFieldset { Id = itemFieldSetFTR, Name = "Futures", Description = "Collection of fields for item input on future deals" });
            
            mb.HasData(new DealItemField { Id = 17, DealItemFieldsetId = id, DisplayOrder = 20, Name = "Product", Field = "ProductId" });
            mb.HasData(new DealItemField { Id = 20, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 21, DealItemFieldsetId = id, DisplayOrder = 60, Name = "Day Type", Field = "DayType" });
            mb.HasData(new DealItemField { Id = 22, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate" });
            mb.HasData(new DealItemField { Id = 23, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate" });
            mb.HasData(new DealItemField { Id = 24, DealItemFieldsetId = id, DisplayOrder = 90, Name = "TP Start", Field = "HalfHourTradingPeriodStart" });
            mb.HasData(new DealItemField { Id = 25, DealItemFieldsetId = id, DisplayOrder = 100, Name = "TP End", Field = "HalfHourTradingPeriodEnd" });
            mb.HasData(new DealItemField { Id = 26, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity", Execution = true });
            mb.HasData(new DealItemField { Id = 27, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price", Execution = true });
            mb.HasData(new DealItemField { Id = 28, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });

            id = mb.HasData(new DealItemFieldset { Id = itemFieldSetGas, Name = "Retail", Description = "Collection of fields for item input on retail deals" });
            
            mb.HasData(new DealItemField { Id = 34, DealItemFieldsetId = id, DisplayOrder = 40, Name = "Product", Field = "ProductId" });
            mb.HasData(new DealItemField { Id = 35, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 37, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate", Execution = true });
            mb.HasData(new DealItemField { Id = 38, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate", Execution = true });
            mb.HasData(new DealItemField { Id = 39, DealItemFieldsetId = id, DisplayOrder = 90, Name = "TP Start", Field = "HalfHourTradingPeriodStart", Execution = true });
            mb.HasData(new DealItemField { Id = 40, DealItemFieldsetId = id, DisplayOrder = 100, Name = "TP End", Field = "HalfHourTradingPeriodEnd", Execution = true });
            mb.HasData(new DealItemField { Id = 41, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity", Execution = true });
            mb.HasData(new DealItemField { Id = 42, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price", Execution = true });
            mb.HasData(new DealItemField { Id = 43, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });
            mb.HasData(new DealItemField { Id = 44, DealItemFieldsetId = id, DisplayOrder = 120, Name = "Min", Field = "MinQuantity", Execution = true });
            mb.HasData(new DealItemField { Id = 45, DealItemFieldsetId = id, DisplayOrder = 130, Name = "Max", Field = "MaxQuantity", Execution = true });

            id = mb.HasData(new DealItemFieldset { Id = itemFieldSetCarbon, Name = "Foreign Trade", Description = "Collection of fields for item input on foreign trade deals" });

            mb.HasData(new DealItemField { Id = 49, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Product", Field = "ProductId" });
            mb.HasData(new DealItemField { Id = 50, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 52, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate", Execution = true });
            mb.HasData(new DealItemField { Id = 53, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate", Execution = true });
            mb.HasData(new DealItemField { Id = 56, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity", Execution = true });
            mb.HasData(new DealItemField { Id = 57, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price", Execution = true });
            mb.HasData(new DealItemField { Id = 58, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });

            id = mb.HasData(new DealItemFieldset { Id = itemFieldSetABCTrades, Name = "ABC trades", Description = "Collection of fields for item input on ABC trades" });

            mb.HasData(new DealItemField { Id = 59, DealItemFieldsetId = id, DisplayOrder = 40, Name = "Product", Field = "ProductId" });
            mb.HasData(new DealItemField { Id = 60, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 61, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate" });
            mb.HasData(new DealItemField { Id = 62, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate" });
            mb.HasData(new DealItemField { Id = 63, DealItemFieldsetId = id, DisplayOrder = 90, Name = "TP Start", Field = "HalfHourTradingPeriodStart" });
            mb.HasData(new DealItemField { Id = 64, DealItemFieldsetId = id, DisplayOrder = 100, Name = "TP End", Field = "HalfHourTradingPeriodEnd" });
            mb.HasData(new DealItemField { Id = 65, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity" });
            mb.HasData(new DealItemField { Id = 66, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price" });
            mb.HasData(new DealItemField { Id = 67, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });

            id = mb.HasData(new DealItemFieldset { Id = itemFieldInternalTransfer, Name = "Branch to branch sales", Description = "Collection of fields for branch to branch sales" });

            mb.HasData(new DealItemField { Id = 71, DealItemFieldsetId = id, DisplayOrder = 10, Name = "Product", Field = "ProductId", Execution = true });
            mb.HasData(new DealItemField { Id = 72, DealItemFieldsetId = id, DisplayOrder = 50, Name = "Position", Field = "Position" });
            mb.HasData(new DealItemField { Id = 73, DealItemFieldsetId = id, DisplayOrder = 60, Name = "Day Type", Field = "DayType" });
            mb.HasData(new DealItemField { Id = 74, DealItemFieldsetId = id, DisplayOrder = 70, Name = "Start Date", Field = "StartDate" });
            mb.HasData(new DealItemField { Id = 75, DealItemFieldsetId = id, DisplayOrder = 80, Name = "End Date", Field = "EndDate" });
            mb.HasData(new DealItemField { Id = 76, DealItemFieldsetId = id, DisplayOrder = 90, Name = "TP Start", Field = "HalfHourTradingPeriodStart" });
            mb.HasData(new DealItemField { Id = 77, DealItemFieldsetId = id, DisplayOrder = 100, Name = "TP End", Field = "HalfHourTradingPeriodEnd" });
            mb.HasData(new DealItemField { Id = 78, DealItemFieldsetId = id, DisplayOrder = 110, Name = "Qty", Field = "Quantity", Execution = true });
            mb.HasData(new DealItemField { Id = 79, DealItemFieldsetId = id, DisplayOrder = 140, Name = "Price", Field = "Price" });
            mb.HasData(new DealItemField { Id = 80, DealItemFieldsetId = id, DisplayOrder = 150, Name = "Criteria", Field = "Criteria" });
        }

        static int TraderLevel2WorkflowRole = 1;
        static int TraderLevel1WorkflowRole = 2;
        static int EPMWorkflowRole = 3;
        static int GMSGWorkflowRole = 4;
        static int CEOWorkflowRole = 5;
        static int BoardWorkflowRole = 6;

        static int ElectricityPhysicalPolicyId = 1;
        static int ElectricityInternalTransferPolicyId = 2;
        static int ElectricityDerivativesASXFutureTradesPolicyId = 3;
        static int ElectricityDerivativesSwapsOTCTradesPolicyId = 4;
        static int ElectricityOptionsBuyPolicyId = 5;
        static int ElectricityFTRsObligationsPolicyId = 6;
        static int ElectricityFTRsOptionsPolicyId = 7;
        static int GasPhysicalPolicyId = 8;
        static int CarbonExistingCarbonRightPolicyId = 9;
        static int ElectricityOptionsSellPolicyId = 10;

        static int AbcTradesDealType = 101;

        private static void TraderAuthorityPolicies(ModelBuilder mb)
        {
            int policy;

            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityPhysicalPolicyId, Name = "General" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 5
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 48,
                MaxTermInMonths = 36,
                MaxVolume = 15
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 48,
                MaxTermInMonths = 36,
                MaxVolume = 20
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 60,
                MaxTermInMonths = 48,
                MaxVolumeForecastPercentage = 0.20M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 1006,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 72,
                MaxTermInMonths = 60,
                MaxVolume = 10
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 1007, WorkflowRoleId = BoardWorkflowRole });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityInternalTransferPolicyId, Name = "General - branch to branch" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 5
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2002,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 12
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2003,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2004,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 45
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2005,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2006,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 50
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2007,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolumeForecastPercentage = 0.10M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2008,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 60
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2009,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 72,
                MaxTermInMonths = 60,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2010,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 48,
                MaxTermInMonths = 36,
                MaxVolumeForecastPercentage = 0.20M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 2011,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 6,
                MaxVolume = 60
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 2012, WorkflowRoleId = BoardWorkflowRole });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityDerivativesASXFutureTradesPolicyId, Name = "General - futures" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 3001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 51,
                MaxTermInMonths = 6,
                MaxBuyVolume = 12,
                MaxSellVolume = 12
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 3002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 51,
                MaxTermInMonths = 12,
                MaxBuyVolume = 25,
                MaxSellVolume = 25
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 3003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 51,
                MaxTermInMonths = 18,
                MaxBuyVolume = 40,
                MaxSellVolume = 40
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 3004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 51,
                MaxTermInMonths = 24,
                MaxBuyVolume = 50,
                MaxSellVolume = 50
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 3005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 51,
                MaxTermInMonths = 51,
                MaxBuyVolume = 100,
                MaxSellVolume = 100
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 3007, WorkflowRoleId = BoardWorkflowRole });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityDerivativesSwapsOTCTradesPolicyId, Name = "General - swaps" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 5
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4002,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 12
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4003,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4004,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 45
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4005,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4006,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 50
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4007,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolumeForecastPercentage = 0.10M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4008,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 60
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4009,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 72,
                MaxTermInMonths = 60,
                MaxVolume = 10
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4010,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 48,
                MaxTermInMonths = 36,
                MaxVolumeForecastPercentage = 0.20M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 4011,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 6,
                MaxVolume = 60
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 4012, WorkflowRoleId = BoardWorkflowRole });

            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityOptionsBuyPolicyId, Name = "General - import" });
            // TODO: SELL
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxAcquisitionCost = 500_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxAcquisitionCost = 1_000_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxAcquisitionCost = 1_500_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxAcquisitionCost = 2_000_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxAcquisitionCost = 5_000_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 5007,
                WorkflowRoleId = BoardWorkflowRole,
                OnlyBuy = true
            });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityFTRsObligationsPolicyId, Name = "General - future obligations" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 6001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 1,
                MaxVolume = 5
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 6002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 1,
                MaxVolume = 10
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 6003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 1,
                MaxVolume = 15
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 6004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 1,
                MaxVolume = 20
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 6005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 1,
                MaxVolume = 40
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 6007, WorkflowRoleId = BoardWorkflowRole });

            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityFTRsOptionsPolicyId, Name = "General - future options" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 7001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxAcquisitionCost = 1_000_000,
                OnlyBuy = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 7002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxAcquisitionCost = 2_000_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 7003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxAcquisitionCost = 3_000_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 7004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxAcquisitionCost = 4_000_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 7005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxAcquisitionCost = 5_000_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 7007, WorkflowRoleId = BoardWorkflowRole });

            policy = mb.HasData(new TraderAuthorityPolicy { Id = GasPhysicalPolicyId, Name = "Retail" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 8001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxDurationInMonths = 12,
                MaxTermInMonths = 6,
                MaxBuyVolume = 3000,
                MaxSellVolume = 3000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 8002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxDurationInMonths = 12,
                MaxTermInMonths = 6,
                MaxBuyVolume = 5000,
                MaxSellVolume = 5000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 8003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 24,
                MaxTermInMonths = 12,
                MaxBuyVolume = 6000,
                MaxSellVolume = 6000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 8004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxBuyVolume = 7000,
                MaxSellVolume = 7000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 8005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 60,
                MaxTermInMonths = 48,
                MaxBuyVolume = 10000,
                MaxSellVolume = 10000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 8007, WorkflowRoleId = BoardWorkflowRole });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = CarbonExistingCarbonRightPolicyId, Name = "Foreign Trade" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 9001,
                WorkflowRoleId = TraderLevel2WorkflowRole,
                MaxBuyVolume = 0,
                MaxSellVolume = 25_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 9002,
                WorkflowRoleId = TraderLevel1WorkflowRole,
                MaxBuyVolume = 0,
                MaxSellVolume = 50_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 9003,
                WorkflowRoleId = EPMWorkflowRole,
                MaxBuyVolume = 0,
                MaxSellVolume = 75_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 9004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxBuyVolume = 0,
                MaxSellVolume = 100_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 9005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxBuyVolume = 100_000,
                MaxSellVolume = 150_000
            });

            mb.HasData(new TraderAuthorityPolicyCriteria { TraderAuthorityPolicyId = policy, Id = 9007, WorkflowRoleId = BoardWorkflowRole });


            policy = mb.HasData(new TraderAuthorityPolicy { Id = ElectricityOptionsSellPolicyId, Name = "General - options sell" });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10001,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolume = 10,
                OnlySell = true
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10002,
                WorkflowRoleId = EPMWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 50,
                OnlySell = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10003,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 24,
                MaxVolumeForecastPercentage = 0.10M
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10004,
                WorkflowRoleId = GMSGWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 3,
                MaxVolume = 60,
                OnlySell = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10005,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 72,
                MaxTermInMonths = 60,
                MaxVolume = 10,
                OnlySell = true
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10006,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 48,
                MaxTermInMonths = 36,
                MaxVolumeForecastPercentage = 0.20M,
                OnlySell = true
            });
            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10007,
                WorkflowRoleId = CEOWorkflowRole,
                MaxDurationInMonths = 36,
                MaxTermInMonths = 6,
                MaxVolume = 60,
                OnlySell = true
            });

            mb.HasData(new TraderAuthorityPolicyCriteria
            {
                TraderAuthorityPolicyId = policy,
                Id = 10008,
                WorkflowRoleId = BoardWorkflowRole,
                OnlySell = true
            });
        }
        const int MainWorkflowSetId = 1;
        const int preExecutedDealsWorkflowSetId = 2;
        const int middleOfficeRoleId = 7;
        const int backOfficeRoleId = 8;
        private static void Workflow(ModelBuilder mb)
        {
            mb.HasData(new WorkflowRole { Id = TraderLevel2WorkflowRole, Name = "Junior Trader", ApprovalLevel = 10 });
            mb.HasData(new WorkflowRole { Id = TraderLevel1WorkflowRole, Name = "Trader", ApprovalLevel = 20 });
            mb.HasData(new WorkflowRole { Id = EPMWorkflowRole, Name = "Senior Trader", ApprovalLevel = 30 });
            mb.HasData(new WorkflowRole { Id = GMSGWorkflowRole, Name = "General Manager", ApprovalLevel = 40 });
            mb.HasData(new WorkflowRole { Id = CEOWorkflowRole, Name = "CEO", ApprovalLevel = 50 });
            mb.HasData(new WorkflowRole { Id = BoardWorkflowRole, Name = "Board", ApprovalLevel = 60 });

            mb.HasData(new WorkflowRole { Id = middleOfficeRoleId, Name = "Compliance Office" });
            mb.HasData(new WorkflowRole { Id = backOfficeRoleId, Name = "Accounting" });

            mb.HasData(new WorkflowSet { Id = MainWorkflowSetId, Name = "Main Workflow", Description = "This workflow is used by the majority of the deal types" });
            mb.HasData(new WorkflowSet { Id = preExecutedDealsWorkflowSetId, Name = "Pre-executed Trades", Description = "This workflow is meant to be used by deals where the trade has been already executed before submission." });

            DealTypes(mb);
            MainWorkflowStatuses(mb);
            PreExecutedWorkflowStatuses(mb);
        }

        const int preExecutedDealsSubmitActionId = 160;
        static void PreExecutedWorkflowStatuses(ModelBuilder mb)
        {
            var entered = mb.HasData(new WorkflowStatus
            {
                Id = 110,
                Order = 10,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Name = "Entered",
                AssignmentType = WorkflowAssignmentTypeEnum.DealTrader,
                AllowsDealEditing = true,
                AllowsEditDelegatedAuthority = true,
            });

            var submitted = mb.HasData(new WorkflowStatus
            {
                Id = 120,
                Order = 20,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Name = "Submitted",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId,
                AllowsEditDelegatedAuthority = true,
            });

            var checkedMO = mb.HasData(new WorkflowStatus
            {
                Id = 130,
                Order = 30,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Name = "Checked by Compliance Office",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = backOfficeRoleId
            });

            var completed = mb.HasData(new WorkflowStatus
            {
                Id = 140,
                Order = 40,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Name = "Completed",
                FinalizeDeal = true,
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId,
            });

            var cancelled = mb.HasData(new WorkflowStatus
            {
                Id = 150,
                Order = 50,
                WorkflowSetId = preExecutedDealsWorkflowSetId,
                Name = "Cancelled",
                FinalizeDeal = true,
                CancelDeal = true,
                AssignmentType = WorkflowAssignmentTypeEnum.DealTrader
            });
            
            var submitAction = mb.HasData(new WorkflowAction
            {
                Id = preExecutedDealsSubmitActionId,
                SourceWorkflowStatusId = entered,
                TargetWorkflowStatusId = submitted,
                Name = "Submit",
                Description = "Submits the deal for Middle Office validation.",
                IsSubmission = true,
                PerformsExecutionAutomatically = true,
            });

            var checkMOAction = mb.HasData(new WorkflowAction
            {
                Id = 170,
                Name = "Check - Middle Office",
                Description = "Performs the Middle Office checks, and sends the deal for Back Office checking.",
                SourceWorkflowStatusId = submitted,
                TargetWorkflowStatusId = checkedMO
            });

            int answerYes, answerNo;

            (answerYes, answerNo) = AddMandatoryTaskWithTraderLimitsQuestion(
                null, mb, checkMOAction, 1010, null, "Are all trades within respective Trader Authority levels?",
                Combine(AbcTradesDealType));
            AddMandatoryTask(null, mb, checkMOAction, 1020, answerNo, "Please investigate/report breach", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(AbcTradesDealType));


            answerYes = AddMandatoryTaskWithYes(null, mb, checkMOAction, 1030, null, "Has Hedge Contracts Database been updated?",
                Combine(AbcTradesDealType));
            AddMandatoryTask(null, mb, checkMOAction, 1040, answerYes, "Date", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(AbcTradesDealType));
            AddMandatoryTask(null, mb, checkMOAction, 1050, answerYes, "Contract IDs", WorkflowTaskTypeEnum.EnterMultipleInformation,
                Combine(AbcTradesDealType));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, checkMOAction, 1060, null, "ABC deal notification attached?",
                abcDealNotificationsAttachmentType,
                Combine(AbcTradesDealType));
            AddMandatoryTask(null, mb, checkMOAction, 1070, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(AbcTradesDealType));

            AddMandatoryTask(null, mb, checkMOAction, 1080, null, "Produce Summary report", WorkflowTaskTypeEnum.SimpleCheck,
                Combine(AbcTradesDealType));

            AddMandatoryTask(null, mb, checkMOAction, 1090, null, "Produce Daily report", WorkflowTaskTypeEnum.SimpleCheck,
                Combine(AbcTradesDealType));

            var rejectMOAction = mb.HasData(new WorkflowAction
            {
                Id = 180,
                Name = "Revert",
                Description = "Sends deal back to the trader.",
                SourceWorkflowStatusId = submitted,
                TargetWorkflowStatusId = entered,
                TargetAlternateDescriptionSuffix = "Reversed By Compliance Office Checks"
            });

            AddMandatoryTask(null, mb, rejectMOAction, 1100, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);

            var boChecksAction = mb.HasData(new WorkflowAction
            {
                Id = 190,
                Description = "Performs the Accounting checks, completing/finalising the deal.",
                Name = "Check - Back Office",
                SourceWorkflowStatusId = checkedMO,
                TargetWorkflowStatusId = completed
            });

            AddMandatoryTask(null, mb, boChecksAction, 1110, null, "Does this deal pass all Accounting checks?", WorkflowTaskTypeEnum.SimpleCheck);

            var rejectBOAction = mb.HasData(new WorkflowAction
            {
                Id = 200,
                Name = "Revert",
                Description = "Sends deal back to Compliance Office.",
                SourceWorkflowStatusId = checkedMO,
                TargetWorkflowStatusId = submitted,
                TargetAlternateDescriptionSuffix = "Reversed By Accounting Checks"
            });

            AddMandatoryTask(null, mb, rejectBOAction, 1120, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);

            var reopenAction = mb.HasData(new WorkflowAction
            {
                Id = 210,
                Name = "Reopen",
                Description = "Reopens a completed deal.",
                SourceWorkflowStatusId = completed,
                TargetWorkflowStatusId = checkedMO,
                TargetAlternateDescriptionSuffix = "Reopened"
            });

            AddMandatoryTask(null, mb, reopenAction, 1130, null, "Provide reason for reopening deal", WorkflowTaskTypeEnum.EnterTextInformation);

            var cancelAction = mb.HasData(new WorkflowAction
            {
                Id = 220,
                Name = "Cancel",
                Description = "Cancels a deal permanently.",
                SourceWorkflowStatusId = null,
                TargetWorkflowStatusId = cancelled,
            });

            AddMandatoryTask(null, mb, cancelAction, 1140, null, "Provide reason for cancelling deal", WorkflowTaskTypeEnum.EnterTextInformation);
        }

        static void MainWorkflowStatuses(ModelBuilder mb)
        {
            var entered = mb.HasData(new WorkflowStatus
            {
                Id = 1,
                Order = 10,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Entered",
                AssignmentType = WorkflowAssignmentTypeEnum.DealTrader,
                AllowsDealEditing = true
            });

            var submitted = mb.HasData(new WorkflowStatus
            {
                Id = 2,
                Order = 20,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Submitted",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId
            });

            var validated = mb.HasData(new WorkflowStatus
            {
                Id = 3,
                Order = 30,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Validated",
                AssignmentType = WorkflowAssignmentTypeEnum.ApprovalLevelSelectionEqualHigher
            });

            var approved = mb.HasData(new WorkflowStatus
            {
                Id = 4,
                Order = 40,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Approved",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId,
                AllowsDealExecution = true,
            });

            mb.HasData(new WorkflowStatus
            {
                Id = 5,
                Order = 50,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Executed (Deprecated)",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId
            });

            var checkedMO = mb.HasData(new WorkflowStatus
            {
                Id = 6,
                Order = 60,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Checked by Compliance Office",
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = backOfficeRoleId
            });

            var completed = mb.HasData(new WorkflowStatus
            {
                Id = 7,
                Order = 70,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Completed",
                FinalizeDeal = true,
                AssignmentType = WorkflowAssignmentTypeEnum.PredefinedApprovalLevel,
                WorkflowRoleId = middleOfficeRoleId,
            });

            var cancelled = mb.HasData(new WorkflowStatus
            {
                Id = 8,
                Order = 80,
                WorkflowSetId = MainWorkflowSetId,
                Name = "Cancelled",
                FinalizeDeal = true,
                CancelDeal = true,
                AssignmentType = WorkflowAssignmentTypeEnum.DealTrader
            });

            var action = 0;
            var answerNo = 0;
            var answerYes = 0;
            var answerNotApplicable = 0;
            action = mb.HasData(new WorkflowAction
            {
                Id = 1,
                SourceWorkflowStatusId = entered,
                TargetWorkflowStatusId = submitted,
                Name = "Submit",
                Description = "Submits the deal for Compliance validation.",
                IsSubmission = true,
            });

            AddMandatoryTask(null, mb, action, 400, null, "Does the deal have at least one dealItem?", WorkflowTaskTypeEnum.HasItems);
            AddOptionalTask(null, mb, action, 401, null, "Was a note created on this deal?", WorkflowTaskTypeEnum.CreatedNoteDuringStatus);
            AddOptionalTask(null, mb, action, 402, null, "Was a document attached to this deal?", WorkflowTaskTypeEnum.AttachedDocumentDuringStatus);
            AddMandatoryTask(null, mb, action, 1, null, "Was the Deal Expiry Date recorded?", WorkflowTaskTypeEnum.ExpiryDateCheck, InternalTransferDealTypes);

            (var yesIFRSAccounted, var noIFRSAccounted) = AddMandatoryTaskWithYesNo(null, mb, action, 2, null, "Is IFRS accounted?", ElectricityOTCHedgesDealTypes);
            AddInactiveMandatoryTask(null, mb, action, 3, noIFRSAccounted, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation, ElectricityOTCHedgesDealTypes);

            action = mb.HasData(new WorkflowAction
            {
                Id = 2,
                Name = "Validate",
                Description = "Validates the deal in regards to compliance concerns, so the deal can proceed for approval.",
                SourceWorkflowStatusId = submitted,
                TargetWorkflowStatusId = validated,
                CantBePerformedBySameUser = true,
            });

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 4, null, "Does the deal meet the current trading stategy?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 5, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 6, null, "Is CFaR/Effective Length test required?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 7, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 8, answerYes, "Within governance limits?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 9, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 10, null, "Are Long Term Limits checks required?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 11, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 12, answerYes, "Within governance limits?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 13, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 14, null, "Is Market test required?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 15, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 16, answerYes, "Within governance limits?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 17, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 18, null, "Is Stress Limits testing required?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 19, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 20, answerYes, "Within governance limits?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 21, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 22, null, "Is price path check required?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 23, answerYes, "Please comment:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 24, answerNo, "Please comment:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 25, null, "Is the counterparty verified?",
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 26, answerNo, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, CarbonDealTypes));

            (var yesLegalOverviewPPA, var noLegalOverviewPPA) = AddMandatoryTaskWithYesNo(null, mb, action, 27, null, "Will legal overview of PPA be required?",
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 28, noLegalOverviewPPA, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 29, null, "Is proposed peak exposure within approved limit?",
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 30, answerYes, "Approved limit", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 31, answerNo, "Approved limit", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 32, answerNo, "Mandatory Note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (var firstAnswer, var secAnswer) = AddMandatoryTaskWithTwoAnswers(null, mb, action, 33, null, "Will deal be under ISDA or Long Form Confirmation (LFC)?"
                , "ISDA", "LFC",
                Combine(ElectricityOTCHedgesDealTypes));
            (var yesISDAHeld, var noISDAHeld) = AddMandatoryTaskWithYesNo(null, mb, action, 34, firstAnswer, "ISDA Held?",
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 35, yesISDAHeld, "ISDA Date", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 36, yesISDAHeld, "LEX ID", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (var yesLegalOverviewDocs, var noLegalOverviewDocs) = AddMandatoryTaskWithYesNo(null, mb, action, 37, null, "Will legal overview of documents be required?",
                Combine(ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes, CarbonDealTypes));

            (answerYes, answerNo) = AddInactiveMandatoryTaskWithYesNo(null, mb, action, 38, null, "Will deal be IFRS accounted?",
                Combine(ElectricityOTCHedgesDealTypes));
            (var yesMarketSettled, var noMarketSettled) = AddMandatoryTaskWithYesNo(null, mb, action, 39, null, "Will deal be Market settled?",
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 40, null, "Have Acquisition Cost checks been completed & match FO Bid File?",
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 41, answerNo, "Please explain:", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (var yesSellDeal, var noSellDeal) = AddMandatoryTaskWithYesNo(null, mb, action, 300, null, "Is this a sell deal?",
                Combine(GasPhysicalDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 42, yesSellDeal, "Is proposed exposure within approved limit?",
                Combine(GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 43, answerYes, "Approved limit", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 44, answerNo, "Approved limit", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 45, answerNo, "Mandatory Note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes));

            var rejectValidationActionId = 3;
            (firstAnswer, secAnswer) = AddMandatoryTaskWithTwoAnswers(null, mb, action, 46, null, "Are we buying or selling?", "Buying", "Selling",
                Combine(CarbonDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 47, secAnswer, "Is the deal value (Price x Quantity) less than or equal to the Approved Limit?", rejectValidationActionId,
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 48, answerNo, "Mandatory Note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(CarbonDealTypes));
            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 49, answerYes, "Are there sufficient NZU/AAU stocks on hand?", rejectValidationActionId,
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 50, answerNo, "Mandatory Note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(CarbonDealTypes));

            action = mb.HasData(new WorkflowAction
            {
                Id = rejectValidationActionId,
                Name = "Revert",
                Description = "Sends deal back to the trader.",
                SourceWorkflowStatusId = submitted,
                TargetWorkflowStatusId = entered,
                TargetAlternateDescriptionSuffix = "Reverted - Validation"
            });

            AddMandatoryTask(null, mb, action, 51, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);

            action = mb.HasData(new WorkflowAction
            {
                Id = 4,
                Name = "Approve",
                Description = "Approves the deal so it can be executed.",
                SourceWorkflowStatusId = validated,
                TargetWorkflowStatusId = approved,
                DirectActionOnEmailNotification = true,
                CantBePerformedBySameUser = true,
            });

            action = mb.HasData(new WorkflowAction
            {
                Id = 5,
                Name = "Revert",
                Description = "Sends deal back to Compliance Office.",
                SourceWorkflowStatusId = validated,
                TargetWorkflowStatusId = submitted,
                TargetAlternateDescriptionSuffix = "Reverted - Approval",
                DirectActionOnEmailNotification = true,
            });

            AddMandatoryTask(null, mb, action, 52, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);

            action = mb.HasData(new WorkflowAction
            {
                Id = 6,
                Name = "Execute (deprecated)",
                Description = "Executes the deal, sending it to Compliance Office checking.",
                TargetWorkflowStatusId = entered,
                Active = false,
            });

            action = mb.HasData(new WorkflowAction
            {
                Id = 7,
                Name = "Cancel Execution (deprecated)",
                Description = "Cancels the execution, sending it back to Compliance Office.",
                TargetWorkflowStatusId = entered,
                Active = false,
            });
            action = mb.HasData(new WorkflowAction
            {
                Id = 8,
                Name = "Check - Compliance Office",
                Description = "Performs the Compliance Office checks, and sends the deal for Back Office checking.",
                SourceWorkflowStatusId = approved,
                TargetWorkflowStatusId = checkedMO
            });

            AddMandatoryTask(null, mb, action, 302, null, "Ensure deal is executed", WorkflowTaskTypeEnum.DealExecutedCheck);

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 54, null, "Deal Approval Notice sent?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 55, answerYes, "Date it was sent", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 56, answerNo, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 57, null, "7 Day Notice sent?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 58, answerYes, "Date it was sent", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 59, answerNo, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 60, null, "Final/Expiry Notice sent?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 61, answerYes, "Date it was sent", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 62, answerNo, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNotApplicable) = AddMandatoryTaskWithYesNotApplicable(null, mb, action, 63, null, "Interim Transfer Volumes received from Commercial Sales Manager?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 64, answerYes, "Date it was received", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 65, answerYes, "Month/Year data is for", WorkflowTaskTypeEnum.EnterMonthAndYearInformation,
                Combine(InternalTransferDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 66, null, "Final Transfer Volumes received from Commercial Sales Manager?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 67, answerYes, "Date it was received", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 68, null, "Are Final Transfer Volumes within approved limits?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 69, answerNo, "Mandatory note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 70, answerNo, "Record in Breach db, and report.", WorkflowTaskTypeEnum.SimpleCheck,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 71, null, "Is EA Disclosure required for any deals?",
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 72, answerYes, "Obtain details from Commercial Sales Manager.", WorkflowTaskTypeEnum.SimpleCheck,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 73, answerNo, "Has confirmation been received from Commercial Sales Manager?", WorkflowTaskTypeEnum.SimpleCheck,
                Combine(InternalTransferDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 74, null, "Has Hedge Contracts Database been updated?",
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 75, answerYes, "Date", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 76, answerYes, "Contract IDs", WorkflowTaskTypeEnum.EnterMultipleInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 77, null, "Pricing Summary attached?",
                pricingSummaryAttachmentType,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 78, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 79, null, "Validation Summary attached?",
                validationSummaryAttachmentType,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 80, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes, ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 81, null, "Interim Volumes attached?",
                interimVolumesAttachmentType,
                Combine(InternalTransferDealTypes));
            AddMandatoryTask(null, mb, action, 82, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(InternalTransferDealTypes));

            answerYes = AddMandatoryTaskWithYesWithAttachmentVerification(null, mb, action, 83, null, "Final Volumes attached?",
                finalVolumesAttachmentType,
                Combine(InternalTransferDealTypes));

            int rejectMiddleOfficeCheckActionId = 9;
            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 85, null, "Do terms of signed PPA match DealSystem?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 86, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 87, null, "Has the PPA been signed in terms of the Trading Guidelines?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 88, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 89, null, "Has PPA been filed in LEX?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 90, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 91, answerYes, "Date Actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 92, answerYes, "LEX ID", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(yesLegalOverviewPPA, mb, action, 93, null, "Is satisfactory legal opinion held?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 94, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 95, null, "Is EA Disclosure/Verification required?",
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTaskWithTwoAnswers(null, mb, action, 96, answerYes, "Disclosed or Verified", "Disclosed", "Verified",
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 97, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 98, answerYes, "Disclosure ID", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 99, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 100, null, "Is a new settlement process required?",
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 101, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 102, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 103, null, "Signed PPA attached?",
                signedPPAAttachmentType,
                Combine(ElectricityOTCPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 104, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 105, null, "Solicitor's Certificate attached?",
                solicitorsCertificateAttachmentType,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 106, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCPhysicalDealTypes, ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 107, null, "Do terms of signed Confirmation match DealSystem?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 108, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(noISDAHeld, mb, action, 109, null, "Is signed ISDA to hand and in order?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 110, answerYes, "ISDA Date", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 111, answerYes, "LEX ID", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 112, null, "Has the Confirmation been signed in terms of the parties' respective Trading Guidelines?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 113, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 114, null, "Have AML and FMCA requirements been satisfied?",
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 115, answerYes, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 116, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(yesLegalOverviewDocs, mb, action, 117, null, "Is satisfactory legal opinion held?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 118, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes, GasPhysicalDealTypes, CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(yesMarketSettled, mb, action, 119, null, "Has Hedge Settlement Agreement (HSA) been sent to and confirmed by Clearing Manager?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 120, answerYes, "Date received from Clearing Manager", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 121, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(yesIFRSAccounted, mb, action, 122, null, "Has IFRS documentation been completed by Front Office & deal included in Hedge Valuation report?",
                rejectMiddleOfficeCheckActionId,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 123, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 124, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNo(null, mb, action, 125, null, "Is counterparty included in the Counterparty Exposure report?",
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 126, answerNo, "Why not?", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 127, null, "Counterparty Exposure attached?",
                counterpartyExposureAttachmentType,
                Combine(ElectricityOTCHedgesDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 128, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes, CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 129, null, "Signed Confirmation attached?",
                signedConfirmationAttachmentType,
                Combine(ElectricityOTCHedgesDealTypes));
            AddMandatoryTask(null, mb, action, 130, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(ElectricityOTCHedgesDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 131, null, "CSV files downloaded from ABC website",
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 132, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 133, null, "Has the Hedge Contracts Database been updated?",
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 134, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 135, null, "Have the contracts awarded vs bid/approved checks been completed?",
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 136, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 137, answerYes, "Mandatory note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 138, null, "Bid File attached?",
                bidFileAttachmentType,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 139, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 140, null, "Validation & Results Check Workbook attached?",
                validationResultsCheckWorkbookAttachmentType,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 141, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 142, null, "FTR CSV Files attached?",
                FTRCSVFileAttachmentType,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 143, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 144, null, "Contracts Awarded Summary attached?",
                contractsAwardedSummaryAttachmentType,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            var spotCheckYes = AddMandatoryTaskWithYes(null, mb, action, 145, answerYes, "Did you spot check the summary against the CSV files?",
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 146, spotCheckYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 147, spotCheckYes, "Mandatory note", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));
            AddMandatoryTask(null, mb, action, 148, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(FTROptionDealTypes, FTRObligationDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 149, null, "Do terms of signed Contract match DealSystem?",
                rejectMiddleOfficeCheckActionId,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 150, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithNoCausingAction(null, mb, action, 151, null, "Has the contract been signed in terms of the Trading Guidelines?",
                rejectMiddleOfficeCheckActionId,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 152, answerYes, "LEX ID", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 153, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(yesSellDeal, mb, action, 154, null, "Counterparty Exposure (sell deals) Attached?",
                counterpartyExposureAttachmentType,
                Combine(GasPhysicalDealTypes));
            AddMandatoryTask(null, mb, action, 155, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 156, null, "Signed Contract Attached?",
                signedContractAttachmentType,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 157, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(GasPhysicalDealTypes, CarbonDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 158, null, "Has transfer via NZEUR been completed?",
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 159, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(CarbonDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 160, null, "Has the Carbon Stocks Register been updated?",
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 161, answerYes, "Date actioned", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(CarbonDealTypes));

            answerYes = AddMandatoryTaskWithYes(null, mb, action, 162, null, "Has settlement occurred?",
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 163, answerYes, "Settlement Date", WorkflowTaskTypeEnum.EnterDateInformation,
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 164, answerYes, "Invoice Number", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 165, null, "Contract Note Attached?",
                contractNoteAttachmentType,
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 166, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(CarbonDealTypes));

            (answerYes, answerNo) = AddMandatoryTaskWithYesNoWithAttachmentVerification(null, mb, action, 167, null, "Invoice Attached?",
                invoiceAttachmentType,
                Combine(CarbonDealTypes));
            AddMandatoryTask(null, mb, action, 168, answerNo, "Please explain", WorkflowTaskTypeEnum.EnterTextInformation,
                Combine(CarbonDealTypes));


            action = mb.HasData(new WorkflowAction
            {
                Id = rejectMiddleOfficeCheckActionId,
                Name = "Revert",
                Description = "Sends deal back to the trader in an entered state.",
                SourceWorkflowStatusId = approved,
                TargetWorkflowStatusId = entered,
                TargetAlternateDescriptionSuffix = "Reversed By Compliance Office Checks"
            });

            AddMandatoryTask(null, mb, action, 169, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);
            AddMandatoryTask(null, mb, action, 301, null, "Ensure deal is not executed or had its execution cancelled", WorkflowTaskTypeEnum.DealNotExecutedCheck);

            action = mb.HasData(new WorkflowAction
            {
                Id = 10,
                Description = "Performs the Accounting checks, completing/finalising the deal.",
                Name = "Check - Accounting",
                SourceWorkflowStatusId = checkedMO,
                TargetWorkflowStatusId = completed
            });

            AddMandatoryTask(null, mb, action, 170, null, "Does this deal pass all Accounting checks?", WorkflowTaskTypeEnum.SimpleCheck);
            AddMandatoryInactiveTask(null, mb, action, 171, null, "Check B.O. # 2", WorkflowTaskTypeEnum.SimpleCheck);
            AddMandatoryInactiveTask(null, mb, action, 172, null, "Check B.O. # 3", WorkflowTaskTypeEnum.SimpleCheck);

            action = mb.HasData(new WorkflowAction
            {
                Id = 11,
                Name = "Revert",
                Description = "Sends deal back to Compliance Office.",
                SourceWorkflowStatusId = checkedMO,
                TargetWorkflowStatusId = approved,
                TargetAlternateDescriptionSuffix = "Reversed By Accounting Checks"
            });

            AddMandatoryTask(null, mb, action, 173, null, "Provide reason for reversion", WorkflowTaskTypeEnum.EnterTextInformation);

            action = mb.HasData(new WorkflowAction
            {
                Id = 12,
                Name = "Reopen",
                Description = "Reopens a completed deal.",
                SourceWorkflowStatusId = completed,
                TargetWorkflowStatusId = checkedMO,
                TargetAlternateDescriptionSuffix = "Reopened"
            });

            AddMandatoryTask(null, mb, action, 174, null, "Provide reason for reopening deal", WorkflowTaskTypeEnum.EnterTextInformation);

            action = mb.HasData(new WorkflowAction
            {
                Id = 13,
                Name = "Cancel",
                Description = "Cancels a deal permanently.",
                SourceWorkflowStatusId = null,
                TargetWorkflowStatusId = cancelled,
            });

            AddMandatoryTask(null, mb, action, 175, null, "Provide reason for cancelling deal", WorkflowTaskTypeEnum.EnterTextInformation);
            AddMandatoryTask(null, mb, action, 500, null, "Ensure deal is not executed or had its execution cancelled", WorkflowTaskTypeEnum.DealNotExecutedCheck);
        }

        private static void AddOptionalTask(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, WorkflowTaskTypeEnum type, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, type, precedingAnswerId, mandatory: false);
            AddDealTypeToTask(mb, taskId, dealTypes);
        }
        private static void AddInactiveMandatoryTask(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, WorkflowTaskTypeEnum type, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, type, precedingAnswerId, active: false);
            AddDealTypeToTask(mb, taskId, dealTypes);
        }
        private static void AddMandatoryTask(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, WorkflowTaskTypeEnum type, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, type, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);
        }
        private static void AddMandatoryInactiveTask(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, WorkflowTaskTypeEnum type, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, type, precedingAnswerId, active: false);
            AddDealTypeToTask(mb, taskId, dealTypes);
        }

        private static (int, int, int) AddMandatoryTaskWithYesNoNotApplicable(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;
            int naId = noId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No");
            var naAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "N/A");
            return (yesAnswer, noAnswer, naAnswer);
        }

        private static (int, int) AddMandatoryTaskWithYesNotApplicable(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;
            int naId = noId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var naAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "N/A");
            return (yesAnswer, naAnswer);
        }

        private static int AddMandatoryTaskWithYes(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            return yesAnswer;
        }

        private static (int, int) AddMandatoryTaskWithYesNoWithAttachmentVerification(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, int attachmentTypeId, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes", attachmentTypeId);
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No");
            return (yesAnswer, noAnswer);
        }

        private static int AddMandatoryTaskWithYesWithAttachmentVerification(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, int attachmentTypeId, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes", attachmentTypeId);

            return yesAnswer;
        }

        private static (int, int) AddInactiveMandatoryTaskWithYesNo(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId, active: false);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No");
            return (yesAnswer, noAnswer);
        }

        private static (int, int) AddMandatoryTaskWithYesNo(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No");
            return (yesAnswer, noAnswer);
        }

        private static (int, int) AddMandatoryTaskWithTraderLimitsQuestion(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.DealWithinRespectiveAuthorityLevels, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No");
            return (yesAnswer, noAnswer);
        }

        private static (int, int) AddMandatoryTaskWithYesNoWithNoCausingAction(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, int alternativeActionId, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int yesId = id * 10;
            int noId = yesId + 1;

            var yesAnswer = AddWorkflowTaskAnswer(mb, taskId, yesId, "Yes");
            var noAnswer = AddWorkflowTaskAnswer(mb, taskId, noId, "No", null, alternativeActionId);
            return (yesAnswer, noAnswer);
        }

        private static (int, int) AddMandatoryTaskWithTwoAnswers(int? dependedUponAnswerid, ModelBuilder mb, int actionId, int id, int? precedingAnswerId, string description, string answer1, string answer2, params int[] dealTypes)
        {
            var taskId = AddWorkflowTask(dependedUponAnswerid, mb, id, actionId, description, WorkflowTaskTypeEnum.AnswerToQuestion, precedingAnswerId);
            AddDealTypeToTask(mb, taskId, dealTypes);

            int firstId = id * 10;
            int secondId = firstId + 1;

            var firstAnswer = AddWorkflowTaskAnswer(mb, taskId, firstId, answer1);
            var secondAnswer = AddWorkflowTaskAnswer(mb, taskId, secondId, answer2);
            return (firstAnswer, secondAnswer);
        }

        private static int AddWorkflowTask(int? dependedUponAnswerid, ModelBuilder mb, int id, int actionId, string description, WorkflowTaskTypeEnum type, int? precedingAnswerId = null, bool mandatory = true, bool active = true)
        {
            return mb.HasData(new WorkflowTask { Id = id, Order = id, WorkflowActionId = actionId, Description = description, Type = type, Mandatory = mandatory, PrecedingAnswerId = precedingAnswerId, DependingUponAnswerId = dependedUponAnswerid, Active = active });
        }

        private static int AddWorkflowTaskAnswer(ModelBuilder mb, int taskId, int id, string description, int? attachmentTypeToVerifyId = null, int? alternateWorkflowActionId = null)
        {
            var answerType = WorkflowTaskAnswerTypeEnum.Other;
            if (description == "Yes")
                answerType = WorkflowTaskAnswerTypeEnum.Yes;
            else if (description == "No")
                answerType = WorkflowTaskAnswerTypeEnum.No;

            return mb.HasData(new WorkflowTaskAnswer
            {
                Id = id,
                Description = description,
                WorkflowTaskId = taskId,
                AttachmentTypeToVerifyId = attachmentTypeToVerifyId,
                AlternateWorkflowActionId = alternateWorkflowActionId,
                AnswerType = answerType,
            });
        }

        static void AddDealTypeToTask(ModelBuilder mb, int taskId, params int[] dealTypes)
        {
            foreach (var dealTypeId in dealTypes)
                mb.HasData(new WorkflowTaskInDealType { DealTypeId = dealTypeId, WorkflowTaskId = taskId });
        }

        static int[] Combine(params int[] arraysToBeCombined)
        {
            var newList = new List<int>();
            foreach (var array in arraysToBeCombined)
            {
                newList.Add(array);
            }
            return newList.ToArray();
        }
        static int[] Combine(params int[][] arraysToBeCombined)
        {
            var newList = new List<int>();
            foreach (var array in arraysToBeCombined)
            {
                newList.AddRange(array);
            }
            return newList.ToArray();
        }
    }
}