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
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Application.Services
{
    public class ProductService : BaseService
    {
        public ProductService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<ProductsListResponse> List(ProductsListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(ProductListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("name", c => c.Name)
                .ConditionalOrder("dealCategory", c => c.DealCategory)
                .ConditionalOrder("active", c => c.Active);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c => c.Name.Contains(str)
                || c.DealCategory.Name.Contains(str));
            }

            return new ProductsListResponse
            {
                Products = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        async public Task<ProductDto> Get(int productId)
        {
            var product = await _repo.ProjectedGetById(productId, ProductDto.ProjectionFromEntity);

            return product;
        }

        async public Task<ProductPostResponse> Save(ProductDto product, int? userId = null)
        {
            Validate(product);

            var creation = !product.Id.HasValue || product.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<Product>(product.Id);

            entity = product.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Products);

            return new ProductPostResponse { ProductName = entity.Name };
        }

        async public Task<List<LookupRequest>> GetProducts(ProductDto product, int dealCategoryId, string query)
        {
            var products = _repo.ProjectedListBuilder(LookupRequest.ProjectionFromProduct);

            products.Where(n => n.Id != product.Id &&
                (n.DealCategoryId == dealCategoryId) &&
                (query == null || n.Name.Contains(query.Trim()))
                );

            products.OrderBy(n => n.Name);

            return await products.ExecuteAsync();
        }

        public void Validate(ProductDto product)
        {
            var creation = !product.Id.HasValue || product.Id == 0;
            if (Updatable.IsUpdatedButEmpty(product.Name))
                throw new BusinessRuleException("Please enter a product name.");

            if (_repo.Context.Products.Any(c => c.Id != product.Id && c.Name == product.Name.Value))
                throw new BusinessRuleException($"There is another product using {product.Name.Value} as a name.", "Names must be unique");

            if (!Updatable.IsUpdated(product.DealCategoryId) && creation)
                product.DealCategoryId.Updated = true;

            if (!Updatable.IsUpdated(product.Active) && creation)
                product.Active.Updated = true;

        }
    }
}
