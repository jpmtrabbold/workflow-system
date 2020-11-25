using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Users;
using Company.WorkflowSystem.Service.Models.Dtos.Configuration;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Configuration
{
    public class ConfigurationGroupsListResponse : ListResponse
    {
        public List<ConfigurationGroupsListDto> ConfigurationGroups { get; set; }
    }
}
