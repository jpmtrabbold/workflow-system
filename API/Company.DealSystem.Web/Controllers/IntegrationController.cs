using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.Dtos.Integration;
using Company.DealSystem.Application.Models.Dtos.Users;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Models.ViewModels.Users;
using Company.DealSystem.Application.Services;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IntegrationController : BaseController
    {
        readonly IntegrationService _service;
        public IntegrationController(IntegrationService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<IntegrationRunsListResponse> List([FromBody] IntegrationRunsListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("GetEntries")]
        async public Task<List<IntegrationRunEntryDto>> GetEntries(int integrationRunId) => await _service.GetEntries(integrationRunId);
        [HttpPost("ChangeIntegrationRunStatus")]
        async public Task ChangeIntegrationRunStatus(int integrationRunId, IntegrationRunStatusEnum currentStatus, IntegrationRunStatusEnum newStatus) =>
            await _service.ChangeIntegrationRunStatus(integrationRunId, currentStatus, newStatus);

    }
}
