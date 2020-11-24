using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Domain.Entities
{
    public partial class Deal : BaseEntity
    {
        public DateTimeOffset CreationDate { get; set; } = DateUtils.GetDateTimeOffsetNow();
        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }

        [Required]
        public string DealNumber { get; set; }

        [Required]
        public int CounterpartyId { get; set; }
        public Counterparty Counterparty { get; set; }

        [Required]
        public int DealCategoryId { get; set; }
        public DealCategory DealCategory { get; set; }

        [Required]
        public int DealTypeId { get; set; }
        public DealType DealType { get; set; }

        public bool ForceMajeure { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }

        /// <summary>
        /// Collection of dynamic fields that is to be used for deal items.
        /// </summary>
        public int? DealItemFieldsetId { get; set; }

        /// <summary>
        /// Id of Workflow set that is being used in this deal, that defines the Deal Life-Cycle
        /// </summary>
        public int? WorkflowSetId { get; set; }
        /// <summary>
        /// Workflow set that is being used in this deal, that defines the Deal Life-Cycle
        /// </summary>
        public WorkflowSet WorkflowSet { get; set; }
        /// <summary>
        /// All Workflow Statuses that this deal been through. Be nice, a deal might have gone through a lot.
        /// </summary>
        public ICollection<DealWorkflowStatus> DealWorkflowStatuses { get; private set; } = new List<DealWorkflowStatus>();
        /// <summary>
        /// The latest and current workflow status id of the deal
        /// </summary>
        public int? CurrentDealWorkflowStatusId { get; set; }
        /// <summary>
        /// The latest and current workflow status of the deal 
        /// </summary>
        public DealWorkflowStatus CurrentDealWorkflowStatus { get; set; }
        /// <summary>
        /// If the user started an action, this will hold the workflow action id
        /// </summary>
        public int? OngoingWorkflowActionId { get; set; }
        /// <summary>
        /// If the user started an action, this will hold the workflow action
        /// </summary>
        public WorkflowAction OngoingWorkflowAction { get; set; }
        /// <summary>
        /// if the user started an action, this will hold what is the next status id if the action is done
        /// </summary>
        public int? NextDealWorkflowStatusId { get; set; }
        /// <summary>
        /// if the user started an action, this will hold what is the next status if the action is done
        /// </summary>
        public DealWorkflowStatus NextDealWorkflowStatus { get; set; }
        /// <summary>
        /// when the deal moves statuses, this will hold which was the previous status (so the user that created the new status can revert back)
        /// </summary>
        public int? PreviousDealWorkflowStatusId { get; set; }
        /// <summary>
        /// when the deal moves statuses, this will hold which was the previous status (so the user that created the new status can revert back)
        /// </summary>
        public DealWorkflowStatus PreviousDealWorkflowStatus { get; set; }
        /// <summary>
        /// if the trader used a delegated authority from another user, this will be the user from which the authority was delegated
        /// </summary>
        public User DelegatedAuthorityUser { get; set; }
        /// <summary>
        /// if the trader used a delegated authority from another user, this will be the user from which the authority was delegated
        /// </summary>
        public int? DelegatedAuthorityUserId { get; set; }

        /// <summary>
        /// all deal items of this deal
        /// </summary>
        public ICollection<DealItem> Items { get; private set; } = new List<DealItem>();
        /// <summary>
        /// All notes of this deal
        /// </summary>
        public ICollection<DealNote> Notes { get; private set; } = new List<DealNote>();
        /// <summary>
        /// all attachments of this deal
        /// </summary>
        public ICollection<DealAttachment> Attachments { get; private set; } = new List<DealAttachment>();
        
        /// <summary>
        /// when the deal was submitted
        /// </summary>
        public DateTimeOffset? SubmissionDate { get; set; }
        /// <summary>
        /// by whom the deal was submitted
        /// </summary>
        public int? SubmissionUserId { get; set; }
        /// <summary>
        /// by whom the deal was submitted
        /// </summary>
        public User SubmissionUser { get; set; }


        /// <summary>
        /// whether the deal is executed or not
        /// </summary>
        public bool Executed { get; set; } = false;
        /// <summary>
        /// when the deal was executed
        /// </summary>
        public DateTimeOffset? ExecutionDate { get; set; }
        /// <summary>
        /// by whom the deal was executed or is being executed
        /// </summary>
        public int? ExecutionUserId { get; set; }
        /// <summary>
        /// by whom the deal was executed or is being executed
        /// </summary>
        public User ExecutionUser { get; set; }
        public int? TermInMonthsOverride { get; set; }

    }
}
