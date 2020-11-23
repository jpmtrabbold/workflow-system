using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Deals
{
    public class DealPostResponse
    {
        public int DealId { get; set; }
        public string DealNumber { get; set; }
        public List<string> WarningMessages { get; private set; } = new List<string>();
        
        public void AddWarningMessage(string message) => WarningMessages.Add(message);
        
    }
}
