using InversionRepo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Interfaces;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Shared
{
    public class ListRequest : IListRequest
    {
        public int? Id { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string SortField { get; set; }
        public bool SortOrderAscending { get; set; }
        public string SearchString { get; set; }
        public bool OnlyActive { get; set; }
    }
}
