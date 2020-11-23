﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Company.DealSystem.Application.Exceptions;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.Dtos.DealItemFieldsets;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Models.ViewModels.ItemFieldsets;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using System.Linq;
using Company.DealSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Application.Services
{
    public class DealItemFieldsetService : BaseService
    {
        public DealItemFieldsetService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<DealItemFieldsetsListResponse> List(DealItemFieldsetsListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(DealItemFieldsetListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("description", c => c.Description);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c => c.Description.Contains(str));
            }

            return new DealItemFieldsetsListResponse
            {
                ItemFieldsets = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        public List<StringLookupRequest> GetItemFieldLookups()
        {
            return DealItemDto.ItemFieldLookups;
        }

        async public Task<DealItemFieldsetDto> Get(int itemFieldsetId)
        {
            var itemFieldset = await _repo.ProjectedGetById(itemFieldsetId, DealItemFieldsetDto.ProjectionFromEntity);

            return itemFieldset;
        }

        async public Task<DealItemFieldsetPostResponse> Save(DealItemFieldsetDto itemFieldset, int? userId = null)
        {
            Validate(itemFieldset);

            var creation = !itemFieldset.Id.HasValue || itemFieldset.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<DealItemFieldset>(itemFieldset.Id);
            if (!creation)
            {
                _repo.LoadCollection(entity, d => d.ItemFields);
            }

            entity = itemFieldset.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.DealItemFieldsets);

            return new DealItemFieldsetPostResponse { DealItemFieldsetName = entity.Name };
        }

        void Validate(DealItemFieldsetDto itemFieldset)
        {
            if (Updatable.IsUpdatedButEmpty(itemFieldset.Description))
                throw new BusinessRuleException("Please enter a description.");

            if (_repo.Context.DealItemFieldsets.Any(c => c.Id != itemFieldset.Id && c.Name == itemFieldset.Name.Value))
                throw new BusinessRuleException($"There is another item fieldset using {itemFieldset.Name.Value} as a name.", "Names must be unique");

        }
    }
}
