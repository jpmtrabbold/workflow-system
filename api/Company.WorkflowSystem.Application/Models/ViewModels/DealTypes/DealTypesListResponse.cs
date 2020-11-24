﻿using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.DealTypes;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.DealTypes
{
    public class DealTypesListResponse : ListResponse
    {
        public List<DealTypeListDto> DealTypes { get; set; }
    }
}