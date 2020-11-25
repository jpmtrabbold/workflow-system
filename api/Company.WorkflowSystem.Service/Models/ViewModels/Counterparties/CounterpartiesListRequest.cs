using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Counterparties
{
    public class CounterpartiesListRequest : ListRequest
    {
        public int? DealId { get; set; }
        public bool? OnlyNonExpiredAndApproved { get; set; }
    }
}
