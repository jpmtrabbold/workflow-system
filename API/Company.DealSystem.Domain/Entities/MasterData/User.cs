using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DealSystem.Domain.Entities
{
    public class User : DeactivatableBaseEntity
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public int? UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        public ICollection<UserInWorkflowRole> WorkflowRolesInUser { get; private set; } = new List<UserInWorkflowRole>();
        public ICollection<Deal> Deals { get; private set; } = new List<Deal>();
        public ICollection<Deal> DealsExecutedByUser { get; private set; } = new List<Deal>();
        public ICollection<Deal> DealsSubmittedByUser { get; private set; } = new List<Deal>();
        public ICollection<UserIntegrationData> IntegrationData { get; private set; } = new List<UserIntegrationData>();
    }
}