using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Application.Models.Dtos.Deals
{
    public class DealWorkflowAssignmentDto
    {
        public int WorkflowRoleId { get; set; }
        public string WorkflowRoleName { get; set; }
        public bool EnabledForSelection { get; set; } = true;
        public bool MeetsTradingPolicy { get; set; } = false;
        public int? ApprovalLevel { get; set; }
        public bool IsTraderWorkflowLevel { get; set; } = false;
        public List<TraderAuthorityPolicyAssessment> Assessments { get; set; } = new List<TraderAuthorityPolicyAssessment>();
        public TraderAuthorityPolicyAssessment AddAssessment()
        {
            var assesment = new TraderAuthorityPolicyAssessment();
            Assessments.Add(assesment);
            return assesment;
        }

        internal static Expression<Func<WorkflowRole, DealWorkflowAssignmentDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealWorkflowAssignmentDto()
                {
                    WorkflowRoleId = entity.Id,
                    WorkflowRoleName = entity.Name,
                    ApprovalLevel = entity.ApprovalLevel,
                };
            }
        }
    }
}