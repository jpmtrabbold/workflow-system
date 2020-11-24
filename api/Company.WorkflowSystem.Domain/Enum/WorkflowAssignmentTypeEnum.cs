using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company.WorkflowSystem.Domain.Models.Enum
{
    /*
    submit - anyone in MO - Role
    validate - goes for approval to any role higher than current - RoleSelectionEqualHigher
    approve - goes for execution to the initial trader - UserSelection
    execute - goes for MO checking by anyone in MO - PredefinedRole
    check MO - goes for BO checking by anyone in BO - PredefinedRole
    check BO - gets finalised, assigns to no one - null
    */

    public enum WorkflowAssignmentTypeEnum
    {
        [Description("Deal Trader")]
        DealTrader = 1,
        //[Description("User Selection")] // not necessary at the moment
        //UserSelection = 2,
        [Description("Predefined Approval Level")]
        PredefinedApprovalLevel = 3,
        [Description("Approval Level Selection (equal or higher than current)")]
        ApprovalLevelSelectionEqualHigher = 4,
    }
}
