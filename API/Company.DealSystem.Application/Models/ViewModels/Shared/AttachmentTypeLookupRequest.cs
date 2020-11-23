using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;

namespace Company.DealSystem.Application.Models.ViewModels.Shared
{
    public class AttachmentTypeLookupRequest: LookupRequest
    {
        /// <summary>
        /// whether this attachment type refers to the "Other" type
        /// </summary>
        public bool Other { get; set; }

        internal static Expression<Func<AttachmentType, AttachmentTypeLookupRequest>> ProjectionFromAttachmentType
        {
            get
            {
                return entity => new AttachmentTypeLookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Other = entity.Other,
                };
            }
        }
    }
}
