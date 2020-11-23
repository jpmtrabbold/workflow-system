using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.DealSystem.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.DealSystem.Domain.Entities
{
    public class DealItemField : BaseEntity
    {
        public int DealItemFieldsetId { get; set; }
        public DealItemFieldset DealItemFieldset { get; set; }
        public int DisplayOrder { get; set; }
        public string Field { get; set; }
        public string Name { get; set; }
        public bool Execution { get; set; } = false;

    }
}
