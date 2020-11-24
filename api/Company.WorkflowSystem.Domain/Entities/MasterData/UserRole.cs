using System.Collections.Generic;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class UserRole : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<FunctionalityInUserRole> FunctionalitiesInUserRole { get; set; } = new List<FunctionalityInUserRole>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}