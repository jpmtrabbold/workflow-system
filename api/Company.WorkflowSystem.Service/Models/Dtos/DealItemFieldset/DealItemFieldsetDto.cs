using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.DealItemFieldsets
{
    public class DealItemFieldsetDto : UpdatableListItemDto, IPersistableDto<DealItemFieldset, BaseService>
    {
        public Updatable<string> Name { get; set; }
        public Updatable<string> Description { get; set; }

        public List<DealItemFieldDto> Fields { get; set; } = new List<DealItemFieldDto>();

        internal static Expression<Func<DealItemFieldset, DealItemFieldsetDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealItemFieldsetDto()
                {
                    Id = entity.Id,
                    Name = Updatable.Create(entity.Name),
                    Description = Updatable.Create(entity.Description),
                    Fields = entity.ItemFields.AsQueryable().OrderBy(field => field.DisplayOrder).Select(DealItemFieldDto.ProjectionFromEntity).ToList(),
                };
            }
        }

        public DealItemFieldset ToEntity(DealItemFieldset entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new DealItemFieldset
                {

                };
            }

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(Description))
                entity.Description = Description.Value;

            Updatable.ToEntityCollection(Fields, entity.ItemFields, service);

            return entity;
        }
    }
}
