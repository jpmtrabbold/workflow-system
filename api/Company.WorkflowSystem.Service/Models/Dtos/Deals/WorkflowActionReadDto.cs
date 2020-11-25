using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using LinqKit;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class WorkflowActionReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public WorkflowStatusReadDto TargetWorkflowStatus { get; set; }
        public bool IsSubmission { get; set; }

        internal static Expression<Func<WorkflowAction, WorkflowActionReadDto>> ProjectionFromEntity() =>
            entity => new WorkflowActionReadDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                TargetWorkflowStatus = WorkflowStatusReadDto.ProjectionFromEntity.Invoke(entity.TargetWorkflowStatus),
            };
    }
}