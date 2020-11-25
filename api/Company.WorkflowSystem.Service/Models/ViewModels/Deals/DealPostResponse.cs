using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Deals
{
    public class DealPostResponse
    {
        public int DealId { get; set; }
        public string DealNumber { get; set; }
        public List<string> WarningMessages { get; private set; } = new List<string>();
        
        public void AddWarningMessage(string message) => WarningMessages.Add(message);
        
    }
}
