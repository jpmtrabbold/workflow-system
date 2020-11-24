using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Domain.Enum
{
    public enum IntegrationRunStatusEnum
    {
        Running = 1,
        Success = 2,
        Warning = 3,
        Error = 4,
        ErroredButReprocessed = 5,
        ErroredButNotPending = 6,
    }
}
