using System.Collections.Generic;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Application.Models.ViewModels.Users
{
    public class IntegrationRunsListRequest : ListRequest
    {
        public IntegrationTypeEnum? IntegrationType { get; set; }
        public List<IntegrationRunStatusEnum> Statuses { get; set; }
        public int? IntegrationRunId { get; set; }
    }
}
