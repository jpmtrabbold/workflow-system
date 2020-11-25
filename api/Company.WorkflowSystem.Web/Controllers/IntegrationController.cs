using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Models.Dtos.Integration;
using Company.WorkflowSystem.Service.Models.Dtos.Users;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Models.ViewModels.Users;
using Company.WorkflowSystem.Service.Services;
using Company.WorkflowSystem.Domain.Enum;

namespace Company.WorkflowSystem.Web.Controllers
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
