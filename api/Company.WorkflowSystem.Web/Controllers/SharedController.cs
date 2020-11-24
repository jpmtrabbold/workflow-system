using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Web.Controllers
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
