using System.Collections.Generic;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class Functionality : BaseEntity
    {
        public string Name { get; set; }
        public FunctionalityEnum FunctionalityEnum { get; set; }

        public ICollection<FunctionalityInUserRole> FunctionalitiesInUserRole { get; set; } = new List<FunctionalityInUserRole>();
    }
}