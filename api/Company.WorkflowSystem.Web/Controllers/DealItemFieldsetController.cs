using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Service.Models.Dtos.DealItemFieldsets;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Models.ViewModels.ItemFieldsets;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DealItemFieldsetController : Controller
    {
        readonly DealItemFieldsetService _service;
        public DealItemFieldsetController(DealItemFieldsetService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<DealItemFieldsetsListResponse> List([FromBody] DealItemFieldsetsListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<DealItemFieldsetDto> Get(int id) => await _service.Get(id);

        [HttpGet("GetItemFieldLookups")]
        public List<StringLookupRequest> GetItemFieldLookups() => _service.GetItemFieldLookups();

        [HttpPost]
        async public Task<DealItemFieldsetPostResponse> Post([FromBody] DealItemFieldsetDto itemFieldset)
        {
            return await _service.Save(itemFieldset);
        }

    }
}