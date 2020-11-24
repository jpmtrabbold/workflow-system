using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Application.Models.Dtos.Audit
{
    public class AuditEntryFieldListDto
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<AuditEntryField, AuditEntryFieldListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new AuditEntryFieldListDto()
                {
                    FieldName = entity.FieldName,
                    OldValue = entity.OldValue,
                    NewValue = entity.NewValue,
                };
            }
        }
    }
}
