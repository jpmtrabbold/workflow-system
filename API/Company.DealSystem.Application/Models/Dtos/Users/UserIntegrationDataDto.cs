using System;
using System.Linq.Expressions;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using InversionRepo.Interfaces; 
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Users
{
    public class UserIntegrationDataDto : UpdatableListItemDto, IPersistableDto<UserIntegrationData, BaseService>
    {
        public IntegrationTypeEnum IntegrationType { get; set; }
        public UserIntegrationFieldEnum Field { get; set; }
        public Updatable<string> Data { get; set; }
        public Updatable<bool> Active { get; set; }

        internal static Expression<Func<UserIntegrationData, UserIntegrationDataDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserIntegrationDataDto()
                {
                    Id = entity.Id,
                    IntegrationType = entity.IntegrationType,
                    Field = entity.Field,
                    Data = Updatable.Create(entity.Data),
                    Active = Updatable.Create(entity.Active),
                };
            }
        }

        public UserIntegrationData ToEntity(UserIntegrationData entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new UserIntegrationData
                {
                    IntegrationType = IntegrationType,
                    Field = Field,
                };
            }

            if (Updatable.IsUpdated(Data))
                entity.Data = Data.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            return entity;
        }
    }
}
