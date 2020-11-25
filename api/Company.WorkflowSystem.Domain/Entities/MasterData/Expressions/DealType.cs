
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public partial class DealType : DeactivatableBaseEntity
    {
        public static Expression<Func<DealType, string>> PositionName
        {
            get => (entity) => entity.Position == PositionEnum.Buy ? "Buy" : "Sell";
        }


        public static Expression<Func<DealType, string>> HasLossFactorsDescription
        {
            get => (entity) => entity.HasLossFactors ? "Yes" : "No";
        }

        public static Expression<Func<DealType, string>> DealItemFieldsetName
        {
            get => (entity) => (entity.DealItemFieldsetId.HasValue ? entity.DealItemFieldset.Name : "None");
        }

        public static Expression<Func<DealType, string>> WorkflowSetName
        {
            get => (entity) => (entity.WorkflowSetId.HasValue ? entity.WorkflowSet.Name : "None");
        }

        public static Expression<Func<DealType, string>> ActiveDescription
        {
            get => (entity) => entity.Active ? "Yes" : "No";
        }

    }
}