using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.SalesForecasts;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.SalesForecasts
{
    public class SalesForecastsListResponse : ListResponse
    {
        public List<SalesForecastListDto> SalesForecasts { get; set; }
    }
}
