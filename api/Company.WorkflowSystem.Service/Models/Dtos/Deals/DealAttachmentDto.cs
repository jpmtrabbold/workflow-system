using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Database.Context;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealAttachmentDto : UpdatableListItemDto, IPersistableDto<DealAttachment, DealService>
    {
        public Updatable<int?> AttachmentTypeId { get; set; }
        public Updatable<string> AttachmentTypeOtherText { get; set; }
        /// <summary>
        /// this field is to be used when the attachment is linked by another functionality.
        /// </summary>
        public DealAttachmentLinkType? LinkType { get; set; }
        public List<DealAttachmentVersionDto> Versions { get; set; } = new List<DealAttachmentVersionDto>();

        internal static Expression<Func<DealAttachment, DealAttachmentDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealAttachmentDto()
                {
                    Id = entity.Id,
                    AttachmentTypeId = Updatable.Create(entity.AttachmentTypeId),
                    AttachmentTypeOtherText = Updatable.Create(entity.AttachmentTypeOtherText),
                    LinkType = entity.LinkType,
                    Versions = entity.Versions.AsQueryable().Select(DealAttachmentVersionDto.ProjectionFromEntity).ToList(),
                };
            }
        }

        /// <summary>
        /// creates or updates entity based on current dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DealAttachment ToEntity(DealAttachment entity, DealService service)
        {
            if (entity == null)
                entity = new DealAttachment
                {
                    LinkType = LinkType,
                };

            if (Updatable.IsUpdated(AttachmentTypeId))
                entity.AttachmentTypeId = AttachmentTypeId.Value;

            if (Updatable.IsUpdated(AttachmentTypeOtherText))
                entity.AttachmentTypeOtherText = AttachmentTypeOtherText.Value;

            Updatable.ToEntityCollection(Versions, entity.Versions, service);

            return entity;
        }
    }
}