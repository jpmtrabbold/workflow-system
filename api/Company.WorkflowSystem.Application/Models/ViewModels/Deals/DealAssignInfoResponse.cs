using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Deals
{
    public class DealAssignInfoResponse
    {
        public bool AssignedToSelf { get; set; }
        public bool AssignedToUsersRole { get; set; }
        public string AssignedToUserDescription { get; set; }
        public int? AssignedToUserId { get; set; }
        public DealAssignRevertBackInfoResponse RevertBackInfo { get; set; } = new DealAssignRevertBackInfoResponse
        {
            CanRevertStatusBack = false
        };
    }

    public class DealAssignRevertBackInfoResponse
    {
        public bool CanRevertStatusBack { get; set; }
        public int CurrentDealWorkflowStatusId { get; set; }
        public string CurrentDealWorkflowStatusName { get; set; }
        public int PreviousWorkflowStatusId { get; set; }
        public string PreviousDealWorkflowStatusName { get; set; }
        public string PrecedingWorkflowActionName { get; set; }
        public string CurrentWorkflowRoleName { get; set; }
    }
}
