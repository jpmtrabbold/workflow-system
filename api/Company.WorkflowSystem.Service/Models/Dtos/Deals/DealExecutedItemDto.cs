using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Domain.ExtensionMethods;
using Company.WorkflowSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using System.Linq;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealExecutedItemDto : DealItemDto, IPersistableDto<DealItem, DealService>
    {
        new internal static Expression<Func<DealItem, DealExecutedItemDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealExecutedItemDto()
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
                    SourceData = new DealItemSourceDataDto
                    {
                        CreationDate = entity.SourceData.CreationDate,
                        Type = entity.SourceData.Type,
                        SourceId = entity.SourceData.SourceId,
                    },
                };
            }
        }
    }
}