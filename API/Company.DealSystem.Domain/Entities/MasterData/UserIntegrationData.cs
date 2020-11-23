using System.Collections.Generic;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Domain.Entities
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