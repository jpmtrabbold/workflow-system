﻿using System;
using System.Linq.Expressions;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using InversionRepo.Interfaces; 
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Users
{
    public class UserInWorkflowRoleDto : UpdatableListItemDto, IPersistableDto<UserInWorkflowRole, BaseService>
    {
        public Updatable<int> UserId { get; set; }
        public string UserName { get; set; }
        public Updatable<int> WorkflowRoleId { get; set; }
        public string WorkflowRoleName { get; set; }
        public Updatable<bool> Active { get; set; }

        internal static Expression<Func<UserInWorkflowRole, UserInWorkflowRoleDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserInWorkflowRoleDto()
                {
                    Id = entity.Id,
                    UserId = Updatable.Create(entity.UserId),
                    UserName = entity.User.Name,
                    WorkflowRoleId = Updatable.Create(entity.WorkflowRoleId),
                    WorkflowRoleName = entity.WorkflowRole.Name,
                    Active = Updatable.Create(entity.Active),
                };
            }
        }

        public UserInWorkflowRole ToEntity(UserInWorkflowRole entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new UserInWorkflowRole
                {

                };
            }

            if (Updatable.IsUpdated(UserId))
                entity.UserId = UserId.Value;

            if (Updatable.IsUpdated(WorkflowRoleId))
                entity.WorkflowRoleId = WorkflowRoleId.Value;

            if (Updatable.IsUpdated(Active))
                entity.Active = Active.Value;

            return entity;
        }
    }
}
