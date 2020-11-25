
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Exceptions;
using System;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Company.WorkflowSystem.Service.Models.Helpers;
using System.Transactions;
using System.Linq.Expressions;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Database.Context;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Company.WorkflowSystem.Service.Extensions;
using System.Linq;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Service.Services
{
    public class DealService : BaseService
    {
        private readonly DealWorkflowService _dealWorkflowService;
        private readonly UserService _userService;
        public DealService(IRepository<TradingDealsContext> repo, DealWorkflowService dealWorkflowService, UserService userService, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {
            _dealWorkflowService = dealWorkflowService;
            _userService = userService;
        }

        async public Task<DealsListResponse> List(DealsListRequest listRequest)
        {
            var userId = GetUserId();
            var builder = _repo.ProjectedListBuilder(DealListDto.ProjectionFromEntity(userId), listRequest)
                .OrderBy(d => d.Id, descending: true)
                .ConditionalOrder("dealNumber", d => d.DealNumber)
                .ConditionalOrder("dealStatusName", d => d.DealStatusName)
                .ConditionalOrder("counterpartyName", d => d.CounterpartyName)
                .ConditionalOrder("dealCategoryName", d => d.DealCategoryName)
                .ConditionalOrder("dealTypeName", d => d.DealTypeName)
                .ConditionalOrder("assignedTo", d => d.AssignedTo)
                .ConditionalOrder("creationDate", d => d.CreationDate)
                .ConditionalOrder("creationUserName", d => d.CreationUserName)
                .ConditionalOrder("executed", d => (d.Executed ? "Yes" : "No"));

            if (listRequest.DealId.HasValue)
                builder.Where(d => d.Id == listRequest.DealId);

            if (!listRequest.IncludeFinalizedDeals)
                builder.Where(d => !d.IsFinalized);

            if (listRequest.OnlyDealsAssignedToMeOrMyRole)
                builder.Where(d => d.DealAssignedToCurrentUserOrHisRole);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(d =>
                    d.DealNumber.Contains(str)
                    || d.CounterpartyName.Contains(str)
                    || d.DealStatusName.Contains(str)
                    || d.DealTypeName.Contains(str)
                    || d.DealCategoryName.Contains(str)
                    || d.AssignedTo.Contains(str)
                    || (d.Executed ? "Yes" : "No").Contains(str)
                    || d.CreationUserName.Contains(str)
                    );
            }

            if (listRequest.StartCreationDate.HasValue)
            {
                var start = listRequest.StartCreationDate.Value.DateWithMinTime();
                builder.Where(d => d.CreationDate >= start);
            }
            if (listRequest.EndCreationDate.HasValue)
            {
                var end = listRequest.EndCreationDate.Value.DateWithMaxTime();
                builder.Where(d => d.CreationDate <= end);
            }

            var deals = await builder.ExecuteAsync();
            var count = await builder.CountAsync();

            return new DealsListResponse
            {
                Deals = deals,
                TotalRecords = count
            };
        }

        async public Task<DealTypeConfigurationResponse> GetDealTypeConfiguration(int dealTypeId, int? itemFieldsetId = null, int? workflowSetId = null, int? currentWorkflowStatusId = null)
        {
            var dealConfigurationResponse = await _repo.ProjectedGetById(dealTypeId, 
                DealTypeConfigurationResponse.ProjectionFromEntity(itemFieldsetId, workflowSetId));

            var fieldSetTask = SetDealItemFieldset(dealConfigurationResponse);
            var currentWorkflowStatusTask = _dealWorkflowService.SetCurrentWorkflowStatus(currentWorkflowStatusId, dealConfigurationResponse, workflowSetId);

            await fieldSetTask; await currentWorkflowStatusTask; // these are tasks that don't depend on each other

            return dealConfigurationResponse;
        }

        public async Task<List<AttachmentTypeLookupRequest>> GetAttachmentTypes()
        {
            return await _repo
                .ProjectedListBuilder(AttachmentTypeLookupRequest.ProjectionFromAttachmentType)
                .WhereEntity(at => at.Active)
                .OrderBy(cp => cp.Name)
                .ExecuteAsync();
        }

        class GetAttachmentVersionBinaryFileModel
        {
            internal byte[] FileContent { get; set; }
            internal string FileExtension { get; set; }
            internal string FileName { get; set; }
            internal string DealNumber { get; set; }
            internal static Expression<Func<DealAttachmentVersion, GetAttachmentVersionBinaryFileModel>> ProjectionFromEntity =>
                entity => new GetAttachmentVersionBinaryFileModel()
                {
                    FileContent = entity.File,
                    FileExtension = entity.FileExtension,
                    FileName = entity.FileName,
                    DealNumber = entity.DealAttachment.Deal.DealNumber,
                };
        }

        public async Task<(byte[] fileContent, string fileDownloadName)> GetAttachmentVersionBinaryFile(int attachmentVersionId)
        {
            var version = await _repo.ProjectedGetById(attachmentVersionId, GetAttachmentVersionBinaryFileModel.ProjectionFromEntity);
            var fileName = $"{version.FileName}.{version.FileExtension}";

            if (!fileName.StartsWith(version.DealNumber))
                fileName = $"{version.DealNumber} - {fileName}";

            return (version.FileContent, fileName);
        }

        private async Task SetDealItemFieldset(DealTypeConfigurationResponse dealConfigurationResponse)
        {
            dealConfigurationResponse.DealItemFields = await GetDealItemFieldsetFields(dealConfigurationResponse.DealItemFieldsetId);
        }

        private async Task<List<DealItemFieldReadDto>> GetDealItemFieldsetFields(int? itemFieldSetId)
        {
            if (!itemFieldSetId.HasValue)
                throw new BusinessRuleException("Deal type doesn't have a fieldset setup. Please configure the deal type before using it.");

            var itemsFieldsBuilder = _repo.ProjectedListBuilder(DealItemFieldReadDto.ProjectionFromEntity)
                .Where(dtf => dtf.DealItemFieldsetId == itemFieldSetId.Value)
                .OrderBy(d => d.DisplayOrder);

            return await itemsFieldsBuilder.ExecuteAsync();
        }

        async public Task<List<LookupRequest>> GetDealTypes(int dealCategoryId)
        {
            var deals = _repo.ProjectedListBuilder(LookupRequest.ProjectionFromDealType);
            deals.WhereEntity(dt => dt.DealCategoriesInDealType.Any(pt => pt.DealCategoryId == dealCategoryId))
                .OrderBy(dt => dt.Name);

            return await deals.ExecuteAsync();
        }

        async public Task<List<LookupRequest>> GetDealCategories()
        {
            var dealCategories = _repo.ProjectedListBuilder(LookupRequest.ProjectionFromDealCategory);

            dealCategories.OrderBy(pt => pt.Name);

            return await dealCategories.ExecuteAsync();
        }

        async public Task<List<LookupRequest>> GetProducts(int dealCategoryId)
        {
            return await _repo.ProjectedListBuilder(LookupRequest.ProjectionFromProduct)
                .WhereEntity(dt => dt.DealCategoryId == dealCategoryId)
                .OrderBy(dt => dt.Name)
                .ExecuteAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dealId">Id of the deal</param>
        /// <param name="light">if true, gets the deal without its dealItems</param>
        /// <returns></returns>
        async public Task<DealDto> Get(int dealId, bool light = false)
        {
            var userId = GetUserId();
            var deal = await _repo.ProjectedGetById(dealId, DealDto.ProjectionFromEntity(userId, light));
            await _dealWorkflowService.SetPossibleAssignmentsForApproval(deal, this);

            return deal;
        }

        async public Task<List<DealItemDto>> GetItems(int dealId)
        {
            return await _repo.ProjectedListBuilder(DealItemDto.ProjectionFromEntity)
                .WhereEntity(e => e.DealId == dealId && !e.OriginalItemId.HasValue)
                .OrderBy(t => t.Order)
                .ExecuteAsync();
        }

        async public Task<List<DealNoteDto>> GetNotes(int dealId)
        {
            return await _repo.ProjectedListBuilder(DealNoteDto.ProjectionFromEntity)
                .Where(e => e.DealId == dealId)
                .ExecuteAsync();
        }
        async public Task<List<DealAttachmentDto>> GetAttachments(int dealId)
        {
            return await _repo.ProjectedListBuilder(DealAttachmentDto.ProjectionFromEntity)
                .WhereEntity(e => e.DealId == dealId)
                .ExecuteAsync();
        }

        readonly List<string> CounterpartyCodes = new List<string>();

        async public Task<DealPostResponse> Save(DealDto deal, int? auditEntryId = null)
        {
            var response = new DealPostResponse();
            using (var scope = CreateTransactionScope())
            {
                var userId = GetUserId();
                var creation = !deal.Id.HasValue || deal.Id == 0;

                Deal entity = null;
                if (!creation)
                {
                    entity = await _repo.Context.Deals
                        .Include(d => d.Items)
                        .Include(d => d.Notes)
                        .Include(d => d.DealWorkflowStatuses).ThenInclude(d => d.Tasks).ThenInclude(t => t.WorkflowTask)
                        .Include(d => d.DealWorkflowStatuses).ThenInclude(d => d.PrecedingWorkflowAction)
                        .AsQueryable()
                        .SingleOrDefaultAsync(d => d.Id == deal.Id);
                }

                Validate(deal, entity, creation, userId);

                if (creation)
                    await GenerateCodeSequence(deal);

                entity = deal.ToEntity(entity, this);

                DeleteEmptyAttachments(entity, _repo);
                SetUserId(entity, userId);

                var (currentStatus, nextStatus, statusWasConfirmed) = await _dealWorkflowService.SetWorkflowData(deal, entity, userId);

                auditEntryId = await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals, auditEntryId);

                await _dealWorkflowService.SetWorkflowNextStatus(entity, currentStatus, nextStatus, auditEntryId.Value);

                if (statusWasConfirmed)
                {
                    await _dealWorkflowService.SendCurrentStatusNotifications(entity, response, await GetDealItemFieldsetFields(entity.DealItemFieldsetId), execution: false);
                    await _dealWorkflowService.SendCurrentStatusNotifications(entity, response, await GetDealItemFieldsetFields(entity.DealItemFieldsetId), execution: true);
                }                    

                response.DealId = entity.Id;
                response.DealNumber = entity.DealNumber;

                scope.Complete();
            }

            return response;
        }

        void DeleteEmptyAttachments(Deal entity, IRepository<TradingDealsContext> _repo)
        {
            foreach (var attachment in entity.Attachments)
            {
                if (attachment.Id == 0)
                    continue;

                var hasVersions = false;
                foreach (var version in attachment.Versions)
                {
                    if (!_repo.IsEntityDeleted(version))
                    {
                        hasVersions = true;
                        break;
                    }
                }
                if (!hasVersions)
                {
                    _repo.Remove(attachment);
                }
            }
        }

        void Validate(DealDto deal, Deal entity, bool creation, int? userId)
        {
            if (!userId.HasValue)
                throw new BusinessRuleException("The user is not a valid user in the database.");

            if (Updatable.IsUpdatedButEmpty(deal.DealCategoryId))
                throw new BusinessRuleException("Please select a deal category.");

            if (Updatable.IsUpdatedButEmpty(deal.DealTypeId))
                throw new BusinessRuleException("Please select a deal type.");

            if (Updatable.IsUpdatedButEmpty(deal.CounterpartyId))
                throw new BusinessRuleException("Please select a counterparty.");

            if (deal.CurrentDealWorkflowStatusId != null)
            {
                if (deal.CurrentDealWorkflowStatusId != entity.CurrentDealWorkflowStatusId)
                {
                    throw new BusinessRuleException("While you had this deal opened, it had a change of status by someone else. " +
                        "Because of that, your changes couldn't be saved.");
                }

                if (deal.CurrentDealWorkflowStatusId.HasValue)
                {
                    var previousId = deal.CurrentDealWorkflowStatusId.Value;
                    var previousAssignee = deal.DealWorkflowStatuses.FirstOrDefault(ws => ws.Id == previousId).AssigneeUserId;
                    var currentAssignee = entity.DealWorkflowStatuses.FirstOrDefault(ws => ws.Id == previousId).AssigneeUserId;
                    if (previousAssignee != currentAssignee)
                    {
                        throw new BusinessRuleException("While you had this deal opened, the assignee was changed by someone else. " +
                            "Because of that, your changes couldn't be saved.");
                    }
                }
            }
        }

        async Task GenerateCodeSequence(DealDto deal)
        {
            var sequence = 0;
            var code = (await _repo.GetById<Counterparty>(deal.CounterpartyId.Value)).Code;
            var codeSequence = await _repo.Get<DealCodeSequence>(dcs => dcs.Code == code).FirstOrDefaultAsync();
            if (codeSequence == null)
            {
                await _repo.SaveEntity(new DealCodeSequence { Code = code, NextSequence = 2 });
                sequence = 1;
            }
            else
            {
                sequence = codeSequence.NextSequence;
                codeSequence.NextSequence++;
                await _repo.SaveEntity(codeSequence);
            }
            deal.DealNumber = code.Trim() + sequence.ToString("D5").Trim();
        }

        public void SetUserId(Deal deal, int? userId)
        {
            if (!userId.HasValue || userId == 0)
                throw new Exception("Invalid userId");

            foreach (var note in deal.Notes.Where(n => n.CreationUserId == 0))
                note.CreationUserId = (int)userId;

            foreach (var attachment in deal.Attachments)
                foreach (var version in attachment.Versions.Where(n => n.CreationUserId == 0))
                    version.CreationUserId = (int)userId;
        }

        public async Task<DealExecutionInfoResponse> DealExecutionInfo(int dealId)
        {
            var userId = GetUserId();
            var assignInfo = await _repo.ProjectedGetById(dealId, (Deal d) => new DealExecutionInfoResponse
            {
                AssignedToSelf = d.ExecutionUserId == userId,
                AssignedToUserName = d.ExecutionUser != null ? d.ExecutionUser.Name : null,
                AssignedToUserId = d.ExecutionUserId,
                Executed = d.Executed,
                ExecutionDate = d.ExecutionDate,
            });

            return assignInfo;
        }

        async public Task AssignDealExecutionToSelf(int dealId, int? currentExecutionUserId, DateTimeOffset? currentExecutionDate)
        {
            var userId = GetUserId();
            var entity = await _repo.GetById<Deal>(dealId);

            if (entity.ExecutionUserId != currentExecutionUserId || entity.ExecutionDate != currentExecutionDate || entity.Executed)
                throw new BusinessRuleException("While taking this action, another user changed this deal's execution. Because of that, the operation cannot be completed.");

            entity.ExecutionUserId = userId;

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals);
        }

        async public Task ReverseDealExecution(int dealId, int? currentExecutionUserId, DateTimeOffset? currentExecutionDate)
        {
            var entity = await _repo.GetById<Deal>(dealId);

            if (entity.ExecutionUserId != currentExecutionUserId || entity.ExecutionDate != currentExecutionDate || !entity.Executed)
                throw new BusinessRuleException("While taking this action, another user changed this deal's execution. Because of that, the operation cannot be completed.");

            entity.ExecutionUserId = null;
            entity.ExecutionDate = null;
            entity.Executed = false;

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals);
        }

        public async Task<DealDirectWorkflowActionResponse> PerformDirectWorkflowAction(DealDirectWorkflowActionRequest request)
        {
            var response = new DealDirectWorkflowActionResponse();

            // get the deal's current workflow status
            var dealDetails = (await _repo.ProjectedList((Deal d) => new { d.Id, d.CurrentDealWorkflowStatusId, d.DealNumber }, d => d.Id == request.DealId)).FirstOrDefault();
            if (!dealDetails.CurrentDealWorkflowStatusId.HasValue)
                return response.Error("The deal is invalid.");
            
            response.DealNumber = dealDetails.DealNumber;
            // checks if there is a listener for that status, key and user
            var hasListener = _repo.Context.DealWorkflowActionListeners
                .Any(l => l.DealWorkflowStatusId == dealDetails.CurrentDealWorkflowStatusId && l.UniqueActionGuid == request.Key && l.UserId == request.UserId);
            if (!hasListener)
                return response.Error("This deal's status has changed so this action became obsolete. Please access it on WorkflowSystem for more information.");

            // the user was validated by the listener's key and the data cross reference
            await LogUserIn(request.UserId);

            var workflowAction = await _repo.ProjectedGetById(request.ActionId, (WorkflowAction a) => new
            {
                a.Name,
                hasReasonTask = a.Tasks.Any(t => t.Type == WorkflowTaskTypeEnum.EnterTextInformation)
            });

            if (string.IsNullOrWhiteSpace(request.Reason) && workflowAction.hasReasonTask)
                return response.SetReasonAsRequired();

            if (request.ActionName != workflowAction.Name)
                return response.Error("Action payload is corrupted. You probably manipulated the link.");

            var messages = await PerformWorkflowAction(request.DealId, request.ActionId, request.Reason);
            response.Messages.AddRange(messages);

            return response;
        }

        public async Task<List<string>> PerformWorkflowAction(int dealId, int actionId, string reason = null)
        {
            var messages = new List<string>();
            using (var scope = CreateTransactionScope())
            {
                DealDto deal;
                deal = await Get(dealId, true);

                // set the ongoing workflow action
                deal.OngoingWorkflowActionId.Value = actionId;
                deal.OngoingWorkflowActionId.Updated = true;
                deal.OngoingWorkflowActionNote = reason;

                DealPostResponse dealSaveResponse;
                dealSaveResponse = await Save(deal);

                messages.AddRange(dealSaveResponse.WarningMessages);

                deal = await Get(dealId, true);

                // confirm the workflow action
                deal.CompletedActionId = actionId;

                dealSaveResponse = await Save(deal);

                messages.AddRange(dealSaveResponse.WarningMessages);

                scope.Complete();
            }
            return messages;
        }
    }


    public class TimeTracker : IDisposable
    {
        Stopwatch watch { get; set; }
        string description { get; set; }

        public TimeTracker(string desc)
        {
            watch = new Stopwatch();
            description = desc;
            watch.Start();
        }

        public void Dispose()
        {
            watch.Stop();
            Console.WriteLine($"{description} - took {watch.ElapsedMilliseconds}");
        }
    }
}