using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class UserIntegrationData : DeactivatableBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public IntegrationTypeEnum IntegrationType { get; set; }
        public UserIntegrationFieldEnum Field { get; set; }
        public string Data { get; set; }
    }
}