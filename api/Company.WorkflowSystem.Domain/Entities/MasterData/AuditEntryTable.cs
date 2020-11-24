using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class AuditEntryTable : BaseEntity
    {
        public int AuditEntryId { get; set; }
        public AuditEntry AuditEntry { get; set; }
        
        [Required]
        [MaxLength(128)]
        public string TableName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Action { get; set; }
        public string KeyValues { get; set; }
        public ICollection<AuditEntryField> Fields { get; set; } = new List<AuditEntryField>();
    }
}