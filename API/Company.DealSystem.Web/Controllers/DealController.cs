using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DealController : BaseController
    {
        readonly DealService _service;
        readonly DealWorkflowService _dealWorkflowService;
        readonly CounterpartyService _counterpartyService;

        
        public DealController(DealService service, DealWorkflowService dealWorkflowService, CounterpartyService counterpartyService)
        {
            _service = service;
            _dealWorkflowService = dealWorkflowService;
            _counterpartyService = counterpartyService;
        }

        [HttpPost("List")]
        async public Task<DealsListResponse> List([FromBody] DealsListRequest listRequest)
        {
            return await _service.List(listRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">deal id</param>
        /// <param name="light">if true, does not bring the deal's deal items</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        async public Task<DealDto> Get(int id, bool light = false) => await _service.Get(id, light);

        [HttpGet("GetItems/{id}")]
        async public Task<List<DealItemDto>> GetItems(int id) => await _service.GetItems(id);
        [HttpGet("GetNotes/{id}")]
        async public Task<List<DealNoteDto>> GetNotes(int id) => await _service.GetNotes(id);
        [HttpGet("GetAttachments/{id}")]
        async public Task<List<DealAttachmentDto>> GetAttachments(int id) => await _service.GetAttachments(id);

        [HttpPost]
        async public Task<DealPostResponse> Post([FromBody] DealDto deal)
        {
            return await _service.Save(deal);
        }

        [HttpPost("DealAssignInfo")]
        async public Task<DealAssignInfoResponse> DealAssignInfo(int dealId)
        {
            return await _dealWorkflowService.DealAssignInfo(dealId);
        }

        [HttpPost("AssignDealToSelf")]
        async public Task AssignDealToSelf(int dealId, int? currentAssignedToUserId = null)
        {
            await _dealWorkflowService.AssignDealToUser(dealId, currentAssignedToUserId);
        }

        [HttpPost("RevertDealStatusBack")]
        async public Task RevertDealStatusBack(int dealId, int workflowStatusId, int? currentDealworkflowStatusId, int? currentAssignedToUserId = null)
        {
            await _dealWorkflowService.RevertDealStatusBackTo(dealId, workflowStatusId, currentDealworkflowStatusId, currentAssignedToUserId);
        }

        [HttpPost("DealExecutionInfo")]
        async public Task<DealExecutionInfoResponse> DealExecutionInfo(int dealId)
        {
            return await _service.DealExecutionInfo(dealId);
        }
        [HttpPost("AssignDealExecutionToSelf")]
        async public Task AssignDealExecutionToSelf(int dealId, int? currentExecutionUserId, DateTimeOffset? currentExecutionDate)
        {
            await _service.AssignDealExecutionToSelf(dealId, currentExecutionUserId, currentExecutionDate);
        }

        [HttpPost("ReverseDealExecution")]
        async public Task ReverseDealExecution(int dealId, int? currentExecutionUserId, DateTimeOffset? currentExecutionDate)
        {
            await _service.ReverseDealExecution(dealId, currentExecutionUserId, currentExecutionDate);
        }

        [HttpGet("GetDealCategories")]
        async public Task<List<LookupRequest>> GetDealCategories() => 
            await _service.GetDealCategories();

        [HttpGet("GetDealTypes")]
        async public Task<List<LookupRequest>> GetDealTypes(int dealCategoryId) => 
            await _service.GetDealTypes(dealCategoryId);

        [HttpPost("GetCounterparties")]
        async public Task<LookupRequestHeader> GetCounterparties([FromBody] CounterpartiesListRequest listRequest) => 
            await _counterpartyService.DropdownList(listRequest);

        [HttpGet("GetNodes")]
        async public Task<List<LookupRequest>> GetProducts(int dealCategoryId) => 
            await _service.GetProducts(dealCategoryId);

        [HttpGet("GetDealTypeConfiguration")]
        async public Task<DealTypeConfigurationResponse> GetDealTypeConfiguration(int dealTypeId, int? itemFieldsetId = null, int? workflowSetId = null, int? currentWorkflowStatusId = null) => 
            await _service.GetDealTypeConfiguration(dealTypeId, itemFieldsetId, workflowSetId, currentWorkflowStatusId);

        [HttpGet("GetAttachmentTypes")]
        async public Task<List<AttachmentTypeLookupRequest>> GetAttachmentTypes() =>
            await _service.GetAttachmentTypes();

        [HttpGet("DownloadAttachmentVersion")]
        async public Task<FileContentResult> DownloadAttachmentVersion(int attachmentVersionId)
        {
            (byte[] fileContent, string fileDownloadName) = await _service.GetAttachmentVersionBinaryFile(attachmentVersionId);

            return DownloadFile(fileContent, fileDownloadName);
        }

        [HttpGet("GetTradePolicyEvaluation")]
        async public Task<List<DealWorkflowAssignmentDto>> GetTradePolicyEvaluation(int dealId, int userId) => 
            await _dealWorkflowService.GetTradePolicyEvaluation(dealId, userId, _service);
    }
}
