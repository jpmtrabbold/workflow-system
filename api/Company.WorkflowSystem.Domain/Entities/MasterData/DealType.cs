
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public partial class DealType : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        /// <summary>
        /// default position for this deal type
        /// </summary>
        public PositionEnum Position { get; set; } = PositionEnum.Buy;
        /// <summary>
        /// determines whether the position in the Position field should be mandatory 
        /// (i.e. in a deal with deal type with a Buy position and ForcePosition = true, you can only create Buy deal items)
        /// </summary>
        public bool ForcePosition { get; set; } = false;
        /// <summary>
        /// default unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; }
        /// <summary>
        /// whether this deal type support loss factors
        /// </summary>
        public bool HasLossFactors { get; set; } = false;
        /// <summary>
        /// whether this deal type support Expiry Date
        /// </summary>
        public bool HasExpiryDate { get; set; } = false;
        /// <summary>
        /// whether this deal type supports delegated authorities
        /// </summary>
        public bool HasDelegatedAuthority { get; set; } = false;
        /// <summary>
        /// whether this deal type supports changing the product on the execution
        /// </summary>
        public bool CanChangeProductOnExecution { get; set; } = false;
        /// <summary>
        /// determines which fields are used for deal items when this deal type is selected for a deal
        /// </summary>
        public int? DealItemFieldsetId { get; set; }
        /// <summary>
        /// determines which fields are used for deal items when this deal type is selected for a deal
        /// </summary>
        public DealItemFieldset DealItemFieldset { get; set; }
        /// <summary>
        /// determines which workflow set id is used when this deal type is selected for a deal
        /// </summary>
        public int? WorkflowSetId { get; set; }
        /// <summary>
        /// determines which workflow set is used when this deal type is selected for a deal
        /// </summary>
        public WorkflowSet WorkflowSet { get; set; }
        /// <summary>
        /// Deal categories that apply to this deal type
        /// </summary>
        public ICollection<DealTypeInDealCategory> DealCategoriesInDealType { get; private set; } = new List<DealTypeInDealCategory>();
        /// <summary>
        /// Workflow tasks that apply to this deal type
        /// </summary>
        public ICollection<WorkflowTaskInDealType> WorkflowTasksInDealType { get; private set; } = new List<WorkflowTaskInDealType>();
        
        /// <summary>
        /// Policy by which the approver for this type of deal is determined
        /// </summary>
        public int? TraderAuthorityPolicyId { get; set; }
        /// <summary>
        /// Policy by which the approver for this type of deal is determined
        /// </summary>
        public TraderAuthorityPolicy TraderAuthorityPolicy { get; set; }
        /// <summary>
        /// Determines the type of
        /// </summary>
        public DealItemExecutionImportTemplateTypeEnum? ItemExecutionImportTemplateType { get; set; }

    }
}