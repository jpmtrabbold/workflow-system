using System;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Domain.ExtensionMethods;
using Company.DealSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using System.Linq;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Deals
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