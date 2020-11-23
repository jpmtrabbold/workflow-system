﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Company.DealSystem.Application.Exceptions;
using Company.DealSystem.Application.Models.Dtos.DealTypes;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Models.ViewModels.DealTypes;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Domain.Models.Enum;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Application.Services
{
    public class DealTypeService : BaseService
    {
        public DealTypeService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<DealTypesListResponse> List(DealTypesListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(DealTypeListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("name", c => c.Name)
                .ConditionalOrder("positionName", c => DealType.PositionName(c))
                .ConditionalOrder("unitOfMeasure", c => c.UnitOfMeasure)
                .ConditionalOrder("hasLossFactors", c => DealType.HasLossFactorsDescription(c))
                .ConditionalOrder("hasExpiryDate", c => (c.HasExpiryDate ? "Yes" : "No"))
                .ConditionalOrder("itemFieldsetName", c => DealType.ItemFieldsetName(c))
                .ConditionalOrder("workflowSetName", c => DealType.WorkflowSetName(c))
                .ConditionalOrder("activeDescription", c => DealType.ActiveDescription(c));

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(entity => 
                    entity.Name.Contains(str)
                    || DealType.PositionName(entity).Contains(str)
                    || entity.UnitOfMeasure.Contains(str)
                    || DealType.HasLossFactorsDescription(entity).Contains(str)
                    || (entity.HasExpiryDate ? "Yes" : "No").Contains(str)
                    || DealType.ItemFieldsetName(entity).Contains(str)
                    || DealType.WorkflowSetName(entity).Contains(str)
                    || DealType.ActiveDescription(entity).Contains(str)
                );
            }

            return new DealTypesListResponse
            {
                DealTypes = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        public async Task<List<LookupRequest>> GetDealItemFieldsetLookups()
        {
            var lookups = _repo.ProjectedListBuilder(LookupRequest.ProjectionFromDealItemFieldset);
            lookups.OrderBy(pt => pt.Description);
            return await lookups.ExecuteAsync();
        }

        public async Task<List<LookupRequest>> GetWorkflowSetLookups()
        {
            var lookups = _repo.ProjectedListBuilder(LookupRequest.ProjectionFromWorkflowSet);
            lookups.OrderBy(pt => pt.Description);
            return await lookups.ExecuteAsync();
        }

        async public Task<DealTypeDto> Get(int dealTypeId)
        {
            var dealType = await _repo.ProjectedGetById(dealTypeId, DealTypeDto.ProjectionFromEntity);

            return dealType;
        }

        async public Task<DealTypePostResponse> Save(DealTypeDto dealType, int? userId = null)
        {
            Validate(dealType);

            var creation = !dealType.Id.HasValue || dealType.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<DealType>(dealType.Id);
            if (!creation)
            {
                _repo.LoadCollection(entity, d => d.DealCategoriesInDealType);
            }
            
            entity = dealType.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.DealTypes);

            return new DealTypePostResponse { DealTypeName = entity.Name };
        }

        void Validate(DealTypeDto dealType)
        {
            if (Updatable.IsUpdatedButEmpty(dealType.Name))
                throw new BusinessRuleException("Please enter a name.");

            if (Updatable.IsUpdatedButEmpty(dealType.UnitOfMeasure))
                throw new BusinessRuleException("Please enter an Unit of Measure.");

            if (Updatable.IsUpdatedButEmpty(dealType.DealItemFieldsetId))
                throw new BusinessRuleException("Please enter a Deal Item Fieldset.");

            if (Updatable.IsUpdatedButEmpty(dealType.WorkflowSetId))
                throw new BusinessRuleException("Please enter a Workflow Set.");

            if (_repo.Context.DealTypes.Any(c => c.Id != dealType.Id && c.Name == dealType.Name.Value))
                throw new BusinessRuleException($"There is another deal type using {dealType.Name.Value} as a name.", "Names must be unique");
        }
    }
}
