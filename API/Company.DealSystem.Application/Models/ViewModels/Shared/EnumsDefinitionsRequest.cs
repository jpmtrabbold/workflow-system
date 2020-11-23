﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Application.Models.ViewModels.Shared
{
    public class EnumsDefinitionsRequest
    {
        public DealCategoryEnum DealCategoryEnum { get; set; }
        public DealStatusEnum DealStatusEnum { get; set; }

    }
}
