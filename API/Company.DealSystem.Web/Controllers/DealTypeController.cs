using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.DealSystem.Application.Models.Dtos.DealTypes;
using Company.DealSystem.Application.Models.ViewModels.DealTypes;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DealTypeController : Controller
    {
        readonly DealTypeService _service;
        public DealTypeController(DealTypeService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<DealTypesListResponse> List([FromBody] DealTypesListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("GetWorkflowSetLookups")]
        async public Task<List<LookupRequest>> GetWorkflowSetLookups(int id) => await _service.GetWorkflowSetLookups();

        [HttpGet("GetDealItemFieldsetLookups")]
        async public Task<List<LookupRequest>> GetDealItemFieldsetLookups(int id) => await _service.GetDealItemFieldsetLookups();

        [HttpGet("{id}")]
        async public Task<DealTypeDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<DealTypePostResponse> Post([FromBody] DealTypeDto dealType)
        {
            return await _service.Save(dealType);
        }
    }
}