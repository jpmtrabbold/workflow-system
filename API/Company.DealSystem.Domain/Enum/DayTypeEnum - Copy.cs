using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum DayTypeEnum
    {
        [Description("All Days")]
        AllDays = 1,
        [Description("Week Days")]
        WeekDays = 2,
        [Description("Weekend Days")]
        WeekendDays = 3
    }
}
