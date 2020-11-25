using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Domain.Models.Enum;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.DealTypes
{
    public class DealTypeDto : UpdatableListItemDto, IPersistableDto<DealType, BaseService>
    {
        public Updatable<string> Name { get; set; }
        public Updatable<PositionEnum> Position { get; set; }
        public Updatable<bool> ForcePosition { get; set; }
        public Updatable<string> UnitOfMeasure { get; set; }
        public Updatable<bool> HasLossFactors { get; set; }
        public Updatable<bool> HasExpiryDate{ get; set; }
        public Updatable<bool> HasDelegatedAuthority { get; set; }
        public Updatable<int?> DealItemFieldsetId { get; set; }
        public Updatable<int?> WorkflowSetId { get; set; }
        public Updatable<bool> Active { get; set; }
        public List<int> DealCategories { get; set; }
        
        internal static Expression<Func<DealType, DealTypeDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealTypeDto()
                {
                    Id = entity.Id,
                    Name = Updatable.Create(entity.Name),
                    Position = Updatable.Create(entity.Position),
                    ForcePosition = Updatable.Create(entity.ForcePosition),
                    UnitOfMeasure = Updatable.Create(entity.UnitOfMeasure),
                    HasLossFactors = Updatable.Create(entity.HasLossFactors),
                    HasExpiryDate = Updatable.Create(entity.HasExpiryDate),
                    HasDelegatedAuthority = Updatable.Create(entity.HasDelegatedAuthority),
                    DealItemFieldsetId = Updatable.Create(entity.DealItemFieldsetId),
                    WorkflowSetId = Updatable.Create(entity.WorkflowSetId),
                    Active = Updatable.Create(entity.Active),
                    DealCategories = entity.DealCategoriesInDealType.AsQueryable().Select(pt => pt.DealCategoryId).ToList()
                };
            }
        }

        public DealType ToEntity(DealType entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new DealType
                {

                };
            }

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(ForcePosition))
                entity.ForcePosition = ForcePosition.Value;

            if (Updatable.IsUpdated(Position))
                entity.Position = Position.Value;

            if (Updatable.IsUpdated(UnitOfMeasure))
                entity.UnitOfMeasure = UnitOfMeasure.Value;

            if (Updatable.IsUpdated(HasExpiryDate))
                entity.HasExpiryDate = HasExpiryDate.Value;
            
            if (Updatable.IsUpdated(HasDelegatedAuthority))
                entity.HasDelegatedAuthority = HasDelegatedAuthority.Value;

            if (Updatable.IsUpdated(HasLossFactors))
                entity.HasLossFactors = HasLossFactors.Value;

            if (Updatable.IsUpdated(DealItemFieldsetId))
                entity.DealItemFieldsetId = DealItemFieldsetId.Value;

            if (Updatable.IsUpdated(WorkflowSetId))
                entity.WorkflowSetId = WorkflowSetId.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            foreach (var dealCategoryIdFromClient in DealCategories)
                if (!entity.DealCategoriesInDealType.Any(pt => pt.DealCategoryId == dealCategoryIdFromClient))
                    entity.DealCategoriesInDealType.Add(new DealTypeInDealCategory { DealCategoryId = dealCategoryIdFromClient });

            foreach (var dealCategoryFromDatabase in entity.DealCategoriesInDealType)
                if (!DealCategories.Any(id => id == dealCategoryFromDatabase.DealCategoryId))
                    service._repo.Remove(dealCategoryFromDatabase);

            return entity;
        }
    }
}
