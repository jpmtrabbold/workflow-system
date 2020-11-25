using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.SalesForecasts
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
