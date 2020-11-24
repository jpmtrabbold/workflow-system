using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Application.Models.Dtos.Audit
{
    public class AuditEntryListDto
    {
        public int Id { get; set; }

        public DateTimeOffset DateTime { get; set; }
        public string UserName { get; set; }
        public List<AuditEntryTableListDto> Tables { get; set; } = new List<AuditEntryTableListDto>();
        public AuditEntryTypeEnum Type { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<AuditEntry, AuditEntryListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new AuditEntryListDto()
                {
                    Id = entity.Id,
                    DateTime = entity.DateTime,
                    UserName = entity.User.Name,
                    Type = entity.Type,
                    Tables = entity.Tables
                        .AsQueryable()
                        .Select(AuditEntryTableListDto.ProjectionFromEntity)
                        .ToList(),
                };
            }
        }
    }
}
