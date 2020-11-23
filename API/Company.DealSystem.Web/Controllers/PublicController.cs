using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.ViewModels.Counterparties;
using Company.DealSystem.Application.Models.ViewModels.Deals;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
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
