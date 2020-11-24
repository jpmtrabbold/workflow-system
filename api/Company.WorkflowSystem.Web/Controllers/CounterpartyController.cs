using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Application.Models.Dtos.Counterparties;
using Company.WorkflowSystem.Application.Models.ViewModels.Counterparties;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CounterpartyController : Controller
    {
        readonly CounterpartyService _service;
        readonly UserService _userService;
        public CounterpartyController(CounterpartyService service, UserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("List")]
        async public Task<CounterpartiesListResponse> List([FromBody] CounterpartiesListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<CounterpartyDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<CounterpartyPostResponse> Post([FromBody] CounterpartyDto counterparty) => await _service.Save(counterparty);

        [HttpGet("CheckCodeUsedInDeals")]
        public async Task<bool> CheckCodeUsedInDeals(int counterpartyId) =>
            await _service.CheckCodeUsedInDeals(counterpartyId);

        [HttpGet("CheckForDuplicateCodes")]
        async public Task<bool> CheckForDuplicateCodes(int counterpartyId, string code) =>
            await _service.CheckForDuplicateCodes(counterpartyId, code);

        [HttpGet("GetCountries")]
        public async Task<List<LookupRequest>> GetCountries() =>
            await _service.GetCountries();
    }
}