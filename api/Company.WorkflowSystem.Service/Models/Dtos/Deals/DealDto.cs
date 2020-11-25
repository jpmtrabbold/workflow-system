using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Helpers;
using System.Collections.Generic;
using System.Linq;
using Company.WorkflowSystem.Service.Interfaces;
using LinqKit;
using Company.WorkflowSystem.Service.Exceptions;
using Company.WorkflowSystem.Service.DataAggregators;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealDto : IPersistableDto<Deal, DealService>
    {
        public DealDto()
        {

        }

        public DealDto(int? dealCategoryId, int? dealTypeId, int? counterpartyId, int? workflowSetId, int? itemFieldsetId)
        {
            DealCategoryId = Updatable.Create(dealCategoryId, true);
            DealTypeId = Updatable.Create(dealTypeId, true);
            CounterpartyId = Updatable.Create(counterpartyId, true);
            WorkflowSetId = Updatable.Create(workflowSetId, true);
            DealItemFieldsetId = Updatable.Create(itemFieldsetId, true);

            Items.State = LazyLoadedDataStateEnum.Ready;
            Notes.State = LazyLoadedDataStateEnum.Ready;
            Attachments.State = LazyLoadedDataStateEnum.Ready;
        }

        public int? Id { get; set; }

        public string DealNumber { get; set; }
        public string DealStatusName { get; set; }
        public bool AssignedToSelf { get; set; }
        public bool UserParticipatedOnThisDeal { get; set; }
        public int CurrentUserId { get; set; }

        public Updatable<int?> DealCategoryId { get; set; }
        public Updatable<int?> DealTypeId { get; set; }
        public Updatable<int?> CounterpartyId { get; set; }
        public string CounterpartyName { get; set; }
        public Updatable<bool> ForceMajeure { get; set; }
        public Updatable<DateTimeOffset?> ExpiryDate { get; set; }
        public Updatable<int?> DealItemFieldsetId { get; set; }

        public Updatable<int?> WorkflowSetId { get; set; }
        public int? CurrentDealWorkflowStatusId { get; set; }
        public int? NextDealWorkflowStatusId { get; set; }

        public Updatable<int?> TermInMonthsOverride { get; set; }

        public Updatable<int?> OngoingWorkflowActionId { get; set; }
        public string OngoingWorkflowActionNote { get; set; }
        public int? CompletedActionId { get; set; }
        public bool Executed { get; set; }
        public Updatable<DateTimeOffset?> ExecutionDate { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        /// <summary>
        /// whether the deal is in a status that it can be executed or have its execution cancelled
        /// </summary>
        public bool IsExecutionStatus { get; set; }
        public DateTimeOffset? SubmissionDate { get; set; }
        public int? SubmissionUserId { get; set; }
        public Updatable<int?> DelegatedAuthorityUserId { get; set; }
        public string DelegatedAuthorityUserName { get; set; }
        public List<DealWorkflowStatusDto> DealWorkflowStatuses { get; set; } = new List<DealWorkflowStatusDto>();

        public List<DealWorkflowAssignmentDto> PossibleAssignments { get; set; }
        

        public LazyLoadedData<List<DealItemDto>> Items { get; set; } = new LazyLoadedData<List<DealItemDto>>();
        public LazyLoadedData<List<DealNoteDto>> Notes { get; set; } = new LazyLoadedData<List<DealNoteDto>>();
        public LazyLoadedData<List<DealAttachmentDto>> Attachments { get; set; } = new LazyLoadedData<List<DealAttachmentDto>>();

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<Deal, DealDto>> ProjectionFromEntity(int userId, bool light)
        {
            return entity => new DealDto()
            {
                Id = entity.Id,
                DealNumber = entity.DealNumber,
                DealStatusName = (entity.CurrentDealWorkflowStatus != null ? entity.CurrentDealWorkflowStatus.WorkflowStatus.Name : null),
                AssignedToSelf = Deal.AssignedToSpecificUser.Invoke(entity, userId),
                CurrentUserId = userId,
                UserParticipatedOnThisDeal = Deal.UserParticipatedOnThisDeal.Invoke(entity, userId),
                CounterpartyId = Updatable.CreateNullable(entity.CounterpartyId),
                CounterpartyName = entity.Counterparty.Name,
                DealCategoryId = Updatable.CreateNullable(entity.DealCategoryId),
                DealTypeId = Updatable.CreateNullable(entity.DealTypeId),
                ForceMajeure = Updatable.Create(entity.ForceMajeure),
                ExpiryDate = Updatable.Create(entity.ExpiryDate),
                DealItemFieldsetId = Updatable.Create(entity.DealItemFieldsetId),
                ExecutionDate = Updatable.Create(entity.ExecutionDate),
                CreationDate = entity.CreationDate,
                Executed = entity.Executed,
                IsExecutionStatus = (entity.CurrentDealWorkflowStatus != null ? entity.CurrentDealWorkflowStatus.WorkflowStatus.AllowsDealExecution : false),
                WorkflowSetId = Updatable.Create(entity.WorkflowSetId),
                CurrentDealWorkflowStatusId = entity.CurrentDealWorkflowStatusId,
                OngoingWorkflowActionId = Updatable.Create(entity.OngoingWorkflowActionId),
                NextDealWorkflowStatusId = entity.NextDealWorkflowStatusId,
                DelegatedAuthorityUserId = Updatable.Create(entity.DelegatedAuthorityUserId),
                DelegatedAuthorityUserName = entity.DelegatedAuthorityUser != null ? entity.DelegatedAuthorityUser.Name : null,
                DealWorkflowStatuses = entity.DealWorkflowStatuses
                    .AsQueryable()
                    .Select(DealWorkflowStatusDto.ProjectionFromEntity)
                    .OrderBy(s => s.DateTimeCreated)
                    .ToList(),
                SubmissionDate = entity.SubmissionDate,
                SubmissionUserId = entity.SubmissionUserId,
                TermInMonthsOverride = Updatable.Create(entity.TermInMonthsOverride),
            };
        }

        /// <summary>
        /// creates or updates entity based on current dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Deal ToEntity(Deal entity, DealService service)
        {
            if (entity == null)
            {
                entity = new Deal
                {
                    DealNumber = DealNumber,
                    CreationUserId = service.GetUserId(),
                };
            }

            if (Updatable.IsUpdated(DealCategoryId))
                entity.DealCategoryId = DealCategoryId.Value.Value;

            if (Updatable.IsUpdated(DealTypeId))
                entity.DealTypeId = DealTypeId.Value.Value;

            if (Updatable.IsUpdated(CounterpartyId))
                entity.CounterpartyId = CounterpartyId.Value.Value;

            if (Updatable.IsUpdated(DealItemFieldsetId))
                entity.DealItemFieldsetId = DealItemFieldsetId.Value;

            if (Updatable.IsUpdated(ForceMajeure))
                entity.ForceMajeure = ForceMajeure.Value;

            if (Updatable.IsUpdated(ExpiryDate))
            {
                if (ExpiryDate.Value.HasValue)
                {
                    entity.ExpiryDate = ExpiryDate.Value.Value.DateWithMinTime();
                }
                else
                {
                    entity.ExpiryDate = null;
                }
            }

            if (Updatable.IsUpdated(TermInMonthsOverride))
                entity.TermInMonthsOverride = TermInMonthsOverride.Value;

            if (Items.State == LazyLoadedDataStateEnum.Ready)
                Updatable.ToEntityCollection(Items.Data, entity.Items, service);

            if (Notes.State == LazyLoadedDataStateEnum.Ready)
                Updatable.ToEntityCollection(Notes.Data, entity.Notes, service);

            if (Updatable.IsUpdated(WorkflowSetId))
                entity.WorkflowSetId = WorkflowSetId.Value;

            if (Updatable.IsUpdated(OngoingWorkflowActionId))
                entity.OngoingWorkflowActionId = OngoingWorkflowActionId.Value;

            if (Updatable.IsUpdated(DelegatedAuthorityUserId))
                entity.DelegatedAuthorityUserId = DelegatedAuthorityUserId.Value;

            Updatable.ToEntityCollection(DealWorkflowStatuses, entity.DealWorkflowStatuses, service);


            if (Updatable.IsUpdated(ExecutionDate))
            {
                if (!ExecutionDate.Value.HasValue)
                    throw new BusinessRuleException("Execution can't be reversed on a save action.");

                if (entity.ExecutionDate.HasValue || entity.Executed)
                    throw new BusinessRuleException("This deal is already executed, so this action cannot be completed.");

                entity.ExecutionDate = ExecutionDate.Value;
                entity.Executed = true;
            }

            if (Attachments.State == LazyLoadedDataStateEnum.Ready)
            {
                foreach (var dtoItem in Attachments.Data)
                {
                    if (!dtoItem.Id.HasValue)
                    {
                        if (!dtoItem.Deleted)
                            entity.Attachments.Add(dtoItem.ToEntity(null, service));
                    }
                    else if (dtoItem.Deleted || dtoItem.Updated)
                    {
                        var dbEntity = service._repo.Context.DealAttachments.FirstOrDefault(e => e.Id == dtoItem.Id.Value);
                        if (dbEntity != null) // ignores if null, because other user might have deleted this entry
                        {
                            if (dtoItem.Deleted)
                                service._repo.Remove(dbEntity);
                            else
                            {
                                service._repo.Context.Entry(dbEntity).Collection(da => da.Versions).Load();
                                dtoItem.ToEntity(dbEntity, service);
                            }
                        }
                    }
                }
            }

            return entity;
        }
    }
}