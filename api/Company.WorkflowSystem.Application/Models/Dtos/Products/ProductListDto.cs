using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Application.Models.Dtos.Products
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DealCategory { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<Product, ProductListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new ProductListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    DealCategory = entity.DealCategory.Name,
                    Active = entity.Active
                };
            }
        }
    }
}
