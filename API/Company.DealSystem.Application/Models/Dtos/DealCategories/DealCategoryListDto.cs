using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;

namespace Company.DealSystem.Application.Models.Dtos.DealCategories
{
    public class DealCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<DealCategory, DealCategoryListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealCategoryListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    UnitOfMeasure = entity.UnitOfMeasure,
                    Active = entity.Active
                };
            }
        }
    }
}
