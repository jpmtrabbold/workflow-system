using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Domain.Enum;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Users
{
    public class IntegrationRunsListRequest : ListRequest
    {
        public IntegrationTypeEnum? IntegrationType { get; set; }
        public List<IntegrationRunStatusEnum> Statuses { get; set; }
        public int? IntegrationRunId { get; set; }
    }
}
