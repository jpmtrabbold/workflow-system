using System.Collections.Generic;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class WorkflowRole : BaseEntity
    {
        public string Name { get; set; }
        /// <summary>
        /// workflow approval level - the higher "the higher"
        /// </summary>
        public int? ApprovalLevel { get; set; }
        
        public ICollection<UserInWorkflowRole> UsersInWorkflowRole { get; private set; } = new List<UserInWorkflowRole>();
    }
}