using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DealSystem.Domain.Entities
{
    public class UserInWorkflowRole : DeactivatableBaseEntity
    {
        [Column(Order = 1)]
        public int UserId { get; set; }
        public User User { get; set; }
        [Column(Order = 2)]
        public int WorkflowRoleId { get; set; }
        public WorkflowRole WorkflowRole { get; set; }
    }
}