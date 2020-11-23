using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.Dtos.Configuration;
using Company.DealSystem.Application.Models.ViewModels.Configuration;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConfigurationController : BaseController
    {
        readonly ConfigurationService _service;
        public ConfigurationController(ConfigurationService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<ConfigurationGroupsListResponse> List([FromBody] ConfigurationGroupsListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<ConfigurationGroupDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<ConfigurationGroupPostResponse> Post([FromBody] ConfigurationGroupDto model)
        {
            return await _service.Save(model);
        }
    }
}
