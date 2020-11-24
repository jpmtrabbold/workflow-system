using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;
using Company.WorkflowSystem.Application.Services;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Infrastructure.Context;

namespace Company.WorkflowSystem.Application.DataAggregators
{
    internal class DealDataPolicyAssessment
    {
        IRepository<TradingDealsContext> _repo;
        DealDto Deal;
        DealService DealService;

        internal DealDataPolicyAssessment(DealDto deal, IRepository<TradingDealsContext> repo, DealService dealService)
        {
            _repo = repo;
            Deal = deal;
            DealService = dealService;
        }

        List<DealItemDto> Items;
        async Task<List<DealItemDto>> GetItems()
        {
            if (Items == null)
                Items = await DealService.GetItems(Deal.Id.Value);

            return Items;
        }

        string unitOfMeasure;
        internal string GetUnitOfMeasure()
        {
            if (unitOfMeasure == null)
            {
                unitOfMeasure = _repo.Context.DealTypes.Single(dt => dt.Id == Deal.DealTypeId.Value).UnitOfMeasure;
            }
            return unitOfMeasure;
        }

        bool? hasOnlyBuyItems;
        internal async Task<bool> HasOnlyBuyItems()
        {
            if (!hasOnlyBuyItems.HasValue)
            {
                var dealItems = await GetItems();
                hasOnlyBuyItems = !dealItems.Any(t => t.Position.Value == PositionEnum.Sell);
            }

            return hasOnlyBuyItems.Value;
        }

        bool? hasOnlySellItems;
        internal async Task<bool> HasOnlySellItems()
        {
            if (!hasOnlySellItems.HasValue)
            {
                var dealItems = await GetItems();
                hasOnlySellItems = !dealItems.Any(t => t.Position.Value == PositionEnum.Buy);
            }

            return hasOnlySellItems.Value;
        }

        decimal? buyVolume;
        decimal? sellVolume;
        decimal? volume;
        List<ItemGroupByDate> itemVolumeGroups;
        List<ItemGroupByDate> itemVolumeGroupsAllDeals;


        internal async Task<decimal> GetVolume(PositionEnum? position = null)
        {
            switch (position)
            {
                case PositionEnum.Buy:
                    if (!buyVolume.HasValue)
                        await CalcVolumes();
                    return buyVolume.Value;
                case PositionEnum.Sell:
                    if (!sellVolume.HasValue)
                        await CalcVolumes();
                    return sellVolume.Value;
                default:
                    if (!volume.HasValue)
                        await CalcVolumes();
                    return volume.Value;
            }
        }

        internal async Task<List<ItemGroupByDate>> GetItemVolumeGroups()
        {
            if (itemVolumeGroups == null)
                await CalcVolumes();
            return itemVolumeGroups;
        }

        internal async Task<List<ItemGroupByDate>> GetItemVolumeGroupsAllDeals()
        {
            if (itemVolumeGroupsAllDeals == null)
                await CalcVolumes();
            return itemVolumeGroupsAllDeals;
        }

        internal class ItemGroupByDate
        {
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
            public List<ItemGroupByHalfHour> HalfHours { get; set; } = new List<ItemGroupByHalfHour>();

            public ItemGroupByDate Clone()
            {
                return new ItemGroupByDate
                {
                    StartDate = StartDate,
                    EndDate = EndDate,
                    HalfHours = HalfHours.Select(hf => hf.Clone()).ToList()
                };

            }

        }
        internal class ItemGroupByHalfHour
        {
            public int StartHalfHour { get; set; }
            public int EndHalfHour { get; set; }
            public decimal BuyVolume { get; set; } = 0;
            public decimal SellVolume { get; set; } = 0;
            public decimal Volume { get; set; } = 0;

            public ItemGroupByHalfHour Clone()
            {
                return new ItemGroupByHalfHour
                {
                    StartHalfHour = StartHalfHour,
                    EndHalfHour = EndHalfHour,
                    BuyVolume = BuyVolume,
                    SellVolume = SellVolume,
                    Volume = Volume,
                };
            }
        }

        async Task CalcVolumes()
        {

            buyVolume = 0;
            sellVolume = 0;
            volume = 0;

            var groups = new List<ItemGroupByDate>();
            var otherItemsTask = GetOtherDealItems();
            var dealItems = await GetItems();

            foreach (var dealItem in dealItems)
                GroupItem(dealItem, groups);

            itemVolumeGroups = groups.Select(g => g.Clone()).ToList(); // clones to get only this deal's data

            var otherItems = await otherItemsTask;
            foreach (var otherItem in otherItems)
                GroupItem(otherItem, groups);

            buyVolume = groups.DefaultIfEmpty(null).Max(g => g?.HalfHours.DefaultIfEmpty(null).Max(hh => hh?.BuyVolume ?? 0) ?? 0);
            sellVolume = groups.DefaultIfEmpty(null).Max(g => g?.HalfHours.DefaultIfEmpty(null).Max(hh => hh?.SellVolume ?? 0) ?? 0);
            volume = groups.DefaultIfEmpty(null).Max(g => g?.HalfHours.DefaultIfEmpty(null).Max(hh => hh?.Volume ?? 0) ?? 0);
            itemVolumeGroupsAllDeals = groups;
        }

        private void GroupItem(DealItemDto dealItem, List<ItemGroupByDate> groups)
        {
            if (dealItem.Executed)
            {
                var executed = dealItem.ExecutedItems ?? new List<DealExecutedItemDto>();
                var hasExecuted = executed.Any();
                
                // if the dealItem has execution
                if (hasExecuted)
                {
                    foreach (var executedItem in executed)
                        GroupItemAux(executedItem, groups);

                    return;
                }
            }

            GroupItemAux(dealItem, groups);
        }

        private void GroupItemAux(DealItemDto dealItem, List<ItemGroupByDate> groups)
        {
            var startDate = dealItem.StartDate.Value.Value.DateWithMinTime();
            var endDate = dealItem.EndDate.Value.Value.DateWithMinTime();

            var thereIsStillSomeGroupingToDo = true;
            while (thereIsStillSomeGroupingToDo)
            {
                var match = groups.FirstOrDefault(tg => startDate >= tg.StartDate && startDate <= tg.EndDate);

                if (match == null)
                    match = groups.FirstOrDefault(tg => endDate >= tg.StartDate && endDate <= tg.EndDate);

                if (match == null)
                {
                    match = groups.FirstOrDefault(tg => tg.StartDate >= startDate && tg.StartDate <= endDate);
                    if (match == null)
                    {
                        var newGroup = new ItemGroupByDate { StartDate = startDate, EndDate = endDate };
                        GroupItemByHalfHour(dealItem, newGroup);
                        groups.Add(newGroup);
                        thereIsStillSomeGroupingToDo = false;
                    }
                    else
                    {
                        var newGroup = new ItemGroupByDate { StartDate = startDate, EndDate = match.StartDate.AddDays(-1) };
                        groups.Add(newGroup);
                        GroupItemByHalfHour(dealItem, newGroup);
                        startDate = match.StartDate;
                    }
                }
                else if (startDate > match.StartDate)
                {
                    var newGroup = match.Clone();
                    newGroup.StartDate = startDate;
                    match.EndDate = startDate.AddDays(-1);
                    groups.Insert(groups.IndexOf(match) + 1, newGroup);
                }
                else if (endDate < match.EndDate)
                {
                    var newGroup = match.Clone();
                    newGroup.EndDate = endDate;
                    match.StartDate = endDate.AddDays(1);
                    groups.Insert(groups.IndexOf(match), newGroup);
                }
                else if (endDate > match.EndDate)
                {
                    GroupItemByHalfHour(dealItem, match);
                    startDate = match.EndDate.AddDays(1);
                }
                else if (startDate < match.StartDate)
                {
                    GroupItemByHalfHour(dealItem, match);
                    endDate = match.StartDate.AddDays(-1);
                }
                else
                {
                    GroupItemByHalfHour(dealItem, match);
                    thereIsStillSomeGroupingToDo = false;
                }
            }
        }

        private void GroupItemByHalfHour(DealItemDto dealItem, ItemGroupByDate group)
        {
            var startTp = dealItem.HalfHourTradingPeriodStart.Value.HasValue ? dealItem.HalfHourTradingPeriodStart.Value.Value : 1;
            var endTp = dealItem.HalfHourTradingPeriodEnd.Value.HasValue ? dealItem.HalfHourTradingPeriodEnd.Value.Value : 48;

            var groups = group.HalfHours;

            var thereIsStillSomeGroupingToDo = true;
            while (thereIsStillSomeGroupingToDo)
            {
                var match = groups.FirstOrDefault(tg => startTp >= tg.StartHalfHour && startTp <= tg.EndHalfHour);

                if (match == null)
                    match = groups.FirstOrDefault(tg => endTp >= tg.StartHalfHour && endTp <= tg.EndHalfHour);

                if (match == null)
                {
                    match = groups.FirstOrDefault(tg => tg.StartHalfHour >= startTp && tg.StartHalfHour <= endTp);
                    if (match == null)
                    {
                        var newGroup = new ItemGroupByHalfHour { StartHalfHour = startTp, EndHalfHour = endTp };
                        AddItemVolumes(dealItem, newGroup);
                        groups.Add(newGroup);
                        thereIsStillSomeGroupingToDo = false;
                    }
                    else
                    {
                        var newGroup = new ItemGroupByHalfHour { StartHalfHour = startTp, EndHalfHour = match.StartHalfHour - 1 };
                        AddItemVolumes(dealItem, newGroup);
                        groups.Add(newGroup);
                        startTp = match.StartHalfHour;
                    }
                }
                else if (startTp > match.StartHalfHour)
                {
                    var newGroup = match.Clone();
                    newGroup.StartHalfHour = startTp;
                    match.EndHalfHour = startTp - 1;
                    groups.Insert(groups.IndexOf(match) + 1, newGroup);
                }
                else if (endTp < match.EndHalfHour)
                {
                    var newGroup = match.Clone();
                    newGroup.EndHalfHour = endTp;
                    match.StartHalfHour = endTp + 1;
                    groups.Insert(groups.IndexOf(match), newGroup);
                }
                else if (endTp > match.EndHalfHour)
                {
                    AddItemVolumes(dealItem, match);
                    startTp = match.EndHalfHour + 1;
                }
                else if (startTp < match.StartHalfHour)
                {
                    AddItemVolumes(dealItem, match);
                    endTp = match.StartHalfHour - 1;
                }
                else
                {
                    AddItemVolumes(dealItem, match);
                    thereIsStillSomeGroupingToDo = false;
                }
            }
        }

        private void AddItemVolumes(DealItemDto dealItem, ItemGroupByHalfHour newGroup)
        {
            var volume = dealItem.Quantity.Value ?? 0;

            if (dealItem.Position.Value == PositionEnum.Buy)
                newGroup.BuyVolume += volume;

            else if (dealItem.Position.Value == PositionEnum.Sell)
                newGroup.SellVolume += volume;

            newGroup.Volume += volume;

        }

        /// <summary>
        /// get all deal dealItems from the same submission user, deal category, deal type, counterparty and that are in between the same date period
        /// </summary>
        /// <returns></returns>
        async Task<List<DealItemDto>> GetOtherDealItems()
        {
            var minDate = MinDealDate().Result.DateWithMinTime();
            var maxDate = MaxDealDate().Result.DateWithMaxTime();

            if (Deal.SubmissionUserId.HasValue)
                return await _repo.ProjectedListBuilder(DealItemDto.ProjectionFromEntity)
                    .Where(dt =>
                    dt.DealId != Deal.Id &&
                    !dt.Deal.CurrentDealWorkflowStatus.WorkflowStatus.CancelDeal &&
                    dt.Deal.SubmissionUserId == Deal.SubmissionUserId &&
                    dt.Deal.DealCategoryId == Deal.DealCategoryId.Value &&
                    dt.Deal.DealTypeId == Deal.DealTypeId.Value &&
                    dt.Deal.CounterpartyId == Deal.CounterpartyId.Value)
                    .Where(dt => (dt.StartDate >= minDate && dt.StartDate <= maxDate) ||
                    (dt.EndDate >= minDate && dt.EndDate <= maxDate) ||
                    (minDate >= dt.StartDate && minDate <= dt.EndDate) ||
                    (maxDate >= dt.StartDate && maxDate <= dt.EndDate))
                    .ExecuteAsync();
            else
                return new List<DealItemDto>();
        }

        decimal? volumeForecastPercentage;
        internal async Task<decimal> GetVolumeForecastPercentage()
        {
            if (!volumeForecastPercentage.HasValue)
            {
                // add months to the range to make sure we get all forecasts we need (if there are unused it's ok)
                var minDate = MinDealDate().Result?.AddMonths(-1);
                var maxDate = MaxDealDate().Result?.AddMonths(1);

                var forecastsByMonth = await _repo.ProjectedListBuilder((SalesForecast sf) => new
                {
                    sf.MonthYear,
                    sf.Volume,
                })
                    .Where((SalesForecast sf) => sf.MonthYear >= minDate && sf.MonthYear <= maxDate)
                    .OrderBy(sf => sf.MonthYear)
                    .ExecuteAsync();

                volumeForecastPercentage = 0M;
                var groups = (await GetItemVolumeGroupsAllDeals());

                foreach (var forecast in forecastsByMonth)
                {
                    var perc = groups.Where(g => equalMonths(g.StartDate, forecast.MonthYear.DateWithMinTime()) || equalMonths(g.EndDate, forecast.MonthYear.DateWithMinTime()))
                        .DefaultIfEmpty(null)
                        .Max(g => g?.HalfHours.Max(hh => hh.Volume / forecast.Volume) ?? 0);
                    if (perc > volumeForecastPercentage)
                        volumeForecastPercentage = perc;
                }
            }
            return volumeForecastPercentage.Value;
        }

        bool equalMonths(DateTimeOffset date1, DateTimeOffset date2) => date1.Year == date2.Year && date1.Month == date2.Month;

        decimal? sellAcquisitionCost;
        decimal? acquisitionCost;

        internal async Task<decimal> GetAcquisitionCost(PositionEnum? position = null)
        {
            switch (position)
            {
                case PositionEnum.Buy:
                    return 0; // no Buy only acquisition cost for now
                case PositionEnum.Sell:
                    if (!sellAcquisitionCost.HasValue)
                        await CalcAcquisitionCost();
                    return sellAcquisitionCost.Value;
                default:
                    if (!acquisitionCost.HasValue)
                        await CalcAcquisitionCost();
                    return acquisitionCost.Value;
            }
        }

        async Task CalcAcquisitionCost()
        {
            sellAcquisitionCost = 0;
            acquisitionCost = 0;
            var dealItems = await GetItems();
            foreach (var t in dealItems)
            {
                if (t.Quantity.Value.HasValue &&
                    t.Price.Value.HasValue &&
                    t.HalfHourTradingPeriodStart.Value.HasValue &&
                    t.HalfHourTradingPeriodEnd.Value.HasValue &&
                    t.StartDate.Value.HasValue &&
                    t.EndDate.Value.HasValue)
                {
                    var quantity = t.Quantity.Value.Value;
                    var price = t.Price.Value.Value;
                    var tpStart = t.HalfHourTradingPeriodStart.Value.Value;
                    var tpEnd = t.HalfHourTradingPeriodEnd.Value.Value;
                    var startDate = t.StartDate.Value.Value.DateWithMinTime();
                    var endDate = t.EndDate.Value.Value.DateWithMinTime();

                    var halfHourCount = (tpEnd - tpStart + 1); // i.e.: 48 - 1 + 1 == 48
                    var durationInDays = (endDate - startDate).Days + 1;
                    var cost = quantity * (halfHourCount / 2) * durationInDays * price;

                    if (t.Position.Value == PositionEnum.Sell)
                        sellAcquisitionCost += cost;

                    acquisitionCost += cost;
                }
            }
        }

        MonthsDurationWithDaysOffset durationInMonthsWithDaysOffset;
        internal async Task<MonthsDurationWithDaysOffset> GetDurationInMonthsWithDaysOffset()
        {
            if (durationInMonthsWithDaysOffset == null)
            {
                var groups = (await GetItemVolumeGroups());
                var lastDate = groups.DefaultIfEmpty(null).Max(g => g?.EndDate ?? null);
                if (lastDate.HasValue && lastDate != default && Deal.SubmissionDate.HasValue)
                    durationInMonthsWithDaysOffset = MonthsDurationWithDaysOffset.FromDateDifference(Deal.SubmissionDate.Value.DateWithMinTime(), lastDate.Value.DateWithMinTime());
                else
                    durationInMonthsWithDaysOffset = new MonthsDurationWithDaysOffset();
            }
            return durationInMonthsWithDaysOffset;
        }

        MonthsDurationWithDaysOffset termInMonthsWithDaysOffset;
        internal async Task<MonthsDurationWithDaysOffset> GetTermInMonthsWithDaysOffset()
        {
            if (termInMonthsWithDaysOffset == null)
            {
                if (Deal.TermInMonthsOverride.Value.HasValue)
                {
                    termInMonthsWithDaysOffset = new MonthsDurationWithDaysOffset { MonthsDuration = Deal.TermInMonthsOverride.Value.Value };
                }
                else
                {
                    termInMonthsWithDaysOffset = new MonthsDurationWithDaysOffset();

                    var groups = (await GetItemVolumeGroups());

                    foreach (var group in groups)
                        termInMonthsWithDaysOffset.AddDatesOffset(group.StartDate, group.EndDate);
                }
            }
            return termInMonthsWithDaysOffset;
        }

        DateTimeOffset? minDealDate;

        internal async Task<DateTimeOffset?> MinDealDate()
        {
            if (!minDealDate.HasValue)
            {
                var dealItems = await GetItems();
                minDealDate = dealItems.DefaultIfEmpty(null).Min(t => t?.StartDate.Value ?? null);
            }
                
            return minDealDate;
        }

        DateTimeOffset? maxDealDate;
        internal async Task<DateTimeOffset?> MaxDealDate()
        {
            if (!maxDealDate.HasValue)
            {
                var dealItems = await GetItems();
                maxDealDate = dealItems.DefaultIfEmpty(null).Max(t => t?.EndDate.Value ?? null);
            }
                
            return maxDealDate;
        }
    }
}