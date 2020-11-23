using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.DealSystem.Application.Models.Dtos.Products;
using Company.DealSystem.Application.Models.ViewModels.DealIntegration;
using Company.DealSystem.Application.Models.ViewModels.Nodes;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
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