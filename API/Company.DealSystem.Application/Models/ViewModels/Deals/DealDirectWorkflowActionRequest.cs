using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Deals
{
    public class DealDirectWorkflowActionRequest
    {
        public int DealId { get; set; }
        public int UserId { get; set; }
        public string Key { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string Reason { get; set; }
    }
}
