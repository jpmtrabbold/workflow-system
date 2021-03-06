﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowSet : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// all the statuses within this set
        /// </summary>
        public ICollection<WorkflowStatus> Statuses { get; set; } = new List<WorkflowStatus>();
    }
}