using System;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Entities.Integrations;
using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Application.Models.Dtos.Integration
{
    public class IntegrationRunEntryDto
    {
        public int Id { get; set; }
        public IntegrationRunEntryTypeEnum Type { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string Payload { get; set; }
        public int? AffectedId { get; set; }
        public FunctionalityEnum? FunctionalityOfAffectedId { get; set; }
        public DateTimeOffset? DateTime { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<IntegrationRunEntry, IntegrationRunEntryDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new IntegrationRunEntryDto()
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    Message = entity.Message,
                    Details = entity.Details,
                    Payload = entity.Payload,
                    AffectedId = entity.AffectedId,
                    FunctionalityOfAffectedId = entity.FunctionalityOfAffectedId,
                    DateTime = entity.DateTime,
                };
            }
        }
    }
}