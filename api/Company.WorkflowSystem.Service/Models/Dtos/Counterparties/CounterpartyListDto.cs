using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Service.Models.Dtos.Counterparties
{
    public class CounterpartyListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public decimal ExposureLimit { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        
        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<Counterparty, CounterpartyListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new CounterpartyListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Active = entity.Active,
                    ExposureLimit = entity.ExposureLimit,
                    ExpiryDate = entity.ExpiryDate,
                };
            }
        }
    }
}
