using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;

namespace Company.DealSystem.Application.Models.ViewModels.Shared
{
    public class LookupRequestHeader
    {
        public bool? CurrentUser { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? TotalCount { get; set; }
        public int? CurrentPage { get; set; }
        public List<LookupRequest> Results { get; set; }
    }
}
