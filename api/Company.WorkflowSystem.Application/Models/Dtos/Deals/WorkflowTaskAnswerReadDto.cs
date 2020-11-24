using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Linq;

namespace Company.WorkflowSystem.Application.Models.Dtos.Deals
{
    public class WorkflowTaskAnswerReadDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// if an attachment of this type was added to the deal, this answer will be selected automatically.
        /// </summary>
        public int? AttachmentTypeToVerifyId { get; set; }
        /// <summary>
        /// This Workflow Action will be fired instead of the expected one, if this answer is selected. 
        /// This is useful for when a answer 'No' should cause the Deal to move back to a previous status.
        /// </summary>
        public int? AlternateWorkflowActionId { get; set; }
        /// <summary>
        /// When this answer is selected, these are the following tasks that will be presented to the user during that same action
        /// </summary>
        public List<int> SubsequentWorkflowTaskIds { get; set; } = new List<int>();
        public WorkflowTaskAnswerTypeEnum? AnswerType { get; set; }

        internal static Expression<Func<WorkflowTaskAnswer, WorkflowTaskAnswerReadDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new WorkflowTaskAnswerReadDto()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    AttachmentTypeToVerifyId = entity.AttachmentTypeToVerifyId,
                    AlternateWorkflowActionId = entity.AlternateWorkflowActionId,
                    SubsequentWorkflowTaskIds = entity.SubsequentTasks.AsQueryable().Select(st => st.Id).ToList(),
                    AnswerType = entity.AnswerType,
                };
            }
        }
    }
}