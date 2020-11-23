using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using Company.DealSystem.Domain.Models.Enum;
using System.Linq;
using Company.DealSystem.Domain.ExtensionMethods;
using Company.DealSystem.Application.Utils;
using System.Runtime.InteropServices.ComTypes;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Deals
{
    public class DealNoteDto : UpdatableListItemDto, IPersistableDto<DealNote, DealService>
    {
        public int? DealId { get; set; }
        public int NoteCreatorId { get; set; }
        public string NoteCreatorName { get; set; }
        public Updatable<string> NoteContent { get; set; }
        public Updatable<DateTimeOffset> CreatedDate { get; set; }

        public bool IsLocked { get; set; }
        public Updatable<NoteReminderTypeEnum?> ReminderType { get; set; }
        public Updatable<string> ReminderEmailAccounts { get; set; }
        public Updatable<DateTimeOffset?> ReminderDateTime { get; set; }

        internal static Expression<Func<DealNote, DealNoteDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealNoteDto()
                {
                    Id = entity.Id,
                    DealId = entity.DealId,
                    NoteCreatorId = entity.CreationUserId,
                    NoteCreatorName = entity.CreationUser.Name,
                    IsLocked = entity.IsLocked,
                    NoteContent = Updatable.Create(entity.Content),
                    CreatedDate = Updatable.Create(entity.CreatedDate),
                    ReminderType = Updatable.Create(entity.ReminderType),
                    ReminderEmailAccounts = Updatable.Create(entity.ReminderEmailAccounts),
                    ReminderDateTime = Updatable.Create(entity.ReminderDateTime),
                };
            }
        }

        static Func<DealNote, DealNoteDto> compiledProjection { get; set; } = ProjectionFromEntity.Compile();
        internal static DealNoteDto FromEntity(DealNote entity)
        {
            return compiledProjection.Invoke(entity);
        }

        public DealNote ToEntity(DealNote entity, DealService service)
        {
            if (entity == null)
            {
                entity = new DealNote
                {
                    
                };
            }

            if (Updatable.IsUpdated(CreatedDate))
                entity.CreatedDate = CreatedDate.Value;

            if (Updatable.IsUpdated(NoteContent))
                entity.Content = NoteContent.Value;

            if (Updatable.IsUpdated(ReminderDateTime))
                entity.ReminderDateTime = ReminderDateTime.Value;

            if (Updatable.IsUpdated(ReminderEmailAccounts))
                entity.ReminderEmailAccounts = ReminderEmailAccounts.Value;


            if (Updatable.IsUpdated(ReminderType))
            {
                switch (ReminderType.Value)
                {
                    case NoteReminderTypeEnum.Me:
                    case NoteReminderTypeEnum.MyRole:
                        // me or my role - needs a userId to know to whom the e-mails will be sent
                        entity.ReminderUserId = service.GetUserId();
                        break;
                    case NoteReminderTypeEnum.Emails:
                        // if the type is just ad hoc e-mails, then the user id is not necessary
                        if (entity.ReminderUserId.HasValue)
                            entity.ReminderUserId = null;
                        break;
                }

                entity.ReminderType = ReminderType.Value;
            }                

            return entity;
        }
    }
}
