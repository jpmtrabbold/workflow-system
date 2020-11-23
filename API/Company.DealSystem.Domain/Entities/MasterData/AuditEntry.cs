using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
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