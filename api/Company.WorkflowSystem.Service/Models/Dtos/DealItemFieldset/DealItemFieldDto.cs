using System;
using System.Collections.Generic;
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
    public class DealItemFieldDto : UpdatableListItemDto, IPersistableDto<DealItemField, BaseService>
    {
        public Updatable<int> DisplayOrder { get; set; }
        public Updatable<string> Name { get; set; }
        public Updatable<string> Field { get; set; }
        public Updatable<bool> Execution { get; set; }

        internal static Expression<Func<DealItemField, DealItemFieldDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealItemFieldDto()
                {
                    Id = entity.Id,
                    DisplayOrder = Updatable.Create(entity.DisplayOrder),
                    Name = Updatable.Create(entity.Name),
                    Field = Updatable.Create(entity.Field),
                    Execution = Updatable.Create(entity.Execution),
                };
            }
        }

        public DealItemField ToEntity(DealItemField entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new DealItemField
                {

                };
            }

            if (Updatable.IsUpdated(DisplayOrder))
                entity.DisplayOrder = DisplayOrder.Value;

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(Field))
                entity.Field = Field.Value;

            if (Updatable.IsUpdated(Execution))
                entity.Execution = Execution.Value;

            return entity;
        }
    }
}
