using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Deals
{
    public class DealPostResponse
    {
        public int DealId { get; set; }
        public string DealNumber { get; set; }
        public List<string> WarningMessages { get; private set; } = new List<string>();
        
        public void AddWarningMessage(string message) => WarningMessages.Add(message);
        
    }
}
