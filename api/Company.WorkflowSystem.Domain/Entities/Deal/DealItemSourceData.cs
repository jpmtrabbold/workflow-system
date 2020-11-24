using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealItemSourceData : BaseEntity
    {
        public long? SourceId { get; set; }
        public DealItemSourceTypeEnum? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}
