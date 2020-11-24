using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class AuditEntry : BaseEntity
    {
        public DateTimeOffset DateTime { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int FunctionalityId { get; set; }
        public Functionality Functionality { get; set; }
        public int EntityId { get; set; }
        public AuditEntryTypeEnum Type { get; set; }

        public List<AuditEntryTable> Tables { get; set; } = new List<AuditEntryTable>();
    }
}