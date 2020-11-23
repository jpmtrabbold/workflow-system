using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.DealTypes;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.DealTypes
{
    public class DealTypesListResponse : ListResponse
    {
        public List<DealTypeListDto> DealTypes { get; set; }
    }
}
