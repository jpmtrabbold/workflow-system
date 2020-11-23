using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Application.Models.ViewModels.Audit
{
    public class AuditEntriesListRequest : ListRequest
    {
        public FunctionalityEnum FunctionalityEnum { get; set; }
        public int EntityId { get; set; }
    }
}