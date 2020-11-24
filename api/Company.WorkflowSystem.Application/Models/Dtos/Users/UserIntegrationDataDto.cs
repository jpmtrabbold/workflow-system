using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using InversionRepo.Interfaces; 
using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Application.Models.Dtos.Users
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
