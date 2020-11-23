using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.SalesForecasts
{
    public class SalesForecastDto : UpdatableListItemDto, IPersistableDto<SalesForecast, BaseService>
    {
        public Updatable<DateTimeOffset> MonthYear { get; set; }
        public Updatable<decimal> Volume { get; set; }
        
        internal static Expression<Func<SalesForecast, SalesForecastDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new SalesForecastDto()
                {
                    Id = entity.Id,
                    MonthYear = Updatable.Create(entity.MonthYear),
                    Volume = Updatable.Create(entity.Volume),
                };
            }
        }

        public SalesForecast ToEntity(SalesForecast entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new SalesForecast
                {

                };
            }

            if (Updatable.IsUpdated(MonthYear))
                entity.MonthYear = MonthYear.Value.DateWithMinTime();

            if (Updatable.IsUpdated(Volume))
                entity.Volume = Volume.Value;

            return entity;
        }
    }
}
