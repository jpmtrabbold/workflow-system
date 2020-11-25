using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Counterparties
{
    public class CounterpartyDto : UpdatableListItemDto, IPersistableDto<Counterparty, BaseService>
    {
        public Updatable<string> Name { get; set; }
        public Updatable<string> Code { get; set; }
        public Updatable<bool> Active { get; set; }
        public Updatable<int?> CountryId { get; set; }
        public Updatable<string> CompanyNumber { get; set; }
        public Updatable<string> BusinessNumber { get; set; }
        public Updatable<bool?> NzemParticipant { get; set; }
        public Updatable<string> NzemParticipantId { get; set; }
        public Updatable<string> Conditions { get; set; }
        public Updatable<decimal> ExposureLimit { get; set; }
        public Updatable<DateTimeOffset?> ExpiryDate { get; set; }
        public Updatable<DateTimeOffset?> ApprovalDate { get; set; }
        public Updatable<string> SecurityHeld{ get; set; }

        public List<int> DealCategories { get; set; }

        internal static Expression<Func<Counterparty, CounterpartyDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new CounterpartyDto()
                {
                    Id = entity.Id,
                    Name = Updatable.Create(entity.Name),
                    Code = Updatable.Create(entity.Code),
                    Active = Updatable.Create(entity.Active),
                    CountryId = Updatable.Create(entity.CountryId),
                    CompanyNumber = Updatable.Create(entity.CompanyNumber),
                    BusinessNumber = Updatable.Create(entity.BusinessNumber),
                    NzemParticipant = Updatable.Create(entity.NzemParticipant),
                    NzemParticipantId = Updatable.Create(entity.NzemParticipantId),
                    Conditions = Updatable.Create(entity.Conditions),
                    ExposureLimit = Updatable.Create(entity.ExposureLimit),
                    ExpiryDate = Updatable.Create(entity.ExpiryDate),
                    ApprovalDate = Updatable.Create(entity.ApprovalDate),
                    SecurityHeld = Updatable.Create(entity.SecurityHeld),
                    DealCategories = entity.DealCategories.AsQueryable().Select(pt => pt.DealCategoryId).ToList()
                };
            }
        }

        public Counterparty ToEntity(Counterparty entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new Counterparty
                {
                    
                };
            }

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(Code))
                entity.Code = Code.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            if (Updatable.IsUpdated(CountryId))
                entity.CountryId = CountryId.Value;

            if (Updatable.IsUpdated(CompanyNumber))
                entity.CompanyNumber = CompanyNumber.Value;

            if (Updatable.IsUpdated(BusinessNumber))
                entity.BusinessNumber = BusinessNumber.Value;

            if (Updatable.IsUpdated(NzemParticipant))
                entity.NzemParticipant = NzemParticipant.Value;

            if (Updatable.IsUpdated(NzemParticipantId))
                entity.NzemParticipantId = NzemParticipantId.Value;

            if (Updatable.IsUpdated(Conditions))
                entity.Conditions = Conditions.Value;

            if (Updatable.IsUpdated(ExposureLimit))
                entity.ExposureLimit = ExposureLimit.Value;

            if (Updatable.IsUpdated(ExpiryDate))
                entity.ExpiryDate = ExpiryDate.Value;

            if (Updatable.IsUpdated(ApprovalDate))
                entity.ApprovalDate = ApprovalDate.Value;

            if (Updatable.IsUpdated(SecurityHeld))
                entity.SecurityHeld = SecurityHeld.Value;

            foreach (var dealCategoryIdFromClient in DealCategories)
                if (!entity.DealCategories.Any(pt => pt.DealCategoryId == dealCategoryIdFromClient))
                    entity.DealCategories.Add(new CounterpartyInDealCategory { DealCategoryId = dealCategoryIdFromClient });

            foreach (var dealCategoryFromDatabase in entity.DealCategories)
                if (!DealCategories.Any(id => id == dealCategoryFromDatabase.DealCategoryId))
                    service._repo.Remove(dealCategoryFromDatabase);

            return entity;
        }
    }
}
