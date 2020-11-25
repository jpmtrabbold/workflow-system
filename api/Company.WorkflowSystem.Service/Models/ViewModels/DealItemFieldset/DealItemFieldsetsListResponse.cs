using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.DealItemFieldsets;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.ItemFieldsets
{
    public class DealItemFieldsetsListResponse : ListResponse
    {
        public List<DealItemFieldsetListDto> ItemFieldsets { get; set; }
    }
}
