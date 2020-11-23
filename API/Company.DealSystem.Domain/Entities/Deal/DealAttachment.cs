using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class DealAttachment : BaseEntity
    {
        public Deal Deal { get; set; }
        public int DealId { get; set; }

        public AttachmentType AttachmentType { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string AttachmentTypeOtherText { get; set; }
        
        /// <summary>
        /// this field is to be used when the attachment is linked by another functionality.
        /// </summary>
        public DealAttachmentLinkType? LinkType { get; set; }

        /// <summary>
        /// All the versions of this attachment. Every attachment has at least one version.
        /// </summary>
        public ICollection<DealAttachmentVersion> Versions { get; private set; } = new List<DealAttachmentVersion>();
    }
}
