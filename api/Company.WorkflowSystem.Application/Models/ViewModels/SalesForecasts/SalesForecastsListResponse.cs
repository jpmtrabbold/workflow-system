using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.SalesForecasts;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.SalesForecasts
{
    public class SalesForecastsListResponse : ListResponse
    {
        public List<SalesForecastListDto> SalesForecasts { get; set; }
    }
}
