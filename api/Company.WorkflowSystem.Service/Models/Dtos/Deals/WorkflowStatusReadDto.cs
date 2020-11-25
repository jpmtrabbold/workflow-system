using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Domain.ExtensionMethods;
using Company.WorkflowSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class WorkflowStatusReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AllowsDealEditing { get; set; } = false;
        /// <summary>
        /// whether this status permits setting a Delegated Trading Authority
        /// </summary>
        public bool AllowsEditDelegatedAuthority { get; set; } = false;

        public List<WorkflowActionReadDto> Actions { get; set; } = new List<WorkflowActionReadDto>();
        public bool FinalizeDeal { get; set; }

        internal static Expression<Func<WorkflowStatus, WorkflowStatusReadDto>> ProjectionFromEntity
        {
            get => entity => new WorkflowStatusReadDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                AllowsDealEditing = entity.AllowsDealEditing,
                AllowsEditDelegatedAuthority = entity.AllowsEditDelegatedAuthority,
                Actions = entity.ActionsFromThisSource.AsQueryable().Where(a => a.Active).Select(WorkflowActionReadDto.ProjectionFromEntity()).ToList(),
                FinalizeDeal = entity.FinalizeDeal,
            };
        }
    }
}