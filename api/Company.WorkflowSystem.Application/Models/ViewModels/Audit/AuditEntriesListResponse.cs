using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;
using Company.WorkflowSystem.Application.Models.Dtos.Audit;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Audit
{
    public class AuditEntriesListResponse : ListResponse
    {
        public List<AuditEntryListDto> AuditEntries { get; set; }
    }
}
