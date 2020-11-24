using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Application.Exceptions;
using Company.WorkflowSystem.Application.Models.Dtos.Products;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Application.Models.ViewModels.Nodes;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Application.Models.ViewModels.DealIntegration;
using Company.WorkflowSystem.Domain.Entities.Integrations;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Interfaces.EmsTradepoint;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;
using Company.WorkflowSystem.Domain.Util;
using Newtonsoft.Json;
using Company.WorkflowSystem.Domain.Services;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Company.WorkflowSystem.Application.Utils;

namespace Company.WorkflowSystem.Application.Services
{
    public class DealIntegrationService : BaseService
    {
        UserService _userService;
        DealService _dealService;
        IEmailService _emailService;
        IEmsTradepointService _emsService;
        IConfiguration _configuration;
        ConfigurationReader _configReader;
        ConfigurationReader configReader
        {
            get
            {
                if (_configReader == null)
                {
                    _configReader = GetConfigurationEntries(ConfigurationGroupIdentifiersEnum.AbcTradesIntegration).Result;
                }
                return _configReader;
            }
        }

        public DealIntegrationService(
            IRepository<TradingDealsContext> repo,
            ScopedDataService scopedDataService,
            UserService userService,
            DealService dealService,
            IEmailService emailService,
            IEmsTradepointService emsService,
            IConfiguration configuration) : base(repo, scopedDataService)
        {
            _emsService = emsService;
            _userService = userService;
            _dealService = dealService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task EmsFetchFromYesterday()
        {
            var now = DateUtils.GetDateTimeOffsetNow();
            
            var request = new EmsFetchRequest
            {
                StartCreationDateTime = now.AddDays(-1).DateWithMinTime(), // first milisecond of yesterday
                EndCreationDateTime = now.DateWithMinTime().AddTicks(-1), // last milisecond of yesterday
            };
            await EmsFetch(request);
        }

        public async Task<bool> EmsFetch(EmsFetchRequest request)
        {
            var ok = true;
            var userIdStarted = TryGetUserId();
            var integrationRun = new IntegrationRun(IntegrationTypeEnum.EmsTradepoint, request, userIdStarted);
            await _repo.SaveEntity(integrationRun);

            try
            {
                var response = await _emsService.FetchTrades(request.StartCreationDateTime, request.EndCreationDateTime);
                integrationRun.AddInfoEntry("Trades were fetched successfully from EMS.", response.Payload);
                await ProcessEmsTrades(response.Trades, integrationRun);
            }
            catch (Exception ex)
            {
                integrationRun.AddErrorEntry(ex);
                ok = false;
            }

            integrationRun.EndRun();

            await _repo.SaveEntity(integrationRun);
            
            try
            {
                //if (!userIdStarted.HasValue)
                SendEmailNotifications(integrationRun);
            }
            catch (Exception ex)
            {
                integrationRun.AddErrorEntry(ex);
                await _repo.SaveEntity(integrationRun);
                ok = false;
            }

            return ok;
        }

        void SendEmailNotifications(IntegrationRun integrationRun)
        {
            if (integrationRun.Status != IntegrationRunStatusEnum.Success)
            {
                var to = configReader.GetEntryAsString(ConfigurationIdentifiersEnum.EmailAccountsNotifiedOnError);
                if (!string.IsNullOrWhiteSpace(to))
                {
                    var html = @"
                        <html style='font-family: Helvetica'>
                            Hello,
                            </br></br>
                            The EMS Integration Run #{integrationRunId} {integrationRunOutcome}.
                            </br></br>
                            You can check it out by clicking on this link:
                            </br></br>
                            {actionLinks}
                            </br></br>
                            Best Regards,
                            </br></br>
                            WorkflowSystem
                        </html>
                    ";
                    var links = new List<(string urlSuffix, string buttonName)>();
                    links.Add(($"/integration/ems/{integrationRun.Id}", "View Integration Run"));

                    var actionLinks = UtilMethods.GenerateHtmlLinkButtons(links, _configuration.GetValue<string>("FrontEndBaseUrl"));
                    html = html.Replace("{integrationRunId}", integrationRun.Id.ToString());
                    html = html.Replace("{integrationRunOutcome}", integrationRun.Status == IntegrationRunStatusEnum.Warning ? "ran successfully but had warnings" : "failed");
                    html = html.Replace("{actionLinks}", actionLinks);

                    _emailService.SendEmail(
                        from: _configuration.GetValue<string>("SenderEmailAccount"),
                        to: to,
                        subject: $"Ems Integration Errors/Warnings - Integration Run #{integrationRun.Id}",
                        content: html
                        );
                }
            }
        }
        
        private async Task ProcessEmsTrades(ICollection<DealItemFromEmsTrade> trades, IntegrationRun integrationRun)
        {
            if (trades.Count == 0)
            {
                integrationRun.AddInfoEntry("There are no EMS trades to be imported.");
                return;
            }

            var itemsByUser = await ConvertTradesIntoItems(trades, integrationRun);

            await CreateDeals(itemsByUser, integrationRun);
        }

        private async Task CreateDeals(TradeUserGroupDictionary userGrouping, IntegrationRun integrationRun)
        {
            int? counterpartyId = configReader.GetEntryAsInt(ConfigurationIdentifiersEnum.CounterpartyIdentifier);
            int? dealTypeId = configReader.GetEntryAsInt(ConfigurationIdentifiersEnum.DealTypeIdentifier);
            int? dealCategoryId = configReader.GetEntryAsInt(ConfigurationIdentifiersEnum.DealCategoryIdentifier);
            var shouldReintegrateCancelled = configReader.GetEntryAsBoolean(ConfigurationIdentifiersEnum.ShouldReintegrateCancelledDeals);
            var dealTypeConfig = await _dealService.GetDealTypeConfiguration(dealTypeId.Value);

            var type = DealItemSourceTypeEnum.Ems;

            var sourceIds = userGrouping.SelectMany(ug => ug.Value.dealItems).Select(t => t.SourceData?.SourceId).Where(i => i.HasValue);
            // gathers existing deals with the same source id
            var groupedExistingDeals = await _repo.ProjectedListBuilder((DealItem dt) => new
            {
                dt.Deal.DealNumber,
                dt.Deal.Id,
                SourceId = dt.SourceData != null ? dt.SourceData.SourceId : null,
                Cancelled = dt.Deal.CurrentDealWorkflowStatus.WorkflowStatus.CancelDeal,
            })
                .Where(d => d.SourceData != null && d.SourceData.Type == type && sourceIds.Contains(d.SourceData.SourceId))
                .ExecuteAsync();

            var existingDeals = groupedExistingDeals.GroupBy(x => x.SourceId, (key, g) => g.OrderByDescending(e => e.Id).First());

            foreach (var group in userGrouping)
            {
                var userId = group.Value.userId;
                var dealItems = group.Value.dealItems;
                scopedDataService.SetTemporaryUserId(userId);
                foreach (var dealItem in dealItems)
                {
                    var reintegrated = false;
                    var existingDeal = existingDeals.FirstOrDefault(e => e.SourceId == dealItem.SourceData.SourceId);
                    if (existingDeal != null)
                    {
                        if (existingDeal.Cancelled)
                        {
                            if (shouldReintegrateCancelled)
                            {
                                integrationRun.AddInfoEntry($"Trade {dealItem.SourceData.SourceId} is already in WorkflowSystem " +
                                $"under deal number {existingDeal.DealNumber}, but it was cancelled. This deal will be reintegrated, " +
                                $"as WorkflowSystem is configured to reintegrate cancelled deals.");
                                reintegrated = true;
                            }
                            else
                            {
                                integrationRun.AddInfoEntry($"Trade {dealItem.SourceData.SourceId} is already in WorkflowSystem " +
                                $"under deal number {existingDeal.DealNumber}. Even though the deal is cancelled, WorkflowSystem is " +
                                $"configure to ignore reintegrations of cancelled deals, therefore this trade will be ignored.");
                                continue;
                            }
                        }
                        else
                        {
                            integrationRun.AddInfoEntry($"Trade {dealItem.SourceData.SourceId} is already in WorkflowSystem " +
                                $"under deal number {existingDeal.DealNumber}. This trade will be ignored.");
                            continue;
                        }
                    }
                    var deal = new DealDto(dealCategoryId, dealTypeId, counterpartyId, dealTypeConfig.WorkflowSetId, dealTypeConfig.DealItemFieldsetId);

                    deal.Items.Data.Add(dealItem);
                    deal.Notes.Data.Add(new DealNoteDto
                    {
                        CreatedDate = Updatable.Create(DateUtils.GetDateTimeOffsetNow(), true),
                        NoteContent = Updatable.Create(
                            !reintegrated ?
                            "This deal was integrated from EMS." :
                                $"This deal was reintegrated from EMS. It was integrated before as {existingDeal.DealNumber}, " +
                                $"but it was cancelled, so the current deal replaces the previous.",
                            true),
                        NoteCreatorId = userId,
                    });

                    try
                    {
                        using (var scope = CreateTransactionScope())
                        {
                            var result = await _dealService.Save(deal);

                            var warningMessages = await _dealService.PerformWorkflowAction(
                                dealId: result.DealId,
                                actionId: configReader.GetEntryAsInt(ConfigurationIdentifiersEnum.WorkflowActionId));

                            string details = warningMessages.Any() ? "Some warning messages were thrown during the deal creation: " + String.Join("; ", warningMessages) : null;
                            integrationRun.AddSuccessEntry(
                                message: $"Deal {result.DealNumber} created successfully",
                                functionalityOfAffectedId: FunctionalityEnum.Deals,
                                affectedId: result.DealId,
                                details: details
                                );

                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        integrationRun.AddErrorEntry("There was an error while creating the deal.", ex, deal);
                        _repo.ResetChanges(exceptionEntities: integrationRun.CurrentEntities);
                    }
                }

                scopedDataService.ClearTemporaryUserId();
            }
        }

        async Task<TradeUserGroupDictionary> ConvertTradesIntoItems(
            ICollection<DealItemFromEmsTrade> trades,
            IntegrationRun integrationRun)
        {
            int? nodeId = configReader.GetEntryAsInt(ConfigurationIdentifiersEnum.ProductIdentifier);
            var order = 0;
            var userGrouping = new TradeUserGroupDictionary();
            foreach (var trade in trades)
            {
                try
                {
                    order++;
                    decimal? quantity = null;
                    decimal? price = null;

                    if (trade.Quantity.HasValue)
                        quantity = Convert.ToDecimal(trade.Quantity);

                    if (trade.Price.HasValue)
                        price = Convert.ToDecimal(trade.Price);

                    int userId;
                    List<DealItemDto> dealItems;
                    if (!userGrouping.ContainsKey(trade.TraderId))
                    {
                        var traderId = trade.TraderId.ToString();
                        userId = await _userService.GetCorrespondingUserIdForIntegration(IntegrationTypeEnum.EmsTradepoint, traderId);
                        if (userId <= 0)
                            throw new Exception($"A corresponding user could not be found for the following EMS user: {traderId}-{trade.TraderName}");

                        dealItems = new List<DealItemDto>();
                        userGrouping.Add(trade.TraderId, (userId, dealItems));
                    }
                    else
                    {
                        (userId, dealItems) = userGrouping[trade.TraderId];
                    }

                    var dealItem = new DealItemDto
                    {
                        SourceData = new DealItemSourceDataDto
                        {
                            SourceId = Convert.ToInt64(trade.TradeId),
                            CreationDate = trade.CreationDate,
                            Type = DealItemSourceTypeEnum.Ems,
                        },
                        DayType = Updatable.CreateNullable(DayTypeEnum.AllDays, true),
                        StartDate = Updatable.CreateNullable(trade.StartDate, true),
                        EndDate = Updatable.CreateNullable(trade.EndDate, true),
                        HalfHourTradingPeriodStart = Updatable.CreateNullable(1, true),
                        HalfHourTradingPeriodEnd = Updatable.CreateNullable(48, true),
                        Quantity = Updatable.Create(quantity, true),
                        Price = Updatable.Create(price, true),
                        Position = Updatable.CreateNullable(trade.Position == EmsTradePositionEnum.Buy ? PositionEnum.Buy : PositionEnum.Sell, true),
                        Order = order,
                        ProductId = Updatable.Create(nodeId, true),
                        Updated = true,
                    };
                    dealItems.Add(dealItem);

                    integrationRun.AddInfoEntry("Trade converted into deal item successfully.", trade);
                }
                catch (Exception ex)
                {
                    integrationRun.AddErrorEntry($"This trade couldn't be converted to a deal item. Reason: {ex.Message}", payload: trade);
                }
            }

            return userGrouping;
        }
    }

    public class TradeUserGroupDictionary : Dictionary<double?, (int userId, List<DealItemDto> dealItems)>
    {
        public TradeUserGroupDictionary() : base()
        {

        }
    }
}
