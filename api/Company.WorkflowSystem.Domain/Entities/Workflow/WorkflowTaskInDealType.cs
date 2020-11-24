using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowTaskInDealType
    {
        [Column(Order = 1)]
        public int WorkflowTaskId { get; set; }
        public WorkflowTask WorkflowTask { get; set; }
        [Column(Order = 2)]
        public int DealTypeId { get; set; }
        public DealType DealType { get; set; }
    }
}