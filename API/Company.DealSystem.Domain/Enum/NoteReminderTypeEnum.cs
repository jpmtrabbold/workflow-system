using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum NoteReminderTypeEnum
    {
        [Description("Me")]
        Me = 1,
        [Description("My Role")]
        MyRole = 2,
        [Description("Specific E-mail(s)")]
        Emails = 3,
    }
}
