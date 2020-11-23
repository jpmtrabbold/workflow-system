using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum DealItemSourceTypeEnum
    {
        [Description("EMS")]
        Ems = 1,
        [Description("FTR")]
        Ftr = 2,
    }
}
