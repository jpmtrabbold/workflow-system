using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Exceptions;
using Company.WorkflowSystem.Service.Models.Dtos.DealCategories;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Service.Models.ViewModels.DealCategories;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using System.Linq;
using Company.WorkflowSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Service.Services
{
    public class DealCategoryService : BaseService
    {
        public DealCategoryService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<DealCategoriesListResponse> List(DealCategoriesListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(DealCategoryListDto.ProjectionFromEntity, listRequest)
                .OrderBy(c => c.Id, descending: true)
                .ConditionalOrder("name", c => c.Name)
                .ConditionalOrder("unitOfMeasure", c => c.UnitOfMeasure)
                .ConditionalOrder("active", c => c.Active);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c => c.Name.Contains(str) 
                || c.UnitOfMeasure.Contains(str));
            }

            return new DealCategoriesListResponse
            {
                DealCategories = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        async public Task<DealCategoryDto> Get(int dealCategoryId)
        {
            var dealCategory = await _repo.ProjectedGetById(dealCategoryId, DealCategoryDto.ProjectionFromEntity);

            return dealCategory;
        }

        async public Task<DealCategoryPostResponse> Save(DealCategoryDto dealCategory, int? userId = null)
        {
            Validate(dealCategory);

            var creation = !dealCategory.Id.HasValue || dealCategory.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<DealCategory>(dealCategory.Id);

            entity = dealCategory.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.DealCategories);

            return new DealCategoryPostResponse { DealCategoryName = entity.Name };
        }

        void Validate(DealCategoryDto dealCategory)
        {
            if (Updatable.IsUpdatedButEmpty(dealCategory.Name))
                throw new BusinessRuleException("Please enter a deal category name.");
            
            if (_repo.Context.DealCategories.Any(c => c.Id != dealCategory.Id && c.Name == dealCategory.Name.Value))
                throw new BusinessRuleException($"There is another deal category using {dealCategory.Name.Value} as a name.", "Names must be unique");
        }
    }
}
