using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Counterparties
{
    public class CounterpartiesListRequest : ListRequest
    {
        public int? DealId { get; set; }
        public bool? OnlyNonExpiredAndApproved { get; set; }
    }
}
