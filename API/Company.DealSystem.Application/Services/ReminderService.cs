using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.DealSystem.Application.Exceptions;
using Company.DealSystem.Application.Models.Dtos.Products;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Models.ViewModels.Nodes;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Services;
using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Application.DataAggregators;
using Company.DealSystem.Domain.Util;
using Hangfire.Logging;
using Microsoft.Extensions.Configuration;
using Company.DealSystem.Application.Utils;

namespace Company.DealSystem.Application.Services
{
    public class ReminderService : BaseService
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        ConfigurationReader _configReader;
        ConfigurationReader configReader
        {
            get
            {
                if (_configReader == null)
                    _configReader = GetConfigurationEntries(ConfigurationGroupIdentifiersEnum.Reminders).Result;
                return _configReader;
            }
        }
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public ReminderService(
            IRepository<TradingDealsContext> repo,
            ScopedDataService scopedDataService,
            IConfiguration configuration,
            IEmailService emailService
            ) : base(repo, scopedDataService)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        async public Task SendReminders()
        {
            await DealExpiryDateReminders();
            await DealNotesReminders();
            await CounterpartyExpiryReminders();
        }

        static string getStringDateBasedOnDay(int day, string pastVerb, string futureVerb)
        {
            if (day == 0)
                return futureVerb + " today";
            else if (day == -1)
                return futureVerb + " tomorrow";
            else if (day == 1)
                return pastVerb + " yesterday";
            else if (day < 0)
                return $"{futureVerb} in {Math.Abs(day)} days";
            else
                return $"{pastVerb} {Math.Abs(day)} days ago";
        }

        async Task DealExpiryDateReminders()
        {
            var emails = configReader.GetEntryAsString(ConfigurationIdentifiersEnum.DealExpiryDatesEmailAccountsNotified).Split(";").Select(d => d.Trim());
            var days = configReader.GetEntryAsString(ConfigurationIdentifiersEnum.ReminderDaysBeforeDealExpiryDate).Split(";").Select(d => int.Parse(d));
            var today = DateUtils.GetDateTimeOffsetNow();

            foreach (var day in days)
            {
                var date = today.AddDays(-day);
                var dateMin = date.DateWithMinTime();
                var dateMax = date.DateWithMaxTime();

                var deals = await _repo.ProjectedListBuilder((Deal d) => new { d.Id, d.DealNumber })
                    .Where(d => !d.CurrentDealWorkflowStatus.Finalized)
                    .Where(d => d.ExpiryDate.HasValue && d.ExpiryDate >= dateMin && d.ExpiryDate <= dateMax)
                    .ExecuteAsync();

                var expiryInfo = getStringDateBasedOnDay(day, "expired", "expires");

                foreach (var deal in deals)
                {
                    var subject = $"Deal {deal.DealNumber} {(day <= 0 ? "Expiring Soon" : "Expired")}";
                    var html = @"
                    <html style='font-family: Helvetica'>
                        Hi,
                        </br></br>
                        The deal {dealNumber} {expiryInfo}.
                        </br></br>
                        {actionLinks}
                        </br></br>
                        Best Regards,
                        </br></br>
                        DealSystem
                    </html>
                    ";

                    var links = new List<(string urlSuffix, string buttonName)>();
                    links.Add(($"/all-deals/{deal.Id}", "View Deal"));

                    var actionLinks = UtilMethods.GenerateHtmlLinkButtons(links, _configuration.GetValue<string>("FrontEndBaseUrl"));

                    html = html.Replace("{dealNumber}", deal.DealNumber);
                    html = html.Replace("{expiryInfo}", expiryInfo);
                    html = html.Replace("{actionLinks}", actionLinks);

                    foreach (var to in emails)
                    {
                        (var success, var message) = await _emailService.SendEmail(
                            from: _configuration.GetValue<string>("SenderEmailAccount"),
                            to: to,
                            subject: subject,
                            content: html
                            );

                        if (!success)
                            Logger.Warn($"Deal Expiry Date e-mail failed. Error message: {message}, To: {to}, Subject: {subject}, Content: {html}.");
                        else
                            Logger.Info($"Deal Expiry Date e-mail sent successfully. Success message: {message}, To: {to}, Subject: {subject}, Content: {html}.");
                    }
                }
            }
        }

        async Task DealNotesReminders()
        {
            var today = DateUtils.GetDateTimeOffsetNow();
            var todayStart = today.DateWithMinTime();
            var todayEnd = today.DateWithMaxTime();

            var notes = await _repo.ProjectedListBuilder((DealNote n) => new
            {
                n.Content,
                n.ReminderType,
                n.Deal.DealNumber,
                n.DealId,
                UserEmail = n.ReminderUserId.HasValue ? n.ReminderUser.Username : null,
                RolesEmails = n.ReminderUserId.HasValue ? n.ReminderUser.WorkflowRolesInUser
                    .Select(wr => wr.WorkflowRole.UsersInWorkflowRole
                    .Select(uwr => uwr.User.Username)) : null,
                EmailAccounts = n.ReminderEmailAccounts,
            })
                .Where(n => !n.Deal.CurrentDealWorkflowStatus.Finalized && n.ReminderDateTime.HasValue && n.ReminderType.HasValue)
                .Where(n => n.ReminderDateTime >= todayStart && n.ReminderDateTime <= todayEnd)
                .ExecuteAsync();

            foreach (var note in notes)
            {
                var html = @"
                    <html style='font-family: Helvetica'>
                        Hi. Here's a reminder from deal {dealNumber}:
                        </br></br>
                        {body}.
                        </br></br>
                        {actionLinks}
                        </br></br>
                        Best Regards,
                        </br></br>
                        DealSystem
                    </html>
                    ";

                var body = note.Content;
                var subject = $"Reminder for Deal {note.DealNumber}";
                var links = new List<(string urlSuffix, string buttonName)>();
                links.Add(($"/all-deals/{note.DealId}", "View Deal"));

                var actionLinks = UtilMethods.GenerateHtmlLinkButtons(links, _configuration.GetValue<string>("FrontEndBaseUrl"));

                html = html.Replace("{dealNumber}", note.DealNumber);
                html = html.Replace("{body}", body);
                html = html.Replace("{actionLinks}", actionLinks);

                var toList = new List<string>();
                if (note.ReminderType == NoteReminderTypeEnum.Me)
                    toList.Add(note.UserEmail);
                else if (note.ReminderType == NoteReminderTypeEnum.MyRole)
                    note.RolesEmails.ForEach(r1 => r1.ForEach(r2 => toList.Add(r2)));

                var to = string.Join("; ", toList) + ";" + note.EmailAccounts;

                (var success, var message) = await _emailService.SendEmail(
                    from: _configuration.GetValue<string>("SenderEmailAccount"),
                    to: to,
                    subject: subject,
                    content: html
                    );

                if (!success)
                    Logger.Warn($"Deal Expiry Date e-mail failed. Error message: {message}, To: {toList}, Subject: {subject}, Content: {html}.");
                else
                    Logger.Info($"Deal Expiry Date e-mail sent successfully. Success message: {message}, To: {toList}, Subject: {subject}, Content: {html}.");
            }

        }

        async Task CounterpartyExpiryReminders()
        {
            var emails = configReader.GetEntryAsString(ConfigurationIdentifiersEnum.DealExpiryDatesEmailAccountsNotified).Split(";").Select(d => d.Trim());
            var days = configReader.GetEntryAsString(ConfigurationIdentifiersEnum.ReminderDaysBeforeCounterpartyReviewDate).Split(";").Select(d => int.Parse(d));
            var today = DateUtils.GetDateTimeOffsetNow();

            foreach (var day in days)
            {
                var date = today.AddDays(-day);
                var dateMin = date.DateWithMinTime();
                var dateMax = date.DateWithMaxTime();

                var counterparties = await _repo.ProjectedListBuilder((Counterparty c) => new { c.Id, c.Name, c.ExposureLimit, c.ExpiryDate })
                    .Where(c => !c.Active)
                    .Where(c => c.ExpiryDate.HasValue && c.ExpiryDate >= dateMin && c.ExpiryDate <= dateMax)
                    .ExecuteAsync();

                foreach (var counterparty in counterparties)
                {
                    var expiryInfo = counterparty.ExpiryDate.Value.ToDateString();
                    var subject = $"Counterparty Review ({counterparty.Name})";
                    var html = @"
                    <html style='font-family: Helvetica'>
                        Hi,
                        </br></br>
                        Counterparty {name} with ${exposureLimit} Exposure Limit expires on {expiryInfo}.
                        </br></br>
                        {actionLinks}
                        </br></br>
                        Best Regards,
                        </br></br>
                        DealSystem
                    </html>
                    ";

                    var links = new List<(string urlSuffix, string buttonName)>();
                    links.Add(($"/counterparties/{counterparty.Id}", "View Counterparty"));

                    var actionLinks = UtilMethods.GenerateHtmlLinkButtons(links, _configuration.GetValue<string>("FrontEndBaseUrl"));

                    html = html.Replace("{name}", counterparty.Name);
                    html = html.Replace("{exposureLimit}", counterparty.ExposureLimit.ToString("N2"));
                    html = html.Replace("{expiryInfo}", expiryInfo);
                    html = html.Replace("{actionLinks}", actionLinks);

                    foreach (var to in emails)
                    {
                        (var success, var message) = await _emailService.SendEmail(
                            from: _configuration.GetValue<string>("SenderEmailAccount"),
                            to: to,
                            subject: subject,
                            content: html
                            );

                        if (!success)
                            Logger.Warn($"Counterparty Expiry Date e-mail failed. Error message: {message}, To: {to}, Subject: {subject}, Content: {html}.");
                        else
                            Logger.Info($"Counterparty Expiry Date e-mail sent successfully. Success message: {message}, To: {to}, Subject: {subject}, Content: {html}.");
                    }
                }
            }
        }
    }
}
