using System.Collections.Generic;
using System.Threading.Tasks;
using Company.DealSystem.Application.Exceptions;
using Company.DealSystem.Application.Models.Dtos.SalesForecasts;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Models.ViewModels.SalesForecasts;
using Company.DealSystem.Domain.Entities;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Linq;
using Company.DealSystem.Application.Extensions;
using System;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Application.Services
{
    public class SalesForecastService : BaseService
    {
        public SalesForecastService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<SalesForecastsListResponse> List(SalesForecastsListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(SalesForecastListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.MonthYear)
                .ConditionalOrder("volume", c => c.Volume)
                .ConditionalOrder("monthYear", c => c.MonthYear);

            return new SalesForecastsListResponse
            {
                SalesForecasts = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        public async Task BulkImport(List<SalesForecastDto> list)
        {
            using (var scope = CreateTransactionScope())
            {
                var ordered = list.OrderBy(f => f.MonthYear);
                var first = list.First().MonthYear.Value.AddDays(-32);
                var last = list.Last().MonthYear.Value.AddDays(32);
                var dbForecasts = _repo.Context.SalesForecasts.Where(f => f.MonthYear >= first && f.MonthYear <= last);

                foreach (var forecast in list)
                {
                    forecast.Id = dbForecasts.FirstOrDefault(f => f.MonthYear.Year == forecast.MonthYear.Value.Year && f.MonthYear.Month == forecast.MonthYear.Value.Month)?.Id ?? 0;
                    try
                    {
                        var result = await Save(forecast);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("The bulk data import could not be executed successfully.", ex);
                    }
                    
                }
                scope.Complete();
            }
        }

        async public Task<SalesForecastDto> Get(int salesForecastId)
        {
            var salesForecast = await _repo.ProjectedGetById(salesForecastId, SalesForecastDto.ProjectionFromEntity);

            return salesForecast;
        }

        async public Task<SalesForecastPostResponse> Save(SalesForecastDto salesForecast, int? userId = null)
        {
            Validate(salesForecast);

            var creation = !salesForecast.Id.HasValue || salesForecast.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<SalesForecast>(salesForecast.Id);

            entity = salesForecast.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.SalesForecasts);

            return new SalesForecastPostResponse { SalesForecastName = entity.MonthYear.DateWithMinTime().Month + "/" + entity.MonthYear.DateWithMinTime().Year };
        }

        void Validate(SalesForecastDto salesForecast)
        {
            if (Updatable.IsUpdatedButEmpty(salesForecast.MonthYear))
                throw new BusinessRuleException("Please enter a month/year for the forecast.");

            var month = salesForecast.MonthYear.Value.DateWithMinTime().Month;
            var year = salesForecast.MonthYear.Value.DateWithMinTime().Year;

            if (_repo.Context.SalesForecasts.Any(f => f.Id != salesForecast.Id && f.MonthYear.Year == year && f.MonthYear.Month == month))
                throw new BusinessRuleException($"There is a forecast already for {month + "/" + year}.");
        }
    }
}
