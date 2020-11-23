using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.DealItemFieldsets;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.ItemFieldsets
{
    public class DealItemFieldsetsListResponse : ListResponse
    {
        public List<DealItemFieldsetListDto> ItemFieldsets { get; set; }
    }
}
