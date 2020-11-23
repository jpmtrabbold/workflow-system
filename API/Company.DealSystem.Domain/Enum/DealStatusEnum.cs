using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum DealStatusEnum
    {
        Entered = 1,
        Updated = 2,
        Submitted = 3,
        Validated = 4,
        Approved = 5,
        Rejected = 6,
        Executed = 7,
        CheckedByMidOffice = 8,
        CheckedByBackOfficeCancelledInactive = 9,
        Copy = 10,
        CompletedInactive = 11,
    }
}
