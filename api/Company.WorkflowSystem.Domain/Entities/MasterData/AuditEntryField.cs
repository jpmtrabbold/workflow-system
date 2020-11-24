using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class AuditEntryField : BaseEntity
    {
        public int AuditEntryTableId { get; set; }
        public AuditEntryTable AuditEntryTable { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}