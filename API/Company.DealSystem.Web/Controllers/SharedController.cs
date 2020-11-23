using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : ControllerBase
    {
        IConfiguration _configuration;
        public SharedController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("EnumsDefinitions")]
        public ActionResult<EnumsDefinitionsRequest> EnumsDefinitions()
        {
            return new EnumsDefinitionsRequest();
        }
    }
}
