using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Models.Enum
{
    public enum SubFunctionalityEnum
    {
        Create = 1,
        View = 2,
        Edit = 3,
        Delete = 4,
        SelectDealLossFactors = 5,
        EditDealLossFactorsValue = 6,
        PDFExport = 7,
        AuditLogsView = 8,
        ExecuteDeal = 9,
        RunIntegration = 10,
        ReprocessIntegration = 11,
    }
}
