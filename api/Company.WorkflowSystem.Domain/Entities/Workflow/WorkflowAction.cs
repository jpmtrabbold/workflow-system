using System.Collections.Generic;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowAction : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// status id in which this action may be taken
        /// </summary>
        public int? SourceWorkflowStatusId { get; set; }
        /// <summary>
        /// status in which this action may be taken
        /// </summary>
        public WorkflowStatus SourceWorkflowStatus { get; set; }

        /// <summary>
        /// status id that this action leads towards
        /// </summary>
        public int TargetWorkflowStatusId { get; set; }
        /// <summary>
        /// status that this action leads towards
        /// </summary>
        public WorkflowStatus TargetWorkflowStatus { get; set; }
        /// <summary>
        /// Description suffix for target status
        /// </summary>
        public string TargetAlternateDescriptionSuffix { get; set; }
        /// <summary>
        /// Whether this action can be directly actioned through the email notification
        /// </summary>
        public bool DirectActionOnEmailNotification { get; set; } = false;
        /// <summary>
        /// whether this task refers to a deal submission
        /// </summary>
        public bool IsSubmission { get; set; } = false;
        /// <summary>
        /// if true, by taking this action, the deal becomes "Executed" automatically
        /// </summary>
        public bool PerformsExecutionAutomatically { get; set; } = false;
        /// <summary>
        /// Tasks that need to be performed in order to this action be accomplished
        /// </summary>
        public ICollection<WorkflowTask> Tasks { get; private set; } = new List<WorkflowTask>();
        /// <summary>
        /// any actions that have this flag as true can't be performed by the same user
        /// </summary>
        public bool CantBePerformedBySameUser { get; set; } = false;

    }
}