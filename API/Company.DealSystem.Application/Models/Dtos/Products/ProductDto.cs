using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Products
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
