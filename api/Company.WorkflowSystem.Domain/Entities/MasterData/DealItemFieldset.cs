using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealItemFieldset : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<DealItemField> ItemFields { get; private set; } = new List<DealItemField>();
    }
}
