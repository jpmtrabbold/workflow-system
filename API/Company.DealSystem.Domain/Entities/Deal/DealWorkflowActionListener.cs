using System;
using System.Collections.Generic;

namespace Company.DealSystem.Domain.Entities
{
    /// <summary>
    /// When notifications are sent out to potential approvers, a listener is created to each, with a key, awaiting an action on that deal.
    /// </summary>
    public class DealWorkflowActionListener : BaseEntity
    {
        /// <summary>
        /// the deal's workflow status that this listener refers to 
        /// </summary>
        public int DealWorkflowStatusId { get; set; }
        /// <summary>
        /// the deal's workflow status that this listener refers to 
        /// </summary>
        public DealWorkflowStatus DealWorkflowStatus { get; set; }
        /// <summary>
        /// The user that this listener is waiting for to take an action
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The user that this listener is waiting for to take an action
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// a unique guid used to authenticate the user
        /// </summary>
        public string UniqueActionGuid { get; set; }
    }
}