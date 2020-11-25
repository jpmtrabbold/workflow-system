using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Service.Models.Dtos.Audit;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Audit
{
    public class AuditEntriesListResponse : ListResponse
    {
        public List<AuditEntryListDto> AuditEntries { get; set; }
    }
}
