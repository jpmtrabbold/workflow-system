using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.DealTypes;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.DealTypes
{
    public class DealTypesListResponse : ListResponse
    {
        public List<DealTypeListDto> DealTypes { get; set; }
    }
}
