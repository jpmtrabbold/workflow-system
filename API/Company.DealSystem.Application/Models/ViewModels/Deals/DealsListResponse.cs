using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Deals;

namespace Company.DealSystem.Application.Models.ViewModels.Deals
{
    public class DealsListResponse : ListResponse
    {
        public List<DealListDto> Deals { get; set; }
    }
}
