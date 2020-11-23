using System.Collections.Generic;
using System.Linq;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Infrastructure;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.ViewModels.Users;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Application.Models.ViewModels.Configuration;
using Company.DealSystem.Application.Models.Dtos.Configuration;
using Company.DealSystem.Domain.Entities.Configuration;
using System;
using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Domain.Services;
using System.Transactions;

namespace Company.DealSystem.Application.Services
{
    public class ConfigurationService : BaseService
    {
        public ConfigurationService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<ConfigurationGroupsListResponse> List(ConfigurationGroupsListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(ConfigurationGroupsListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Name)
                .ConditionalOrder("id", c => c.Id)
                .ConditionalOrder("name", c => c.Name)
                .ConditionalOrder("description", c => c.Description);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c => 
                c.Name.Contains(str)
                || c.Description.Contains(str)
                );
            }

            return new ConfigurationGroupsListResponse
            {
                ConfigurationGroups = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        async public Task<ConfigurationGroupDto> Get(int configurationGroupId)
        {
            var group = await _repo.ProjectedGetById(configurationGroupId, ConfigurationGroupDto.ProjectionFromEntity);

            return group;
        }

        async public Task<ConfigurationGroupPostResponse> Save(ConfigurationGroupDto model, int? userId = null)
        {
            Validate(model);

            var creation = !model.Id.HasValue || model.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<ConfigurationGroup>(model.Id);
            
            if (!creation)
            {
                _repo.LoadCollection(entity, d => d.Entries);
            }

            entity = model.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Configuration);

            return new ConfigurationGroupPostResponse { Name = entity.Name };
        }

        public void Validate(ConfigurationGroupDto user)
        {
            // throw any exceptions if invalid
        }
    }
}