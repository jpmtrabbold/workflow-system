using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Audit
{
    public class AuditEntriesListRequest : ListRequest
    {
        public FunctionalityEnum FunctionalityEnum { get; set; }
        public int EntityId { get; set; }
    }
}