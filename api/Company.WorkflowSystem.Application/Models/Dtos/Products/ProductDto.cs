using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Application.Models.Dtos.Products
{
    public class ProductDto : UpdatableListItemDto, IPersistableDto<Product, BaseService>
    {
        public Updatable<string> Name { get; set; }
        public Updatable<bool> Active { get; set; }
        public Updatable<int> DealCategoryId { get; set; }

        internal static Expression<Func<Product, ProductDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new ProductDto()
                {
                    Id = entity.Id,
                    Name = Updatable.Create(entity.Name),
                    DealCategoryId = Updatable.Create(entity.DealCategoryId),
                    Active = Updatable.Create(entity.Active),
                };
            }
        }

        public Product ToEntity(Product entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new Product
                {

                };
            }

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(DealCategoryId))
                entity.DealCategoryId = DealCategoryId.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            return entity;
        }
    }
}
