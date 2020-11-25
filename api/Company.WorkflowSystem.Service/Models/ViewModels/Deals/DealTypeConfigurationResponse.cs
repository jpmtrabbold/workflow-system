using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Domain.Models.Enum;
using System.Linq.Expressions;
using System;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Service.Utils;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Deals
{
    public class DealTypeConfigurationResponse
    {
        public int? DealItemFieldsetId { get; set; }
        public List<DealItemFieldReadDto> DealItemFields { get; set; }

        public PositionEnum Position { get; set; } = PositionEnum.Buy;
        public bool ForcePosition { get; set; } = false;
        public string UnitOfMeasure { get; set; }
        public bool HasLossFactors { get; set; } = false;
        public bool HasExpiryDate { get; set; } = false;
        public bool HasDelegatedAuthority { get; set; } = false;
        public int? WorkflowSetId { get; set; }
        public WorkflowStatusReadDto CurrentWorkflowStatusConfig { get; set; }
        public DealItemExecutionImportTemplateTypeEnum? ItemExecutionImportTemplateType { get; set; }

        public List<LookupRequest> NoteReminderTypeLookups { get; set; } = EnumUtils.GetEnumAsLookups<NoteReminderTypeEnum>();

        internal static Expression<Func<DealType, DealTypeConfigurationResponse>> ProjectionFromEntity(int? itemFieldsetId = null, int? workflowSetId = null)
        {
            return entity => new DealTypeConfigurationResponse()
            {
                DealItemFieldsetId = (itemFieldsetId.HasValue && itemFieldsetId > 0 ? itemFieldsetId : entity.DealItemFieldsetId),
                WorkflowSetId = (workflowSetId.HasValue && workflowSetId > 0 ? workflowSetId : entity.WorkflowSetId),
                Position = entity.Position,
                ForcePosition = entity.ForcePosition,
                UnitOfMeasure = entity.UnitOfMeasure,
                HasLossFactors = entity.HasLossFactors,
                HasExpiryDate = entity.HasExpiryDate,
                HasDelegatedAuthority = entity.HasDelegatedAuthority,
                ItemExecutionImportTemplateType = entity.ItemExecutionImportTemplateType,
            };
        }
    }


}
