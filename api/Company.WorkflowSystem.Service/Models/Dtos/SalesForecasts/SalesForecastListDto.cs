using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Service.Models.Dtos.SalesForecasts
{
    public class SalesForecastListDto
    {
        public int Id { get; set; }
        public DateTimeOffset MonthYear { get; set; }
        public decimal Volume { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<SalesForecast, SalesForecastListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new SalesForecastListDto()
                {
                    Id = entity.Id,
                    MonthYear = entity.MonthYear,
                    Volume = entity.Volume,
                };
            }
        }
    }
}
