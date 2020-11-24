using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Application.Models.Dtos.DealTypes
{
    public class DealTypeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PositionName { get; set; }
        public string UnitOfMeasure { get; set; }
        public string HasLossFactors { get; set; }
        public string HasExpiryDate { get; set; }
        public string DealItemFieldsetName { get; set; }
        public string WorkflowSetName { get; set; }

        public string ActiveDescription { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<DealType, DealTypeListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealTypeListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PositionName = DealType.PositionName(entity),
                    UnitOfMeasure = entity.UnitOfMeasure,
                    HasLossFactors = DealType.HasLossFactorsDescription(entity),
                    HasExpiryDate = (entity.HasExpiryDate ? "Yes" : "No"),
                    DealItemFieldsetName = DealType.ItemFieldsetName(entity),
                    WorkflowSetName = DealType.WorkflowSetName(entity),
                    ActiveDescription = DealType.ActiveDescription(entity),
                };
            }
        }
    }
}
