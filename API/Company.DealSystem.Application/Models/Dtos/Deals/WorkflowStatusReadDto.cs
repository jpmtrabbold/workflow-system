using System;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Domain.ExtensionMethods;
using Company.DealSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using LinqExpander;

namespace Company.DealSystem.Application.Models.Dtos.Deals
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

        [ReplaceWithExpression(MethodName = nameof(ProjectionFromEntity))]
        internal static WorkflowStatusReadDto FromEntity(WorkflowStatus entity) => ProjectionFromEntity().Compile().Invoke(entity);
        internal static Expression<Func<WorkflowStatus, WorkflowStatusReadDto>> ProjectionFromEntity() =>
            entity => new WorkflowStatusReadDto()
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