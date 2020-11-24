using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Users;
using Company.WorkflowSystem.Application.Models.Dtos.Integration;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Users
{
    public class IntegrationRunsListResponse : ListResponse
    {
        public List<IntegrationRunListDto> Runs { get; set; }
    }
}
