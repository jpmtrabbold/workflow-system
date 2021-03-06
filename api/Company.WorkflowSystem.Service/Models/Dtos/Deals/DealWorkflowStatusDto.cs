﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;

using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealWorkflowStatusDto : UpdatableListItemDto, IPersistableDto<DealWorkflowStatus, DealService>
    {
        public int WorkflowStatusId { get; set; }
        public string WorkflowStatusName { get; set; }
        public int? AssigneeUserId { get; set; }
        public string AssigneeUserName { get; set; }
        public Updatable<int?> AssigneeWorkflowRoleId { get; set; }
        public string AssigneeWorkflowRoleName { get; set; }
        public WorkflowAssignmentTypeEnum AssignmentType { get; set; }
        public DateTimeOffset? DateTimeConfirmed { get; set; }
        public DateTimeOffset? DateTimeCreated { get; set; }
        public string InitiatedByUserName { get; set; }
        public bool AllowsDealExecution { get; set; }
        public List<DealWorkflowTaskDto> Tasks { get; set; } = new List<DealWorkflowTaskDto>();
        internal static Expression<Func<DealWorkflowStatus, DealWorkflowStatusDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealWorkflowStatusDto()
                {
                    Id = entity.Id,
                    WorkflowStatusId = entity.WorkflowStatusId,
                    WorkflowStatusName = entity.WorkflowStatusName,
                    AssigneeUserId = entity.AssigneeUserId,
                    AssigneeUserName = entity.AssigneeUser.Name,
                    AssigneeWorkflowRoleId = Updatable.Create(entity.AssigneeWorkflowRoleId),
                    AssigneeWorkflowRoleName = entity.AssigneeWorkflowRole.Name,
                    AssignmentType = entity.WorkflowStatus.AssignmentType,
                    DateTimeConfirmed = entity.DateTimeConfirmed,
                    DateTimeCreated = entity.DateTimeCreated,
                    InitiatedByUserName = (entity.InitiatedByUserId.HasValue ? entity.InitiatedByUser.Name : null),
                    AllowsDealExecution = entity.WorkflowStatus.AllowsDealExecution,
                    Tasks = entity.Tasks.AsQueryable().OrderBy(t => t.WorkflowTask.Order).Select(DealWorkflowTaskDto.ProjectionFromEntity).ToList(),
                };
            }
        }

        public DealWorkflowStatus ToEntity(DealWorkflowStatus entity, DealService service)
        {
            if (entity == null)
            {
                entity = new DealWorkflowStatus
                {
                    WorkflowStatusId = WorkflowStatusId,
                    WorkflowStatusName = WorkflowStatusName,
                    AssigneeUserId = AssigneeUserId,
                };
            }

            if (Updatable.IsUpdated(AssigneeWorkflowRoleId))
                entity.AssigneeWorkflowRoleId = AssigneeWorkflowRoleId.Value;

            Updatable.ToEntityCollection(Tasks, entity.Tasks, service);

            return entity;
        }
    }
}