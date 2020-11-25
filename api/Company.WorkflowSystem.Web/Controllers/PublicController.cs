using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Counterparties;
using Company.WorkflowSystem.Service.Models.ViewModels.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : BaseController
    {
        readonly DealService _service;
        
        public PublicController(DealService service)
        {
            _service = service;
        }

        [HttpPost("DealDirectWorkflowAction")]
        async public Task<DealDirectWorkflowActionResponse> DealDirectWorkflowAction([FromBody] DealDirectWorkflowActionRequest request)
        {
            return await _service.PerformDirectWorkflowAction(request);
        }
    }
}
