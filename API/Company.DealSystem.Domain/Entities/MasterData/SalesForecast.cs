using System;
using System.Collections.Generic;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class SalesForecast : BaseEntity
    {
        public DateTimeOffset MonthYear { get; set; }
        public decimal Volume { get; set; }        
    }
}