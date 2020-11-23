using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.SalesForecasts;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.SalesForecasts
{
    public class SalesForecastsListResponse : ListResponse
    {
        public List<SalesForecastListDto> SalesForecasts { get; set; }
    }
}
