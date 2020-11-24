using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealAttachmentVersion : BaseEntity
    {
        public DealAttachment DealAttachment { get; set; }
        public int DealAttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] File { get; set; }
        public long FileSizeInBytes { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }
        /// <summary>
        /// This determine whether this version can be deleted/changed. It becomes 'locked' whenever the deal transitions from one state to another
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
