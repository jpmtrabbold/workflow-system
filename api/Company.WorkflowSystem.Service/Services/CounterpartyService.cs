using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Exceptions;
using System;
using System.Linq;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Service.Models.ViewModels.Counterparties;
using Company.WorkflowSystem.Service.Models.Dtos.Counterparties;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Database.Context;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Util;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Service.Services
{
    public class CounterpartyService : BaseService
    {
        public CounterpartyService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {
        }

        async public Task<CounterpartiesListResponse> List(CounterpartiesListRequest listRequest)
        {
            var builder = ListBuilder(listRequest, CounterpartyListDto.ProjectionFromEntity);

            return new CounterpartiesListResponse
            {
                Counterparties = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }
        async public Task<LookupRequestHeader> DropdownList(CounterpartiesListRequest listRequest)
        {
            var builder = ListBuilder(listRequest, LookupRequest.ProjectionFromCounterparty);
            var result = new LookupRequestHeader
            {
                CurrentPage = listRequest.PageNumber,
                TotalCount = await builder.CountAsync(),
                Results = await builder.ExecuteAsync(),
            };
            return result;
        }

        public IListRequestBuilder<Counterparty, TProjectedEntity, TradingDealsContext> ListBuilder<TProjectedEntity>
            (CounterpartiesListRequest listRequest, Expression<Func<Counterparty, TProjectedEntity>> projection)
        {
            var builder = _repo.ProjectedListBuilder(projection, listRequest)
                .OrderByEntity(c => c.Id, descending: true)
                .ConditionalOrderEntity("name", c => c.Name)
                .ConditionalOrderEntity("code", c => c.Code)
                .ConditionalOrderEntity("active", c => c.Active)
                .ConditionalOrderEntity("exposureLimit", c => c.ExposureLimit)
                .ConditionalOrderEntity("expiryDate", c => c.ExpiryDate);

            if (listRequest.Id.HasValue)
                builder.WhereEntity(d => d.Id == listRequest.Id);

            var str = listRequest.SearchString?.Trim();
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.WhereEntity(c =>
                    c.Name.Contains(str)
                    || c.Code.Contains(str)
                    || (c.Active ? "Yes" : "No").Contains(str)
                );
            }

            if (listRequest.OnlyActive)
                builder.WhereEntity(c => c.Active);

            if (listRequest.DealId.HasValue)
                builder.WhereEntity(c => c.DealCategories.Any(pt => pt.DealCategoryId == listRequest.DealId));

            if (listRequest.OnlyNonExpiredAndApproved == true)
            {
                var now = DateUtils.GetDateTimeOffsetNow();
                builder.WhereEntity(c => now < c.ExpiryDate && c.ApprovalDate.HasValue);
            }

            return builder;
        }

        async public Task<bool> CheckForDuplicateCodes(int counterpartyId, string code)
        {
            var counterparties = await _repo.ProjectedListBuilder(LookupRequest.ProjectionFromCounterparty)
                .WhereEntity(cp => (cp.Code == code && cp.Id != counterpartyId))
                .ExecuteAsync();

            return counterparties.Any();
        }

        async public Task<CounterpartyDto> Get(int counterpartyId)
        {
            var counterparty = await _repo.ProjectedGetById(counterpartyId, CounterpartyDto.ProjectionFromEntity);

            return counterparty;
        }
        public async Task<List<LookupRequest>> GetCountries()
        {
            return await _repo.ProjectedList(LookupRequest.ProjectionFromCountry);
        }
        async public Task<CounterpartyPostResponse> Save(CounterpartyDto counterparty)
        {
            Validate(counterparty);

            var creation = !counterparty.Id.HasValue || counterparty.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<Counterparty>(counterparty.Id);

            if (!creation)
            {
                _repo.LoadCollection(entity, c => c.DealCategories);
            }

            entity = counterparty.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Counterparties);

            return new CounterpartyPostResponse { CounterpartyName = entity.Name };
        }

        public async Task<bool> CheckCodeUsedInDeals(int counterpartyId)
        {
            if (counterpartyId > 0)
            {
                var code = (await _repo.ProjectedGetById(counterpartyId, (Counterparty c) => c.Code)).Trim();
                return _repo.Get<Deal>(d => d.CounterpartyId == counterpartyId && d.DealNumber.Substring(0, code.Length) == code).Any();
            }
            return false;
        }

        void Validate(CounterpartyDto counterparty)
        {
            if (Updatable.IsUpdatedButEmpty(counterparty.Name))
                throw new BusinessRuleException("Please enter a counterparty name.");

            if (Updatable.IsUpdatedButEmpty(counterparty.Code))
                throw new BusinessRuleException("Please enter a counterparty code.");

            if (_repo.Context.Counterparties.Any(c => c.Id != counterparty.Id && c.Name == counterparty.Name.Value))
                throw new BusinessRuleException($"There is another counterparty using {counterparty.Name.Value} as a name.", "Names must be unique");
        }
    }
}