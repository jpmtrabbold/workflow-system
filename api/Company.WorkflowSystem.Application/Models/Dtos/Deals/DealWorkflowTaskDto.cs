using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Linq;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Application.Models.Dtos.Deals
{
    public class DealWorkflowTaskDto : UpdatableListItemDto, IPersistableDto<DealWorkflowTask, DealService>
    {
        public int? PrecedingAnswerId { get; set; }
        public int WorkflowTaskId { get; set; }
        public Updatable<string> WorkflowTaskDescription { get; set; }
        public WorkflowTaskTypeEnum Type { get; set; }
        public bool Mandatory { get; set; }
        public Updatable<int?> WorkflowTaskAnswerId { get; set; }
        public List<WorkflowTaskAnswerReadDto> PossibleAnswers { get; set; }
        public Updatable<string> WorkflowTaskAnswerText { get; set; }
        public Updatable<string> TextInformation { get; set; }
        public Updatable<DateTimeOffset?> DateInformation { get; set; }
        public Updatable<decimal?> NumberInformation { get; set; }
        public Updatable<bool> Done { get; set; }

        internal static Expression<Func<DealWorkflowTask, DealWorkflowTaskDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealWorkflowTaskDto()
                {
                    Id = entity.Id,
                    PrecedingAnswerId = entity.WorkflowTask.PrecedingAnswerId,
                    WorkflowTaskId = entity.WorkflowTaskId,
                    WorkflowTaskDescription = Updatable.Create(entity.WorkflowTaskDescription),
                    Type = entity.WorkflowTask.Type,
                    Mandatory = entity.WorkflowTask.Mandatory,
                    WorkflowTaskAnswerId = Updatable.Create(entity.WorkflowTaskAnswerId),
                    PossibleAnswers = entity.WorkflowTask.PossibleAnswers.AsQueryable().Select(WorkflowTaskAnswerReadDto.ProjectionFromEntity).ToList(),
                    WorkflowTaskAnswerText = Updatable.Create(entity.WorkflowTaskAnswerText),
                    TextInformation = Updatable.Create(entity.TextInformation),
                    DateInformation = Updatable.Create(entity.DateInformation),
                    NumberInformation = Updatable.Create(entity.NumberInformation),
                    Done = Updatable.Create(entity.Done),
                };
            }
        }

        public DealWorkflowTask ToEntity(DealWorkflowTask entity, DealService service)
        {
            if (entity == null)
            {
                entity = new DealWorkflowTask
                {
                    WorkflowTaskId = WorkflowTaskId
                };
            }

            if (Updatable.IsUpdated(WorkflowTaskDescription))
                entity.WorkflowTaskDescription = WorkflowTaskDescription.Value;

            if (Updatable.IsUpdated(WorkflowTaskAnswerId))
                entity.WorkflowTaskAnswerId = WorkflowTaskAnswerId.Value;

            if (Updatable.IsUpdated(WorkflowTaskAnswerText))
                entity.WorkflowTaskAnswerText = WorkflowTaskAnswerText.Value;

            if (Updatable.IsUpdated(TextInformation))
                entity.TextInformation = TextInformation.Value;

            if (Updatable.IsUpdated(DateInformation))
                entity.DateInformation = DateInformation.Value;

            if (Updatable.IsUpdated(NumberInformation))
                entity.NumberInformation = NumberInformation.Value;

            if (Updatable.IsUpdated(Done))
                entity.Done = Done.Value;

            return entity;
        }
    }
}