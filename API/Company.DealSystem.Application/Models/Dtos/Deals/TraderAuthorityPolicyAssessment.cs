using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Application.Models.Dtos.Deals
{
    public class TraderAuthorityPolicyAssessment
    {
        public bool PolicyMet { get; set; }
        public List<TraderAuthorityPolicyAssessmentRow> AssessmentRows { get; set; } = new List<TraderAuthorityPolicyAssessmentRow>();
        public TraderAuthorityPolicyAssessmentRow AddAssessmentRow(string desc, bool criteriaMet, string policyValue, string dealValue, string detailedDescription = "")
        {
            var row = new TraderAuthorityPolicyAssessmentRow { Description = desc, CriteriaMet = criteriaMet, PolicyValue = policyValue, DealValue = dealValue, DetailedDescription = detailedDescription };
            AssessmentRows.Add(row);
            return row;
        }
        public TraderAuthorityPolicyAssessmentRow AddAssessmentRow(string desc, bool criteriaMet, decimal? policyValue, decimal? dealValue, string detailedDescription = "", string unitOfMeasure = "", bool currency = false)
        {
            var row = new TraderAuthorityPolicyAssessmentRow { 
                Description = desc, 
                CriteriaMet = criteriaMet, 
                PolicyValue = (currency ? "$ " : "") + (policyValue ?? 0).ToString("N2") + " " + unitOfMeasure, 
                DealValue = (currency ? "$ " : "") + (dealValue ?? 0).ToString("N2") + " " + unitOfMeasure, 
                DetailedDescription = detailedDescription 
            };
            AssessmentRows.Add(row);
            return row;
        }
        public TraderAuthorityPolicyAssessmentRow AddAssessmentRow(string desc, bool criteriaMet, int? policyValue, int? dealValue, string detailedDescription = "")
        {
            var row = new TraderAuthorityPolicyAssessmentRow { Description = desc, CriteriaMet = criteriaMet, PolicyValue = (policyValue ?? 0).ToString(), DealValue = (dealValue ?? 0).ToString(), DetailedDescription = detailedDescription };
            AssessmentRows.Add(row);
            return row;
        }
    }
}
