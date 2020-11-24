using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Shared
{
    public class EnumsDefinitionsRequest
    {
        public DealCategoryEnum DealCategoryEnum { get; set; }
        public DealStatusEnum DealStatusEnum { get; set; }

    }
}
