using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Users;
using Company.WorkflowSystem.Service.Models.Dtos.Integration;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Users
{
    public class IntegrationRunsListResponse : ListResponse
    {
        public List<IntegrationRunListDto> Runs { get; set; }
    }
}
