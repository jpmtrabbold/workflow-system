using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class CounterpartyInDealCategory
    {
        [Column(Order = 1)]
        public int DealCategoryId { get; set; }
        public DealCategory DealCategory { get; set; }
        [Column(Order = 2)]
        public int CounterpartyId { get; set; }
        public Counterparty Counterparty { get; set; }
    }
}