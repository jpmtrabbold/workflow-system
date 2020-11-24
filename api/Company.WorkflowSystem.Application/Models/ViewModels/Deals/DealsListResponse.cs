using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Deals
{
    public class DealsListResponse : ListResponse
    {
        public List<DealListDto> Deals { get; set; }
    }
}
