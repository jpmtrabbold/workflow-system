using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum AuditEntryTypeEnum
    {
        [Description("Added")]
        Added = 1,
        [Description("Modified")]
        Modified = 2,
    }
}
