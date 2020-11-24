using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.WorkflowSystem.Domain.Models.Enum
{
    public enum WorkflowTaskAnswerTypeEnum
    {
        [Description("'Yes' answer")]
        Yes = 1,
        [Description("'No' answer")]
        No = 2,
        [Description("Other")]
        Other = 3,
    }
}
