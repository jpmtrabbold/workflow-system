using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.Dtos.Audit;

namespace Company.DealSystem.Application.Models.ViewModels.Audit
{
    public class AuditEntriesListResponse : ListResponse
    {
        public List<AuditEntryListDto> AuditEntries { get; set; }
    }
}
