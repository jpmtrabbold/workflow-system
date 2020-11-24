using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowTaskAnswer : DeactivatableBaseEntity
    {
        /// <summary>
        /// parent task id
        /// </summary>
        public int WorkflowTaskId { get; set; }
        /// <summary>
        /// parent task
        /// </summary>
        public WorkflowTask WorkflowTask { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// if an attachment of this type was added to the deal, this answer will be selected automatically.
        /// </summary>
        public int? AttachmentTypeToVerifyId { get; set; }
        /// <summary>
        /// if an attachment of this type was added to the deal, this answer will be selected automatically.
        /// </summary>
        public AttachmentType AttachmentTypeToVerify { get; set; }
        /// <summary>
        /// This Workflow Action will be fired instead of the expected one, if this answer is selected. 
        /// This is useful for when a answer 'No' should cause the Deal to move back to a previous status.
        /// </summary>
        public int? AlternateWorkflowActionId { get; set; }
        /// <summary>
        /// This Workflow Action will be fired instead of the expected one, if this answer is selected. 
        /// This is useful for when a answer 'No' should cause the Deal to move back to a previous status.
        /// </summary>
        public WorkflowAction AlternateWorkflowAction { get; set; }
        /// <summary>
        /// some workflow tasks have intelligence to answer based on custom evaluation. For that to happen,
        /// the routines need to know which answer is Yes or No (or something else).
        /// </summary>
        public WorkflowTaskAnswerTypeEnum? AnswerType { get; set; }
        /// <summary>
        /// When this answer is selected, these are the following tasks that will be presented to the user during that same action
        /// </summary>
        public ICollection<WorkflowTask> SubsequentTasks { get; set; } = new List<WorkflowTask>();
        /// <summary>
        /// When this answer is selected, these are the tasks that will be presented to the user in future actions
        /// </summary>
        public ICollection<WorkflowTask> DependentTasks { get; set; } = new List<WorkflowTask>();
    }
}