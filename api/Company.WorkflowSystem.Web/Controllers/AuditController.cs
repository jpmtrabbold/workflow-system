using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Models.ViewModels.Audit;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuditController : BaseController
    {
        readonly AuditService _service;
        public AuditController(AuditService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<AuditEntriesListResponse> List([FromBody] AuditEntriesListRequest listRequest) => await _service.List(listRequest);
    }
}
