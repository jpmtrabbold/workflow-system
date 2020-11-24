using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.DealItemFieldsets;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.ItemFieldsets
{
    public class DealItemFieldsetsListResponse : ListResponse
    {
        public List<DealItemFieldsetListDto> ItemFieldsets { get; set; }
    }
}
