using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Service.Models.Dtos.Products;
using Company.WorkflowSystem.Service.Models.ViewModels.DealIntegration;
using Company.WorkflowSystem.Service.Models.ViewModels.Nodes;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DealIntegrationController : Controller
    {
        readonly DealIntegrationService _service;
        public DealIntegrationController(DealIntegrationService service)
        {
            _service = service;
        }

        [HttpPost("EmsFetch")]
        async public Task<bool> EmsFetch([FromBody] EmsFetchRequest request)
        {
            return await _service.EmsFetch(request);
        }
    }
}