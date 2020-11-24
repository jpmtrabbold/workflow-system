using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowTask : DeactivatableBaseEntity
    {
        /// <summary>
        /// Parent Action id
        /// </summary>
        public int WorkflowActionId { get; set; }
        /// <summary>
        /// parent action
        /// </summary>
        public WorkflowAction WorkflowAction { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public WorkflowTaskTypeEnum Type { get; set; }
        /// <summary>
        /// whether completing this task is mandatory
        /// </summary>
        public bool Mandatory { get; set; }
        /// <summary>
        /// Deal types to which this task applies
        /// </summary>
        public ICollection<WorkflowTaskInDealType> DealTypesInWorkflowTask { get; set; } = new List<WorkflowTaskInDealType>();

        public ICollection<WorkflowTaskAnswer> PossibleAnswers { get; set; } = new List<WorkflowTaskAnswer>();
        /// <summary>
        /// Answer that lead to this task in the same action
        /// </summary>
        public int? PrecedingAnswerId { get; set; }
        /// <summary>
        /// Answer that lead to this task in the same action
        /// </summary>
        public WorkflowTaskAnswer PrecedingAnswer { get; set; }

        /// <summary>
        /// Answer that lead to this task in a previous status
        /// </summary>
        public int? DependingUponAnswerId { get; set; }
        /// <summary>
        /// Answer that lead to this task in a previous status
        /// </summary>
        public WorkflowTaskAnswer DependingUponAnswer { get; set; }
    }
}