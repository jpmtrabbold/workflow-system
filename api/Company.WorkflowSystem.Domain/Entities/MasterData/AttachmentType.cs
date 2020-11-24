using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class AttachmentType : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// whether this attachment type refers to "Other"
        /// </summary>
        public bool Other { get; set; }
    }
}
