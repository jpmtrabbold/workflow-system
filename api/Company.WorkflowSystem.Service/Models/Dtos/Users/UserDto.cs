using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Users
{
    public class UserDto : IPersistableDto<User, BaseService>
    {
        public int? Id { get; set; }
        public Updatable<string> Username { get; set; }
        public Updatable<string> Name { get; set; }
        public Updatable<int?> UserRoleId { get; set; }
        public Updatable<bool> Active { get; set; }
        public List<UserInWorkflowRoleDto> WorkflowRolesInUser { get; private set; } = new List<UserInWorkflowRoleDto>();
        public List<UserIntegrationDataDto> IntegrationData { get; private set; } = new List<UserIntegrationDataDto>();

        internal static Expression<Func<User, UserDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserDto()
                {
                    Id = entity.Id,
                    Username = Updatable.Create(entity.Username),
                    Name = Updatable.Create(entity.Name),
                    WorkflowRolesInUser = entity.WorkflowRolesInUser.AsQueryable().Select(UserInWorkflowRoleDto.ProjectionFromEntity).ToList(),
                    IntegrationData = entity.IntegrationData.AsQueryable().Select(UserIntegrationDataDto.ProjectionFromEntity).ToList(),
                    UserRoleId = Updatable.Create(entity.UserRoleId),
                    Active = Updatable.Create(entity.Active),
                };
            }
        }

        public User ToEntity(User entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new User
                {

                };
            }

            if (Updatable.IsUpdated(Username))
                entity.Username = Username.Value;

            if (Updatable.IsUpdated(Name))
                entity.Name = Name.Value;

            if (Updatable.IsUpdated(UserRoleId))
                entity.UserRoleId = UserRoleId.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            Updatable.ToEntityCollection(WorkflowRolesInUser, entity.WorkflowRolesInUser, service);
            Updatable.ToEntityCollection(IntegrationData, entity.IntegrationData, service);

            return entity;
        }
    }
}
