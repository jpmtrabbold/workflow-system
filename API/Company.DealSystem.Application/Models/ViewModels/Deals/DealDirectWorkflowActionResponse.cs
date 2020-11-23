using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Deals
{
    public class DealDirectWorkflowActionResponse
    {
        public bool Success { get; set; } = true;
        public List<string> Messages { get; private set; } = new List<string>();
        public bool ReasonIsRequired { get; set; }
        public string DealNumber { get; set; }

        public DealDirectWorkflowActionResponse Error(string message)
        {
            Success = false;
            Messages.Add(message);
            return this;
        }

        public DealDirectWorkflowActionResponse SetReasonAsRequired()
        {
            Success = false;
            ReasonIsRequired = true;
            return this;
        }
    }
}
