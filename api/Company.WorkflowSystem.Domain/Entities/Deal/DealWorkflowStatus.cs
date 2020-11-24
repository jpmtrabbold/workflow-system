using System;
using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealWorkflowStatus : BaseEntity
    {
        /// <summary>
        /// deal that hold (or held) this status
        /// </summary>
        public int DealId { get; set; }
        /// <summary>
        /// deal that hold (or held) this status
        /// </summary>
        public Deal Deal { get; set; }

        /// <summary>
        /// just a reference to the deal whenever it is the current status (for EF purposes)
        /// </summary>
        public Deal DealHasThisAsCurrent { get; set; }
        /// <summary>
        /// just a reference to the deal whenever it is the next status (for EF purposes)
        /// </summary>
        public Deal DealHasThisAsNext { get; set; }
        /// <summary>
        /// just a reference to the deal whenever it is the previous status (for EF purposes)
        /// </summary>
        public Deal DealHasThisAsPrevious { get; set; }

        /// <summary>
        /// workflow status id - from the master data
        /// </summary>
        public int WorkflowStatusId { get; set; }
        /// <summary>
        /// workflow status - from the master data
        /// </summary>
        public WorkflowStatus WorkflowStatus { get; set; }
        /// <summary>
        /// status name, copied from master data (in case it changes, the deal statuses' names stay the same)
        /// </summary>
        public string WorkflowStatusName { get; set; }

        /// <summary>
        /// user id to whom this deal is assigned to
        /// </summary>
        public int? AssigneeUserId { get; set; }
        /// <summary>
        /// user to whom this deal is assigned to
        /// </summary>
        public User AssigneeUser { get; set; }
        /// <summary>
        /// workflow role id to which this deal is assigned to
        /// </summary>
        public int? AssigneeWorkflowRoleId { get; set; }
        /// <summary>
        /// workflow role to which this deal is assigned to
        /// </summary>
        public WorkflowRole AssigneeWorkflowRole { get; set; }
        /// <summary>
        /// user id that initiated this status
        /// </summary>
        public int? InitiatedByUserId { get; set; }
        /// <summary>
        /// user that initiated this status
        /// </summary>
        public User InitiatedByUser { get; set; }
        /// <summary>
        /// when this status was created
        /// </summary>
        public DateTimeOffset DateTimeCreated { get; set; } = DateUtils.GetDateTimeOffsetNow();
        /// <summary>
        /// when this status was confirmed
        /// </summary>
        public DateTimeOffset? DateTimeConfirmed { get; set; }
        /// <summary>
        /// whether this status finalized the deal or not
        /// </summary>
        public bool Finalized { get; set; } = false;
        /// <summary>
        /// All the tasks that were taken in the preceding action so this status could happen
        /// </summary>
        public ICollection<DealWorkflowTask> Tasks { get; private set; } = new List<DealWorkflowTask>();
        /// <summary>
        /// Workflow action id that was taken to cause the deal to be in this status
        /// </summary>
        public int? PrecedingWorkflowActionId { get; set; }
        /// <summary>
        /// Workflow action that was taken to cause the deal to be in this status
        /// </summary>
        public WorkflowAction PrecedingWorkflowAction { get; set; }
        /// <summary>
        /// Defines whether this status was caused not by a common action, but by the user reverting a status back.
        /// </summary>
        public bool RevertedBackByUser { get; set; } = false;
        /// <summary>
        /// Action listeners awaiting for actions to happen on this status
        /// </summary>
        public ICollection<DealWorkflowActionListener> Listeners { get; private set; } = new List<DealWorkflowActionListener>();
    }
}