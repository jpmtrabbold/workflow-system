using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Entities.Integrations;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Application.Models.Dtos.Integration
{
    public class IntegrationRunListDto
    {
        public int Id { get; set; }
        public string StartedBy { get; set; }
        public DateTimeOffset Started { get; set; }
        public DateTimeOffset Ended { get; set; }
        public IntegrationRunStatusEnum Status { get; set; }
        public string Payload { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<IntegrationRun, IntegrationRunListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new IntegrationRunListDto()
                {
                    Id = entity.Id,
                    Started = entity.Started,
                    Ended = entity.Ended,
                    Status = entity.Status,
                    Payload = entity.Payload,
                    StartedBy = entity.UserId.HasValue ? entity.User.Name : "System",
                };
            }
        }
    }
}
