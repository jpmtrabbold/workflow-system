using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Service.Models.Dtos.SalesForecasts;
using Company.WorkflowSystem.Service.Models.ViewModels.SalesForecasts;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SalesForecastController : Controller
    {
        readonly SalesForecastService _service;
        public SalesForecastController(SalesForecastService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<SalesForecastsListResponse> List([FromBody] SalesForecastsListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<SalesForecastDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<SalesForecastPostResponse> Post([FromBody] SalesForecastDto dealCategory)
        {
            return await _service.Save(dealCategory);
        }

        [HttpPost("BulkImport")]
        async public Task BulkImport([FromBody] List<SalesForecastDto> list)
        {
            await _service.BulkImport(list);
        }
    }
}