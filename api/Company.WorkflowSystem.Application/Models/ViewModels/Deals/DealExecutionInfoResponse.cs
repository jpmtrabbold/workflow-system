using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Deals
{
    public class DealExecutionInfoResponse
    {
        public bool AssignedToSelf { get; set; }
        public string AssignedToUserName { get; set; }
        public int? AssignedToUserId { get; set; }
        public bool Executed { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
    }
}
