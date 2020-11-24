using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Application.Models.Dtos.Audit
{
    public class AuditEntryTableListDto
    {
        public string TableName { get; set; }
        public string Action { get; set; }
        public string KeyValues { get; set; }
        public List<AuditEntryFieldListDto> Fields { get; set; } = new List<AuditEntryFieldListDto>();

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<AuditEntryTable, AuditEntryTableListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new AuditEntryTableListDto()
                {
                    TableName = entity.TableName,
                    Action = entity.Action,
                    KeyValues = entity.KeyValues,
                    Fields = entity.Fields
                        .AsQueryable()
                        .Select(AuditEntryFieldListDto.ProjectionFromEntity)
                        .ToList(),
                };
            }
        }
    }
}
