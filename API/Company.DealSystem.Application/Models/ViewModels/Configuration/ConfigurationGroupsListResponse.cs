using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Users;
using Company.DealSystem.Application.Models.Dtos.Configuration;

namespace Company.DealSystem.Application.Models.ViewModels.Configuration
{
    public class ConfigurationGroupsListResponse : ListResponse
    {
        public List<ConfigurationGroupsListDto> ConfigurationGroups { get; set; }
    }
}
