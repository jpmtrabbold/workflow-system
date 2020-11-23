using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.DealCategories
{
    public class DealCategoryDto : UpdatableListItemDto, IPersistableDto<DealCategory, BaseService>
    {
        public Updatable<string> Name { get; set; }
        public Updatable<string> UnitOfMeasure { get; set; }
        public Updatable<bool> Active { get; set; }

        internal static Expression<Func<DealCategory, DealCategoryDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealCategoryDto()
                {
                    Id = entity.Id,
                    Name = Updatable.Create(entity.Name),
                    UnitOfMeasure = Updatable.Create(entity.UnitOfMeasure),
                    Active = Updatable.Create(entity.Active),
                };
            }
        }

        public DealCategory ToEntity(DealCategory entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new DealCategory
                {

                };
            }

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(UnitOfMeasure))
                entity.UnitOfMeasure = UnitOfMeasure.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            return entity;
        }
    }
}
