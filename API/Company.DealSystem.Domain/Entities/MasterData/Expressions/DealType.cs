using LinqExpander;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public partial class DealType : DeactivatableBaseEntity
    {
        [ReplaceWithExpression(MethodName = nameof(PositionNameProjection))]
        static public string PositionName(DealType entity) => PositionNameProjection().Compile().Invoke(entity);
        static Expression<Func<DealType, string>> PositionNameProjection() =>
            (entity) => entity.Position == PositionEnum.Buy ? "Buy" : "Sell";

        [ReplaceWithExpression(MethodName = nameof(HasLossFactorsDescriptionProjection))]
        static public string HasLossFactorsDescription(DealType entity) => HasLossFactorsDescriptionProjection().Compile().Invoke(entity);
        static Expression<Func<DealType, string>> HasLossFactorsDescriptionProjection() =>
            (entity) => entity.HasLossFactors ? "Yes" : "No";

        [ReplaceWithExpression(MethodName = nameof(DealItemFieldsetNameProjection))]
        static public string ItemFieldsetName(DealType entity) => DealItemFieldsetNameProjection().Compile().Invoke(entity);
        static Expression<Func<DealType, string>> DealItemFieldsetNameProjection() =>
            (entity) => (entity.DealItemFieldsetId.HasValue ? entity.DealItemFieldset.Name : "None");

        [ReplaceWithExpression(MethodName = nameof(WorkflowSetNameProjection))]
        static public string WorkflowSetName(DealType entity) => WorkflowSetNameProjection().Compile().Invoke(entity);
        static Expression<Func<DealType, string>> WorkflowSetNameProjection() =>
            (entity) => (entity.WorkflowSetId.HasValue ? entity.WorkflowSet.Name : "None");

        [ReplaceWithExpression(MethodName = nameof(ActiveDescriptionProjection))]
        static public string ActiveDescription(DealType entity) => ActiveDescriptionProjection().Compile().Invoke(entity);
        static Expression<Func<DealType, string>> ActiveDescriptionProjection() =>
            (entity) => entity.Active ? "Yes" : "No";

    }
}