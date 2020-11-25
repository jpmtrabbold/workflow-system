using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Service.Models.Dtos.DealItemFieldsets
{
    public class DealItemFieldsetListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<DealItemFieldset, DealItemFieldsetListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealItemFieldsetListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                };
            }
        }
    }
}