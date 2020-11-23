using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum PositionEnum
    {
        [Description("Sell")]
        Sell = 0,
        [Description("Buy")]
        Buy = 1,
    }
}
