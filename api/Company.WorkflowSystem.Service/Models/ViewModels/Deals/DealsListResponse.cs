using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Deals
{
    public class DealsListResponse : ListResponse
    {
        public List<DealListDto> Deals { get; set; }
    }
}
