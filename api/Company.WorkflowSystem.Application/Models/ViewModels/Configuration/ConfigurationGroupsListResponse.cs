using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Users;
using Company.WorkflowSystem.Application.Models.Dtos.Configuration;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Configuration
{
    public class ConfigurationGroupsListResponse : ListResponse
    {
        public List<ConfigurationGroupsListDto> ConfigurationGroups { get; set; }
    }
}
