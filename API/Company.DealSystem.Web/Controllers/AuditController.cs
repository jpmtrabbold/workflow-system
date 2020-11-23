using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.ViewModels.Audit;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
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
