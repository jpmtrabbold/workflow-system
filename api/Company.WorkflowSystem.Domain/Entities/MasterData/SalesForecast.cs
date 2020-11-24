using System;
using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class SalesForecast : BaseEntity
    {
        public DateTimeOffset MonthYear { get; set; }
        public decimal Volume { get; set; }        
    }
}