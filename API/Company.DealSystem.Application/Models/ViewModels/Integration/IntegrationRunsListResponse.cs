using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Users;
using Company.DealSystem.Application.Models.Dtos.Integration;

namespace Company.DealSystem.Application.Models.ViewModels.Users
{
    public class IntegrationRunsListResponse : ListResponse
    {
        public List<IntegrationRunListDto> Runs { get; set; }
    }
}
