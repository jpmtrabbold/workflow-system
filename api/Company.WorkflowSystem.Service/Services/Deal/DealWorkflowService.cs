using LinqKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Dtos.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Deals;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using Company.WorkflowSystem.Service.Exceptions;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Company.WorkflowSystem.Service.Models.Helpers;
using System.Transactions;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Database.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Company.WorkflowSystem.Service.DataAggregators;
using Company.WorkflowSystem.Domain.Util;
using Company.WorkflowSystem.Domain.Services;
using Company.WorkflowSystem.Service.Extensions;
using Company.WorkflowSystem.Service.Utils;
using Company.WorkflowSystem.Domain.ExtensionMethods;

namespace Company.WorkflowSystem.Service.Services
{
    public class DealWorkflowService : BaseService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        public DealWorkflowService(
            IRepository<TradingDealsContext> repo,
            ScopedDataService scopedDataService,
            IEmailService emailService,
            IConfiguration configuration,
            UserService userService
            ) : base(repo, scopedDataService)
        {
            _emailService = emailService;
            _configuration = configuration;
            _userService = userService;
        }

        async public Task AssignDealToUser(int dealId, int? currentAssignee = null)
        {
            var userId = GetUserId();
            var entity = await _repo.GetById<Deal>(dealId);
            _repo.LoadCollection(entity, d => d.DealWorkflowStatuses);

            var currentStatus = entity.DealWorkflowStatuses.FirstOrDefault(s => s.Id == entity.CurrentDealWorkflowStatusId);
            if (currentStatus == null)
                throw new Exception("Deal does not have a valid current status.");

            if (currentStatus.AssigneeUserId != currentAssignee)
                throw new BusinessRuleException("While taking this action, another user set an assignee to this deal. Because of that, the operation cannot be completed.");

            currentStatus.AssigneeUserId = userId;

            RemoveWorkflowActionListeners(dealId);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals);
        }

        async public Task RevertDealStatusBackTo(int dealId, int workflowStatusId, int? currentDealworkflowStatusId, int? currentAssignee = null)
        {
            using (var scope = CreateTransactionScope())
            {
                var userId = GetUserId();
                var statusName = await _repo.ProjectedGetById(workflowStatusId, (WorkflowStatus s) => s.Name);

                var entity = await _repo.GetById<Deal>(dealId);
                _repo.LoadCollection(entity, d => d.DealWorkflowStatuses);

                var currentDealStatus = entity.DealWorkflowStatuses.FirstOrDefault(s => s.Id == entity.CurrentDealWorkflowStatusId);
                if (currentDealStatus == null)
                    throw new Exception("Deal does not have a valid current status.");

                if (currentDealStatus.Id != currentDealworkflowStatusId)
                    throw new BusinessRuleException("While taking this action, this deal status changed. Because of that, the operation cannot be completed.");

                if (currentDealStatus.AssigneeUserId != currentAssignee)
                    throw new BusinessRuleException("While taking this action, another user set an assignee to this deal. Because of that, the operation cannot be completed.");

                var status = CreateDealWorkflowStatus(
                    workflowStatusId: workflowStatusId,
                    workflowStatusName: statusName,
                    assigneeUserId: userId,
                    initiatedByUserId: userId,
                    dateTimeConfirmed: DateUtils.GetDateTimeOffsetNow(),
                    nameSuffix: "Reverted back by user",
                    revertedBack: true);

                entity.DealWorkflowStatuses.Add(status);

                entity.PreviousDealWorkflowStatusId = null;
                entity.NextDealWorkflowStatusId = null;
                RemoveWorkflowActionListeners(dealId);

                await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals);

                await SetWorkflowNextStatus(entity, status);

                scope.Complete();
            }
        }

        public void RemoveWorkflowActionListeners(int dealId)
        {
            var listeners = _repo.Get<DealWorkflowActionListener>(l => l.DealWorkflowStatus.DealId == dealId);
            foreach (var listener in listeners)
                _repo.Remove(listener);
        }

        public async Task<DealAssignInfoResponse> DealAssignInfo(int dealId)
        {
            var userId = GetUserId();
            var assignInfo = await _repo.ProjectedGetById(dealId, (Deal d) => new DealAssignInfoResponse
            {
                AssignedToSelf = Deal.AssignedToSpecificUser.Invoke(d, userId),
                AssignedToUsersRole = Deal.AssignedToUsersRole.Invoke(d, userId),
                AssignedToUserDescription = (d.CurrentDealWorkflowStatus != null
                    && d.CurrentDealWorkflowStatus.AssigneeUserId.HasValue
                    && d.CurrentDealWorkflowStatus.AssigneeUserId != userId ? d.CurrentDealWorkflowStatus.AssigneeUser.Name : null),
                AssignedToUserId = (d.CurrentDealWorkflowStatus != null
                    && d.CurrentDealWorkflowStatus.AssigneeUserId.HasValue
                    ? d.CurrentDealWorkflowStatus.AssigneeUserId : null),
            });

            // if this deal is not assigned to the user or his role, check if he can revert the status back
            if (!assignInfo.AssignedToSelf || !assignInfo.AssignedToUsersRole)
            {
                // if:
                // - the deal's previous status was assigned to the current user and;
                // - the current status still wasn't picked up by any user and;
                // - the current status is assigned to a workflow role
                if (_repo.Context.Deals.Any(d =>
                    d.Id == dealId &&
                    d.PreviousDealWorkflowStatusId.HasValue &&
                    d.CurrentDealWorkflowStatusId.HasValue &&
                    d.PreviousDealWorkflowStatus.AssigneeUserId == userId &&
                    !d.CurrentDealWorkflowStatus.AssigneeUserId.HasValue &&
                    d.CurrentDealWorkflowStatus.AssigneeWorkflowRoleId.HasValue
                    ))
                {
                    // then the deal can be reverted back to the previous status
                    assignInfo.RevertBackInfo = await _repo.ProjectedGetById(dealId, (Deal d) => new DealAssignRevertBackInfoResponse
                    {
                        CanRevertStatusBack = true,
                        CurrentDealWorkflowStatusId = d.CurrentDealWorkflowStatusId.Value,
                        CurrentDealWorkflowStatusName = d.CurrentDealWorkflowStatus.WorkflowStatusName,
                        PreviousWorkflowStatusId = d.PreviousDealWorkflowStatus.WorkflowStatus.Id,
                        PreviousDealWorkflowStatusName = d.PreviousDealWorkflowStatus.WorkflowStatusName,
                        PrecedingWorkflowActionName = d.CurrentDealWorkflowStatus.PrecedingWorkflowAction.Name,
                        CurrentWorkflowRoleName = d.CurrentDealWorkflowStatus.AssigneeWorkflowRole.Name,
                    });
                }
            }

            return assignInfo;
        }

        public async Task SetCurrentWorkflowStatus(int? currentWorkflowStatusId, DealTypeConfigurationResponse dealConfigurationResponse, int? workflowSetId)
        {
            if (!workflowSetId.HasValue || workflowSetId.Value == 0)
                workflowSetId = dealConfigurationResponse.WorkflowSetId;

            if (!workflowSetId.HasValue || workflowSetId.Value == 0)
                throw new BusinessRuleException("Deal type doesn't have a Workflow Set defined. Please configure the deal type before using it.");

            dealConfigurationResponse.CurrentWorkflowStatusConfig = await GetCurrentWorkflowStatusConfig(workflowSetId.Value, currentWorkflowStatusId);

            if (dealConfigurationResponse.CurrentWorkflowStatusConfig == null)
                throw new Exception($"There is a problem with the workflow configuration for WorkflowSetId {workflowSetId}{(currentWorkflowStatusId.HasValue ? $" and WorkflowStatusId {currentWorkflowStatusId}" : "")}. Query didn't return any status results.");
        }

        async Task<WorkflowStatusReadDto> GetCurrentWorkflowStatusConfig(int workflowSetId, int? currentDealWorkflowStatusId = null)
        {
            // gets the first status of that workflow set
            var workflowBuilder = _repo.ProjectedListBuilder(WorkflowStatusReadDto.ProjectionFromEntity)
                .WhereEntity(ws => ws.WorkflowSetId == workflowSetId)
                .OrderByEntity(ws => ws.Order);

            int? currentWorkflowStatusId = null;
            // but if the deal is in a specific status, that one will be retrieved instead
            if (currentDealWorkflowStatusId.HasValue && currentDealWorkflowStatusId > 0)
            {
                currentWorkflowStatusId = (await _repo.ProjectedListBuilder<DealWorkflowStatus, int?>(dws => dws.WorkflowStatusId)
                    .WhereEntity(dws => dws.Id == currentDealWorkflowStatusId).FirstOrDefaultAsync());

                if (!currentWorkflowStatusId.HasValue)
                    throw new Exception("Internal error: couldn't find a WorkflowStatusId for current deal workflow status.");

                workflowBuilder.Where(ws => ws.Id == currentWorkflowStatusId);
            }

            var status = await workflowBuilder.FirstOrDefaultAsync();
            if (status == null)
                throw new Exception($"Workflow Set {workflowSetId} does not have statuses configured properly.");

            if (!status.FinalizeDeal)
            {
                var additionalActionsWithoutSource = await _repo.ProjectedListBuilder(WorkflowActionReadDto.ProjectionFromEntity())
                    .WhereEntity(wa => wa.Active
                    && wa.TargetWorkflowStatus.WorkflowSetId == workflowSetId
                    && !wa.SourceWorkflowStatusId.HasValue
                    && wa.TargetWorkflowStatusId != currentWorkflowStatusId
                ).ExecuteAsync();

                status.Actions.AddRange(additionalActionsWithoutSource);
            }

            return status;
        }

        public async Task SetWorkflowNextStatus(Deal entity, DealWorkflowStatus currentStatus, DealWorkflowStatus nextStatus = null, int auditEntryId = 0)
        {

            var save = false;
            if (currentStatus != null)
            {
                var id = entity.DealWorkflowStatuses.Last(s => s.WorkflowStatusId == currentStatus.WorkflowStatusId).Id;
                entity.CurrentDealWorkflowStatusId = id;
                save = true;
            }

            if (nextStatus != null)
            {
                var id = entity.DealWorkflowStatuses.Last(s => s.WorkflowStatusId == nextStatus.WorkflowStatusId).Id;
                entity.NextDealWorkflowStatusId = id;
                save = true;
            }

            if (save)
                await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Deals, auditEntryId);

        }
        public async Task<(DealWorkflowStatus currentStatus, DealWorkflowStatus nextStatus, bool statusWasConfirmed)> SetWorkflowData(DealDto deal, Deal entity, int userId)
        {
            DealWorkflowStatus currentStatus = null;
            DealWorkflowStatus nextStatus = null;

            currentStatus = await InitializeWorkflowStatus(entity, userId, currentStatus);

            nextStatus = await StartWorkflowAction(deal, entity, nextStatus, userId);

            var statusWasConfirmed = await ConfirmWorkflowAction(deal.CompletedActionId, entity, userId);

            return (currentStatus, nextStatus, statusWasConfirmed);
        }

        async Task<string> DealHeaderHtml(Deal entity)
        {
            var deal = await _repo.ProjectedGetById(entity.Id, (Deal d) => new
            {
                dealNumber = d.DealNumber,
                counterparty = d.Counterparty.Name,
                dealCategory = d.DealCategory.Name,
                dealType = d.DealType.Name,
                createdBy = d.CreationUser.Name,
                createdOn = d.CreationDate,
            });

            var fields = new List<(string name, string value)>()
            {
                ("Deal Number", deal.dealNumber),
                ("Counterparty", deal.counterparty),
                ("Deal Category", deal.dealCategory),
                ("Deal Type", deal.dealType),
                ("Created By", deal.createdBy),
                ("Created On", deal.createdOn.ToDateTimeWithTimeZoneString()),
            };

            var html = "<table border='1' bordercolor='lightgray'>";

            html += "<tr>";
            foreach (var field in fields)
                html += $"<th style='padding: 4px;'>{field.name}</th>";
            html += "</tr>";

            html += "<tr>";
            foreach (var field in fields)
                html += $"<td style='padding: 4px;'>{field.value}</td>";
            html += "</tr>";

            html += "</table>";

            return html;
        }

        async Task<string> DealItemsHtml(Deal entity, List<DealItemFieldReadDto> itemFields)
        {
            var maxItems = _configuration.GetIntValue("MaxDealItemsPerEmail");

            var dealItems = (await _repo.ProjectedListBuilder(DealItemDto.ProjectionFromEntity)
                .WhereEntity(t => t.DealId == entity.Id)
                .OrderBy(t => t.StartDate)
                .ExecuteAsync());

            var fieldDefinitions = DealItemDto.ItemFields;

            var html = "<table border='1' bordercolor='lightgray'>";

            html += "<tr>";
            foreach (var field in itemFields)
                html += $"<th style='padding: 4px;'>{field.Name}</th>";
            html += "</tr>";

            for (int i = 0; i < dealItems.Count; i++)
            {
                if (i == maxItems)
                {
                    var phrase = $"These are the first {maxItems} dealItems of the deal. " +
                        $"Please open the deal on WorkflowSystem or refer to Middle Office " +
                        $"if you wish to view all {dealItems.Count} dealItems";

                    html += $"<tr><td colspan='9999' style='padding: 4px;'>{phrase}</td></tr>";
                    break;
                }
                else
                {
                    var dealItem = dealItems[i];
                    html += "<tr>";
                    foreach (var field in itemFields)
                    {
                        var definition = fieldDefinitions.First(fd => fd.fieldName == field.Field);
                        html += $"<td style='padding: 4px;'>{definition.stringDTOValue(dealItem)}</td>";
                    }
                    html += "</tr>";
                }

            }

            html += "</table>";

            return html;
        }

        async Task<string> DealNotesHtml(Deal entity)
        {
            var notes = await _repo.ProjectedList(DealNoteDto.ProjectionFromEntity, d => d.DealId == entity.Id);
            var fields = new List<string>() { "Note Created", "By User", "Note Content" };

            var html = "<table border='1' bordercolor='lightgray'>";

            html += "<tr>";
            foreach (var field in fields)
                html += $"<th style='padding: 4px;'>{field}</th>";
            html += "</tr>";

            for (int i = 0; i < notes.Count; i++)
            {
                var note = notes[i];
                html += "<tr>";
                html += $"<td style='padding: 4px;'>{note.CreatedDate.Value.ToDateTimeWithTimeZoneString()}</td>";
                html += $"<td style='padding: 4px;'>{note.NoteCreatorName}</td>";
                html += $"<td style='padding: 4px;'>{note.NoteContent.Value}</td>";
                html += "</tr>";
            }

            html += "</table>";

            return html;
        }

        internal async Task SendCurrentStatusNotifications(Deal entity, DealPostResponse response, List<DealItemFieldReadDto> itemFields, bool execution)
        {
            var workflowData = await _repo.ProjectedGetById(entity.CurrentDealWorkflowStatusId,
                (DealWorkflowStatus s) => new { WorkflowRoleName = s.AssigneeWorkflowRole.Name, s.WorkflowStatusId, s.WorkflowStatus.AllowsDealExecution });

            int? userId = null;
            int? workflowRoleId = null;
            string assigningMessage = null;
            string subject = null;
            var users = new List<(string email, int userId, string userName, string key)>();
            var actions = new List<WorkflowAction>();
            if (entity.CurrentDealWorkflowStatus.InitiatedByUser == null)
                _repo.Context.Entry(entity.CurrentDealWorkflowStatus).Reference(da => da.InitiatedByUser).Load();
            var actionUserName = entity.CurrentDealWorkflowStatus.InitiatedByUser.Name;

            if (execution)
            {
                if (!workflowData.AllowsDealExecution)
                    return;

                var user = _repo.Context.Users.First(u => u.Id == entity.SubmissionUserId);
                users.Add((user.Username, user.Id, user.Name, Guid.NewGuid().ToString()));
                assigningMessage = $"The deal {entity.DealNumber} " +
                        $"has now the {entity.CurrentDealWorkflowStatus.WorkflowStatusName} status " +
                        $"(actioned by {actionUserName}) " +
                        $"and now can be executed by you.";

                subject = $"Deal {entity.DealNumber} - Ready for Execution";
            }
            else
            {
                userId = entity.CurrentDealWorkflowStatus.AssigneeUserId;
                workflowRoleId = entity.CurrentDealWorkflowStatus.AssigneeWorkflowRoleId;

                // one user will be notified
                if (userId.HasValue)
                {
                    assigningMessage = $"The deal {entity.DealNumber} " +
                        $"has now the {entity.CurrentDealWorkflowStatus.WorkflowStatusName} status " +
                        $"(actioned by {actionUserName}) " +
                        $"and is assigned to you.";

                    var user = _repo.Context.Users.First(u => u.Id == userId);
                    users.Add((user.Username, user.Id, user.Name, Guid.NewGuid().ToString()));
                }
                else // all users in that workflow role will be notified
                {
                    assigningMessage = $"The deal {entity.DealNumber} " +
                        $"has now the {entity.CurrentDealWorkflowStatus.WorkflowStatusName} status " +
                        $"(actioned by {actionUserName}) " +
                        $"and is assigned to your workflow role ({workflowData.WorkflowRoleName}).";

                    users.AddRange(
                        _repo.Context.Users
                        .OnlyActive()
                        .Where(u => u.WorkflowRolesInUser.Any(wru => wru.WorkflowRoleId == workflowRoleId && wru.Active))
                        .ToList()
                        .Select(u => (u.Username, u.Id, u.Name, Guid.NewGuid().ToString()))
                        );
                }

                actions = _repo.Context.WorkflowActions
                    .OnlyActive()
                    .Where(wa => wa.SourceWorkflowStatusId == workflowData.WorkflowStatusId && wa.DirectActionOnEmailNotification)
                    .AsNoTracking().ToList();

                if (actions.Any())
                {
                    foreach (var user in users)
                    {
                        var listener = new DealWorkflowActionListener
                        {
                            DealWorkflowStatusId = entity.CurrentDealWorkflowStatusId.Value,
                            UniqueActionGuid = user.key,
                            UserId = user.userId,
                        };
                        await _repo.SaveEntity(listener);
                    }
                }
                subject = $"Deal {entity.DealNumber} - {entity.CurrentDealWorkflowStatus.WorkflowStatusName}";
            }

            var dealHeader = await DealHeaderHtml(entity);
            var dealItems = await DealItemsHtml(entity, itemFields);
            var dealNotes = await DealNotesHtml(entity);

            foreach (var user in users)
            {
                var links = actions
                    .Select(action => ($"/deal-workflow-action/{entity.Id}?userId={user.userId}&key={user.key}&actionId={action.Id}&actionName={action.Name}", action.Name))
                    .ToList();

                if (execution)
                    links.Add(($"/all-deals/{entity.Id}?subFunctionalityId={(int)SubFunctionalityEnum.ExecuteDeal}", "Execute Deal"));
                else
                    links.Add(($"/all-deals/{entity.Id}?subFunctionalityId={(int)SubFunctionalityEnum.Edit}", "Open Deal"));

                var actionLinks = UtilMethods.GenerateHtmlLinkButtons(links, _configuration["FrontEndBaseUrl"]);

                var html = @"
                    <html style='font-family: Helvetica'>
                        Hi {userName},
                        </br></br>
                        {assigningMessage}
                        </br></br>
                        Here's the deal:
                        </br></br>
                        {dealHeader}
                        </br></br>
                        {dealItems}
                        </br></br>
                        {dealNotes}
                        </br></br>
                        You can take action on it:
                        </br></br>
                        {actionLinks}
                        </br></br>
                        Best Regards,
                        </br></br>
                        WorkflowSystem
                    </html>
                ";

                html = html.Replace("{userName}", user.userName);
                html = html.Replace("{assigningMessage}", assigningMessage);
                html = html.Replace("{actionLinks}", actionLinks);
                html = html.Replace("{dealHeader}", dealHeader);
                html = html.Replace("{dealItems}", dealItems);
                html = html.Replace("{dealNotes}", dealNotes);

                (var success, var message) = await _emailService.SendEmail(
                    from: _configuration["SenderEmailAccount"],
                    to: user.email,
                    subject: subject,
                    content: html
                    );

                if (!success)
                    response.AddWarningMessage("E-mail wasn't sent succesfully. " + message);
            }
        }

        private async Task<DealWorkflowStatus> StartWorkflowAction(DealDto deal, Deal entity, DealWorkflowStatus nextStatus, int userId)
        {
            // if the user started a workflow action
            //if (!deal.CompletedActionId.HasValue && Updatable.IsUpdated(deal.OngoingWorkflowActionId))
            if (Updatable.IsUpdated(deal.OngoingWorkflowActionId))
            {
                var actionId = deal.OngoingWorkflowActionId.Value;

                var previousOngoingWorkflowActionId = await _repo.ProjectedGetById<Deal, int?>(entity.Id, d => d.OngoingWorkflowActionId);

                if (previousOngoingWorkflowActionId.HasValue)
                {
                    // if the deal already had an ongoing action but the user changed the action
                    if (actionId != previousOngoingWorkflowActionId)
                    {
                        var nextStatusCancelled = entity.DealWorkflowStatuses.First(ws => ws.Id == entity.NextDealWorkflowStatusId);
                        _repo.Remove(nextStatusCancelled);
                        entity.NextDealWorkflowStatusId = null;
                    }
                }

                if (actionId.HasValue)
                {
                    // gets the workflow action/checks configuration from the db
                    var workflowAction = await _repo
                        .Get<WorkflowAction>(wa => wa.Id == actionId)
                        .Select(wa => new
                        {
                            wa.TargetWorkflowStatusId,
                            wa.TargetAlternateDescriptionSuffix,
                            wa.CantBePerformedBySameUser,
                            wa.Name,
                            Tasks = wa.Tasks
                                .Where(c => c.Active
                                && (!c.DealTypesInWorkflowTask.Any() || c.DealTypesInWorkflowTask.Any(dt => dt.DealTypeId == entity.DealTypeId)))
                                .Select(c => new
                                {
                                    c.Id,
                                    c.Description,
                                    c.Type,
                                    c.DependingUponAnswerId,
                                })
                        })
                        .FirstOrDefaultAsync();

                    if (workflowAction == null)
                    {
                        throw new Exception($"Action Id {actionId} is not valid among the possible workflow actions of the deal at the moment.");
                    }

                    if (workflowAction.CantBePerformedBySameUser)
                    {
                        var otherWorkflowActionIds = entity.DealWorkflowStatuses
                            .Where(dws => dws.PrecedingWorkflowActionId != actionId && dws.PrecedingWorkflowActionId.HasValue && dws.InitiatedByUserId == userId)
                            .Select(dws => dws.PrecedingWorkflowActionId.Value);

                        var otherActionName = (await _repo.ProjectedListBuilder((WorkflowAction action) => action.Name)
                            .WhereEntity(action => action.CantBePerformedBySameUser && otherWorkflowActionIds.Contains(action.Id))
                            .FirstOrDefaultAsync());

                        //if (otherActionName != null)
                        //throw new BusinessRuleException($"The '{workflowAction.Name}' action and '{otherActionName}' action can't be both performed by the same user.");
                    }


                    var workflowStatusId = workflowAction.TargetWorkflowStatusId;

                    // gets the workflow status configuration
                    var workflowStatus = await _repo
                        .Get<WorkflowStatus>(ws => ws.Id == workflowStatusId)
                        .Select(ws => new
                        {
                            ws.WorkflowSetId,
                            ws.Name,
                            ws.AssignmentType,
                            ws.WorkflowRoleId,
                            ws.FinalizeDeal,
                        })
                        .FirstAsync();

                    // builds the new status into the deal
                    nextStatus = CreateDealWorkflowStatus(
                        workflowStatusId: workflowStatusId,
                        workflowStatusName: workflowStatus.Name,
                        nameSuffix: workflowAction.TargetAlternateDescriptionSuffix,
                        finalizeDeal: workflowStatus.FinalizeDeal
                    );

                    switch (workflowStatus.AssignmentType)
                    {
                        case WorkflowAssignmentTypeEnum.DealTrader:
                            // if the status is to be assigned to the deal trader, we get the last status that was assignable to a trader, and consider it as the official trader
                            var lastStatus = deal.DealWorkflowStatuses.LastOrDefault(ws => ws.AssignmentType == WorkflowAssignmentTypeEnum.DealTrader);
                            if (lastStatus != null)
                                nextStatus.AssigneeUserId = lastStatus.AssigneeUserId;
                            else
                                nextStatus.AssigneeUserId = userId;
                            break;
                        case WorkflowAssignmentTypeEnum.PredefinedApprovalLevel:
                            nextStatus.AssigneeWorkflowRoleId = workflowStatus.WorkflowRoleId;
                            break;
                    }

                    // adds the checks to the deal status
                    foreach (var task in workflowAction.Tasks)
                    {
                        // if this tasks depends on an answer given in a previous status
                        if (task.DependingUponAnswerId.HasValue)
                            // and the previous statuses do not have that answer, then this task will be ignored.
                            if (!entity.DealWorkflowStatuses.Any(s => s.Tasks.Any(t => t.WorkflowTaskAnswerId == task.DependingUponAnswerId)))
                                continue;
                        var dealTask = new DealWorkflowTask
                        {
                            WorkflowTaskId = task.Id,
                            WorkflowTaskDescription = task.Description,
                            Done = false,
                        };
                        if (!string.IsNullOrWhiteSpace(deal.OngoingWorkflowActionNote) &&
                            task.Type == WorkflowTaskTypeEnum.EnterTextInformation)
                        {
                            dealTask.TextInformation = deal.OngoingWorkflowActionNote;
                        }
                        nextStatus.Tasks.Add(dealTask);
                    }

                    entity.DealWorkflowStatuses.Add(nextStatus);
                }
            }

            return nextStatus;
        }

        DealWorkflowStatus CreateDealWorkflowStatus(
            int workflowStatusId,
            string workflowStatusName,
            string nameSuffix = null,
            bool finalizeDeal = false,
            bool revertedBack = false,
            int? assigneeUserId = null,
            int? initiatedByUserId = null,
            DateTimeOffset? dateTimeConfirmed = null)
        {
            return new DealWorkflowStatus
            {
                WorkflowStatusId = workflowStatusId,
                WorkflowStatusName = workflowStatusName +
                            (nameSuffix != null
                            ? $" ({nameSuffix})"
                            : ""),
                Finalized = finalizeDeal,
                RevertedBackByUser = revertedBack,
                AssigneeUserId = assigneeUserId,
                InitiatedByUserId = initiatedByUserId,
                DateTimeConfirmed = dateTimeConfirmed,
            };
        }

        internal async Task SetPossibleAssignmentsForApproval(DealDto deal, DealService dealService)
        {
            if (deal.NextDealWorkflowStatusId.HasValue)
            {
                var nextStatus = deal.DealWorkflowStatuses.First(s => s.Id == deal.NextDealWorkflowStatusId);
                if (nextStatus.AssignmentType == WorkflowAssignmentTypeEnum.ApprovalLevelSelectionEqualHigher)
                {
                    var traderUserId = deal.DealWorkflowStatuses.Last(ws => ws.AssignmentType == WorkflowAssignmentTypeEnum.DealTrader).AssigneeUserId;
                    await SetPossibleAssignments(deal, dealService, traderUserId);
                }
            }
        }

        public async Task<List<DealWorkflowAssignmentDto>> GetTradePolicyEvaluation(int dealId, int userId, DealService dealService)
        {
            var workflowRoleId = await _userService.GetUserHighestApprovalWorkflowRoleId(userId);
            var deal = await dealService.Get(dealId);
            await SetPossibleAssignments(deal, dealService);

            foreach (var pa in deal.PossibleAssignments)
            {
                if (pa.WorkflowRoleId == workflowRoleId)
                {
                    pa.IsTraderWorkflowLevel = true;
                    break;
                }
            }

            return deal.PossibleAssignments;
        }


        private async Task SetPossibleAssignments(DealDto deal, DealService dealService, int? traderUserId = null)
        {
            var builder = _repo.ProjectedListBuilder(DealWorkflowAssignmentDto.ProjectionFromEntity);
            builder.Where(wa => wa.ApprovalLevel.HasValue);
            builder.OrderBy(wa => wa.ApprovalLevel);
            deal.PossibleAssignments = await builder.ExecuteAsync();

            int? traderLevel = null;
            if (traderUserId.HasValue)
            {
                traderLevel = (await _repo.ProjectedListBuilder<User, int?>(entity =>
                entity.WorkflowRolesInUser.Where(wru => wru.Active).Min(wu => wu.WorkflowRole.ApprovalLevel))
                    .WhereEntity(entity => entity.Id == traderUserId).FirstOrDefaultAsync());
            }

            deal.PossibleAssignments.ForEach(wa => wa.EnabledForSelection = (!traderUserId.HasValue || !traderLevel.HasValue || wa.ApprovalLevel >= traderLevel));

            await PickAssignmentBasedOnTradingPolicy(deal, dealService);
        }

        internal async Task PickAssignmentBasedOnTradingPolicy(DealDto deal, DealService dealService)
        {
            var policyCriteria = await _repo.ProjectedListBuilder((TraderAuthorityPolicyCriteria policyCriteria) => new TraderAuthorityPolicyCriteria
            {
                WorkflowRoleId = policyCriteria.WorkflowRoleId,
                OnlyBuy = policyCriteria.OnlyBuy,
                OnlySell = policyCriteria.OnlySell,
                MaxBuyVolume = policyCriteria.MaxBuyVolume,
                MaxSellVolume = policyCriteria.MaxSellVolume,
                MaxVolume = policyCriteria.MaxVolume,
                MaxVolumeForecastPercentage = policyCriteria.MaxVolumeForecastPercentage,
                MaxAcquisitionCost = policyCriteria.MaxAcquisitionCost,
                MaxSellAcquisitionCost = policyCriteria.MaxSellAcquisitionCost,
                MaxTermInMonths = policyCriteria.MaxTermInMonths,
                MaxDurationInMonths = policyCriteria.MaxDurationInMonths,
                TraderAuthorityPolicy = new TraderAuthorityPolicy { Name = policyCriteria.TraderAuthorityPolicy.Name },
            })
                .Where(pc => pc.Active && pc.TraderAuthorityPolicy.DealTypes.Any(dt => dt.Id == deal.DealTypeId.Value))
                .OrderBy(pc => pc.WorkflowRole.ApprovalLevel)
                .ExecuteAsync();

            var dealDataPolicy = new DealDataPolicyAssessment(deal, _repo, dealService);
            foreach (var criteria in policyCriteria)
            {
                var possibleAssignment = deal.PossibleAssignments.FirstOrDefault(pa => pa.WorkflowRoleId == criteria.WorkflowRoleId);
                if (possibleAssignment == null || !possibleAssignment.EnabledForSelection)
                    continue;

                if (await ApplyTradingPolicyToPossibleAssignment(dealDataPolicy, criteria, possibleAssignment))
                    possibleAssignment.MeetsTradingPolicy = true;
            }
        }

        static async Task<bool> ApplyTradingPolicyToPossibleAssignment(DealDataPolicyAssessment dealDataPolicy, TraderAuthorityPolicyCriteria criteria, DealWorkflowAssignmentDto possibleAssignment)
        {
            var yeah = true;
            bool criteriaMet;
            var assessment = possibleAssignment.AddAssessment();

            if (criteria.OnlyBuy)
            {
                var onlyBuy = await dealDataPolicy.HasOnlyBuyItems();
                criteriaMet = onlyBuy;
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Contains only Buy deal items?", criteriaMet, "Yes", (onlyBuy ? "Yes" : "No"));
            }

            if (criteria.OnlySell)
            {
                var onlySell = await dealDataPolicy.HasOnlySellItems();
                criteriaMet = onlySell;
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Contains only Sell deal items?", criteriaMet, "Yes", (onlySell ? "Yes" : "No"));
            }


            if (criteria.MaxBuyVolume.HasValue)
            {
                var dealMax = await dealDataPolicy.GetVolume(PositionEnum.Buy);
                criteriaMet = (dealMax <= criteria.MaxBuyVolume.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Volume (Buy)", criteriaMet, criteria.MaxBuyVolume.Value, dealMax,
                    "This policy determines a ceiling for the highest volume in any point in time in the buy deal items' date range. " +
                    "Volume is accumulated on the coinciding dates and trading periods. " +
                    "It also gathers data from other deals from the same trader / deal category / deal type / counterparty with coinciding item dates.", dealDataPolicy.GetUnitOfMeasure());
            }

            if (criteria.MaxSellVolume.HasValue)
            {
                var dealMax = await dealDataPolicy.GetVolume(PositionEnum.Sell);
                criteriaMet = (dealMax <= criteria.MaxSellVolume.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Volume (Sell)", criteriaMet, criteria.MaxSellVolume.Value, dealMax,
                    "This policy determines a ceiling for the highest volume in any point in time in the sell deal items' date range. " +
                    "Volume is accumulated on the coinciding dates and trading periods. " +
                    "It also gathers data from other deals from the same trader / deal category / deal type / counterparty with coinciding item dates.", dealDataPolicy.GetUnitOfMeasure());
            }

            if (criteria.MaxVolume.HasValue)
            {
                var dealMax = await dealDataPolicy.GetVolume();
                criteriaMet = (dealMax <= criteria.MaxVolume.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Volume", criteriaMet, criteria.MaxVolume.Value, dealMax,
                    "This policy determines a ceiling for the highest volume in any point in time in the deal items' date range. " +
                    "Volume is accumulated on the coinciding dates and trading periods. " +
                    "It also gathers data from other deals from the same trader / deal category / deal type / counterparty with coinciding item dates.", dealDataPolicy.GetUnitOfMeasure());
            }

            if (criteria.MaxVolumeForecastPercentage.HasValue)
            {
                var dealMax = await dealDataPolicy.GetVolumeForecastPercentage();
                criteriaMet = (dealMax <= criteria.MaxVolumeForecastPercentage.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Volume % of Sales Forecast", criteriaMet,
                    (criteria.MaxVolumeForecastPercentage.Value * 100).ToString("N2") + "%", (dealMax * 100).ToString("N2") + "%",
                    "This policy determines a ceiling for the highest volume percentage in relation to each possible monthly forecast. " +
                    "The volume considered is the highest volume in any point in time in the deal items' date range" +
                    "Volume is accumulated on the coinciding dates and trading periods. " +
                    "It also gathers data from other deals from the same trader / deal category / deal type / counterparty with coinciding item dates.");
            }

            if (criteria.MaxAcquisitionCost.HasValue)
            {
                var dealMax = await dealDataPolicy.GetAcquisitionCost();
                criteriaMet = (dealMax <= criteria.MaxAcquisitionCost.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Acquisition Cost", criteriaMet, criteria.MaxAcquisitionCost.Value, dealMax,
                    "This policy determines a ceiling for the sum of all deal items' acquisition costs, which is calculated by the following formula: " +
                    "power * (amount_of_half_hours> / 2) * days * price", currency: true);
            }

            if (criteria.MaxSellAcquisitionCost.HasValue)
            {
                var dealMax = await dealDataPolicy.GetAcquisitionCost(PositionEnum.Sell);
                criteriaMet = (dealMax <= criteria.MaxSellAcquisitionCost.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Acquisition Cost (Sell)", criteriaMet, criteria.MaxSellAcquisitionCost.Value, dealMax,
                    "This policy determines a ceiling for the sum of all sell deal items' acquisition costs, which is calculated by the following formula: " +
                    "power * (amount_of_half_hours> / 2) * days * price", currency: true);
            }

            if (criteria.MaxTermInMonths.HasValue)
            {
                var dealMax = await dealDataPolicy.GetTermInMonthsWithDaysOffset();
                criteriaMet = (dealMax <= criteria.MaxTermInMonths.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Term", criteriaMet, $"{criteria.MaxTermInMonths.Value} months", $"{dealMax}",
                    "This policy determines the maximum possible term, which is the sum of all item date ranges.").IsTermCriteria = true;
            }

            if (criteria.MaxDurationInMonths.HasValue)
            {
                var dealMax = await dealDataPolicy.GetDurationInMonthsWithDaysOffset();
                criteriaMet = (dealMax <= criteria.MaxDurationInMonths.Value);
                if (!criteriaMet)
                    yeah = false;

                assessment.AddAssessmentRow("Max. Duration", criteriaMet, $"{criteria.MaxDurationInMonths.Value} months", $"{dealMax}",
                    "This policy determines the maximum possible duration, " +
                    "which is the duration between the submission date and the most future end date of the deal items.");
            }

            assessment.PolicyMet = yeah;
            return yeah;
        }

        private async Task<bool> ConfirmWorkflowAction(int? completedActionId, Deal entity, int userId)
        {
            // if the user completed a workflow action
            if (completedActionId.HasValue)
            {
                if (completedActionId != entity.OngoingWorkflowActionId)
                    throw new BusinessRuleException("The action cannot be completed as the deal is outdated in comparison to the database. Please reopen the deal and try again.");

                var status = entity.DealWorkflowStatuses.First(dws => dws.Id == entity.NextDealWorkflowStatusId);
                status.DateTimeConfirmed = DateUtils.GetDateTimeOffsetNow();
                status.InitiatedByUserId = userId;
                status.PrecedingWorkflowActionId = entity.OngoingWorkflowActionId;

                // removes tasks that were dependent on preceding answers that were not given
                foreach (var task in status.Tasks.Where(t => t.WorkflowTask.PrecedingAnswerId.HasValue))
                    if (!status.Tasks.Any(t => t.WorkflowTaskAnswerId == task.WorkflowTask.DependingUponAnswerId))
                        _repo.Remove(task);

                // moves the deal to the next status
                entity.PreviousDealWorkflowStatusId = entity.CurrentDealWorkflowStatusId;
                entity.CurrentDealWorkflowStatusId = entity.NextDealWorkflowStatusId;
                entity.NextDealWorkflowStatusId = null;
                entity.OngoingWorkflowActionId = null;

                if (await _repo.ProjectedGetById(status.PrecedingWorkflowActionId, (WorkflowAction a) => a.PerformsExecutionAutomatically))
                {
                    entity.Executed = true;
                    entity.ExecutionUserId = userId;
                    entity.ExecutionDate = DateUtils.GetDateTimeOffsetNow();
                }

                LockSubEntities(entity);

                RemoveWorkflowActionListeners(entity.Id);
                if (await _repo.ProjectedGetById(status.PrecedingWorkflowActionId, (WorkflowAction a) => a.IsSubmission))
                {
                    entity.SubmissionDate = DateUtils.GetDateTimeOffsetNow();
                    entity.SubmissionUserId = userId;
                }

                return true;
            }

            return false;
        }

        void LockSubEntities(Deal entity)
        {
            foreach (var note in entity.Notes.Where(n => !n.IsLocked))
                note.IsLocked = true;

            _repo.Context.DealAttachmentVersions.Where(v => v.DealAttachment.DealId == entity.Id && !v.IsLocked)
                .ForEach(v => v.IsLocked = true);

            entity.Attachments.ForEach(a => a.Versions.Where(n => n.Id == 0 && !n.IsLocked).ForEach(v => v.IsLocked = true));
        }

        private async Task<DealWorkflowStatus> InitializeWorkflowStatus(Deal entity, int userId, DealWorkflowStatus currentStatus)
        {
            // initializes the status
            if (!entity.DealWorkflowStatuses.Any())
            {
                if (entity.WorkflowSetId == null || !entity.WorkflowSetId.HasValue || entity.WorkflowSetId.Value == 0)
                    throw new Exception("Internal exception: entity.WorkflowSetId should be set at this point.");

                var current = await GetCurrentWorkflowStatusConfig(entity.WorkflowSetId.Value);

                currentStatus = CreateDealWorkflowStatus(
                    workflowStatusId: current.Id,
                    workflowStatusName: current.Name,
                    assigneeUserId: userId,
                    initiatedByUserId: userId,
                    dateTimeConfirmed: DateUtils.GetDateTimeOffsetNow()
                    );

                entity.DealWorkflowStatuses.Add(currentStatus);
            }

            return currentStatus;
        }
    }
}