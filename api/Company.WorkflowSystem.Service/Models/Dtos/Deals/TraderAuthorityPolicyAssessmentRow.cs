using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class TraderAuthorityPolicyAssessmentRow
    {
        public string Description { get; set; }
        public string DetailedDescription { get; set; }
        public string PolicyValue { get; set; }
        public string DealValue { get; set; }
        public bool CriteriaMet { get; set; }
        public bool IsTermCriteria { get; set; }
    }
}
