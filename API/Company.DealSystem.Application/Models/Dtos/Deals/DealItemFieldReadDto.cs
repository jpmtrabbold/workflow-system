using System;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Application.Models.Helpers;
using System.Collections.Generic;
using System.Linq;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Extensions;
using Company.DealSystem.Domain.Interfaces;

namespace Company.DealSystem.Application.Models.Dtos.Deals
{
    public class DealItemFieldReadDto
    {
        public int? Id { get; set; }
        public int? DealItemFieldsetId { get; set; }
        public int DisplayOrder { get; set; }
        public string Field { get; set; }
        public string Name { get; set; }
        public bool Execution { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<DealItemField, DealItemFieldReadDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealItemFieldReadDto()
                {
                    Id = entity.Id,
                    DealItemFieldsetId = entity.DealItemFieldsetId,
                    DisplayOrder = entity.DisplayOrder,
                    Field = entity.Field,
                    Name = entity.Name,
                    Execution = entity.Execution,
                };
            }
        }

    }
}