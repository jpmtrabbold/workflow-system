using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Domain.ExtensionMethods;
using Company.WorkflowSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Infrastructure.Context;
using System.Linq;
using Company.WorkflowSystem.Domain.Util;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Application.Models.Dtos.Deals
{
    public class DealItemDto : UpdatableListItemDto, IPersistableDto<DealItem, DealService>
    {
        public int Order { get; set; }
        public Updatable<int?> ProductId { get; set; }
        public string ProductDescription { get; set; }
        public Updatable<PositionEnum?> Position { get; set; }
        public Updatable<DayTypeEnum?> DayType { get; set; }
        public Updatable<DateTimeOffset?> StartDate { get; set; }
        public Updatable<int?> HalfHourTradingPeriodStart { get; set; }
        public Updatable<DateTimeOffset?> EndDate { get; set; }
        public Updatable<int?> HalfHourTradingPeriodEnd { get; set; }
        public Updatable<decimal?> Quantity { get; set; }
        public Updatable<decimal?> MinQuantity { get; set; }
        public Updatable<decimal?> MaxQuantity { get; set; }
        public Updatable<decimal?> Price { get; set; }
        public Updatable<string> Criteria { get; set; }
        public int? OriginalItemId { get; set; }
        public List<DealExecutedItemDto> ExecutedItems { get; set; }
        public bool Executed { get; private set; }
        public DealItemSourceDataDto SourceData { get; set; } = new DealItemSourceDataDto();

        internal static Expression<Func<DealItem, DealItemDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealItemDto()
                {
                    Id = entity.Id,
                    Order = entity.Order,
                    ProductId = Updatable.Create(entity.ProductId),
                    ProductDescription = (entity.Product != null ? entity.Product.Name : null),
                    Position = Updatable.Create(entity.Position),
                    DayType = Updatable.Create(entity.DayType),
                    StartDate = Updatable.Create(entity.StartDate),
                    HalfHourTradingPeriodStart = Updatable.Create(entity.HalfHourTradingPeriodStart),
                    EndDate = Updatable.Create(entity.EndDate),
                    HalfHourTradingPeriodEnd = Updatable.Create(entity.HalfHourTradingPeriodEnd),
                    Quantity = Updatable.Create(entity.Quantity),
                    MinQuantity = Updatable.Create(entity.MinQuantity),
                    MaxQuantity = Updatable.Create(entity.MaxQuantity),
                    Price = Updatable.Create(entity.Price),
                    Criteria = Updatable.Create(entity.Criteria),
                    OriginalItemId = entity.OriginalItemId,
                    ExecutedItems = (!entity.OriginalItemId.HasValue ? entity.Deal.Items
                        .AsQueryable()
                        .Where(t => t.OriginalItemId == entity.Id)
                        .Select(DealExecutedItemDto.ProjectionFromEntity)
                        .OrderBy(t => t.Order)
                        .ToList() : null),
                    Executed = entity.Deal.Executed,
                    SourceData = new DealItemSourceDataDto
                    {
                        CreationDate = entity.SourceData.CreationDate,
                        Type = entity.SourceData.Type,
                        SourceId = entity.SourceData.SourceId,
                    },
                };
            }
        }

        /// <summary>
        /// creates or updates entity based on current dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DealItem ToEntity(DealItem entity, DealService service)
        {
            if (entity == null)
            {
                entity = new DealItem
                {
                    Order = Order,
                    OriginalItemId = OriginalItemId,
                    SourceData = new DealItemSourceData
                    {
                        SourceId = SourceData.SourceId,
                        CreationDate = SourceData.CreationDate,
                        Type = SourceData.Type,
                    },
                };
            }

            if (Updatable.IsUpdated(ProductId))
                entity.ProductId = ProductId.Value;

            if (Updatable.IsUpdated(Position))
                entity.Position = Position.Value;

            if (Updatable.IsUpdated(DayType))
                entity.DayType = DayType.Value;

            if (Updatable.IsUpdated(StartDate))
            {
                if (StartDate.Value.HasValue)
                    entity.StartDate = StartDate.Value.Value;
                else
                    entity.StartDate = null;
            }

            if (Updatable.IsUpdated(HalfHourTradingPeriodStart))
                entity.HalfHourTradingPeriodStart = HalfHourTradingPeriodStart.Value;

            if (Updatable.IsUpdated(EndDate))
            {
                if (EndDate.Value.HasValue)
                    entity.EndDate = EndDate.Value.Value;
                else
                    entity.EndDate = null;
            }
            

            if (Updatable.IsUpdated(HalfHourTradingPeriodEnd))
                entity.HalfHourTradingPeriodEnd = HalfHourTradingPeriodEnd.Value;

            if (Updatable.IsUpdated(Quantity))
                entity.Quantity = Quantity.Value;

            if (Updatable.IsUpdated(MinQuantity))
                entity.MinQuantity = MinQuantity.Value;

            if (Updatable.IsUpdated(MaxQuantity))
                entity.MaxQuantity = MaxQuantity.Value;

            if (Updatable.IsUpdated(Price))
                entity.Price = Price.Value;

            if (Updatable.IsUpdated(Criteria))
                entity.Criteria = Criteria.Value;

            Updatable.ToEntityCollection(ExecutedItems, entity?.Deal?.Items, service);

            return entity;
        }

        static List<StringLookupRequest> _itemFieldLookups;

        // these field need to be static as they refer to DealItemDto properties
        internal static List<StringLookupRequest> ItemFieldLookups
        {
            get
            {
                if (_itemFieldLookups == null)
                    _itemFieldLookups = ItemFields.Select(tf => new StringLookupRequest { Name = tf.fieldTitle, Id = tf.fieldName, Description = tf.fieldDescription }).ToList();

                return _itemFieldLookups;
            }
        }

        internal class DealItemFieldDefinition
        {
            public string fieldTitle { get; set; }
            public string fieldName { get; set; }
            public string fieldDescription { get; set; }
            public Func<DealItemDto, string> stringDTOValue { get; set; }
        }

        static List<DealItemFieldDefinition> _itemFields;

        internal static List<DealItemFieldDefinition> ItemFields
        {
            get
            {
                if (_itemFields == null)
                {
                    _itemFields = new List<DealItemFieldDefinition>
                    {
                        new DealItemFieldDefinition { fieldTitle = "Product", fieldName = "ProductId", fieldDescription = "Field name: NodeId",
                            stringDTOValue = (DealItemDto dt) => dt.ProductDescription },
                        new DealItemFieldDefinition { fieldTitle = "Position", fieldName = "Position", fieldDescription = "Field name: Position",
                            stringDTOValue = (DealItemDto dt) => dt.Position.Value.HasValue ? dt.Position.Value.Value.GetDescription() : "" },
                        new DealItemFieldDefinition { fieldTitle = "Day Type", fieldName = "DayType", fieldDescription = "Field name: DayType",
                            stringDTOValue = (DealItemDto dt) => dt.DayType.Value.HasValue ? dt.DayType.Value.Value.GetDescription() : ""},
                        new DealItemFieldDefinition { fieldTitle = "Start Date", fieldName = "StartDate", fieldDescription = "Field name: StartDate",
                            stringDTOValue = (DealItemDto dt) => dt.StartDate.Value.HasValue ? dt.StartDate.Value.Value.ToDateString() : ""},
                        new DealItemFieldDefinition { fieldTitle = "End Date", fieldName = "EndDate", fieldDescription = "Field name: EndDate",
                            stringDTOValue = (DealItemDto dt) => dt.EndDate.Value.HasValue ? dt.EndDate.Value.Value.ToDateString() : "" },
                        new DealItemFieldDefinition { fieldTitle = "TP Start", fieldName = "HalfHourTradingPeriodStart", fieldDescription = "Field name: HalfHourTradingPeriodStart",
                            stringDTOValue = (DealItemDto dt) => dt.HalfHourTradingPeriodStart.Value.HasValue ? dt.HalfHourTradingPeriodStart.Value.Value.ToString() : ""},
                        new DealItemFieldDefinition { fieldTitle = "TP End", fieldName = "HalfHourTradingPeriodEnd", fieldDescription = "Field name: HalfHourTradingPeriodEnd",
                            stringDTOValue = (DealItemDto dt) => dt.HalfHourTradingPeriodEnd.Value.HasValue ? dt.HalfHourTradingPeriodEnd.Value.Value.ToString() : "" },
                        new DealItemFieldDefinition { fieldTitle = "Qty", fieldName = "Quantity", fieldDescription = "Field name: Quantity",
                            stringDTOValue = (DealItemDto dt) => dt.Quantity.Value.HasValue ? dt.Quantity.Value.Value.ToString("N3") : "" },
                        new DealItemFieldDefinition { fieldTitle = "Price", fieldName = "Price", fieldDescription = "Field name: Price",
                            stringDTOValue = (DealItemDto dt) => dt.Price.Value.HasValue ? dt.Price.Value.Value.ToString("N") : "" },
                        new DealItemFieldDefinition { fieldTitle = "Criteria", fieldName = "Criteria", fieldDescription = "Field name: Criteria",
                            stringDTOValue = (DealItemDto dt) => dt.Criteria.Value },
                        new DealItemFieldDefinition { fieldTitle = "Min", fieldName = "MinQuantity", fieldDescription = "Field name: MinQuantity",
                            stringDTOValue = (DealItemDto dt) => dt.MinQuantity.Value.HasValue ? dt.MinQuantity.Value.Value.ToString("N3") : "" },
                        new DealItemFieldDefinition { fieldTitle = "Max", fieldName = "MaxQuantity", fieldDescription = "Field name: MaxQuantity",
                            stringDTOValue = (DealItemDto dt) => dt.MaxQuantity.Value.HasValue ? dt.MaxQuantity.Value.Value.ToString("N3") : "" },
                    };
                }
                return _itemFields;
            }
        }
    }
}