using System.Collections.Generic;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class WorkflowStatus : BaseEntity
    {
        /// <summary>
        /// parent workflow set 
        /// </summary>
        public int WorkflowSetId { get; set; }
        /// <summary>
        /// parent workflow set 
        /// </summary>
        public WorkflowSet WorkflowSet { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }

        public WorkflowAssignmentTypeEnum AssignmentType { get; set; }

        /// <summary>
        /// if this status is always assigned to an specific role, this has to be defined
        /// </summary>
        public int? WorkflowRoleId { get; set; }
        /// <summary>
        /// if this status is always assigned to an specific role, this has to be defined
        /// </summary>
        public WorkflowRole WorkflowRole { get; set; }

        /// <summary>
        /// usually the first status allows deal editing
        /// </summary>
        public bool AllowsDealEditing { get; set; } = false;
        /// <summary>
        /// whether this is the last status of a deal
        /// </summary>
        public bool FinalizeDeal { get; set; } = false;
        /// <summary>
        /// whether this status renders the deal cancelled
        /// </summary>
        public bool CancelDeal { get; set; } = false;
        /// <summary>
        /// whether this status permits setting a Delegated Trading Authority
        /// </summary>
        public bool AllowsEditDelegatedAuthority { get; set; } = false;

        /// <summary>
        /// Whether the Deal can be executed while the deal is in this status
        /// </summary>
        public bool AllowsDealExecution { get; set; } = false;

        /// <summary>
        /// All the actions that lead to this status
        /// </summary>
        public ICollection<WorkflowAction> ActionsFromThisSource { get; set; } = new List<WorkflowAction>();
        /// <summary>
        /// all the possible actions that can be taken when a deal is in this status
        /// </summary>
        public ICollection<WorkflowAction> ActionsToThisTarget { get; set; } = new List<WorkflowAction>();
    }
}