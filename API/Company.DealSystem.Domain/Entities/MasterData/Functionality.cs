using System.Collections.Generic;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class Functionality : BaseEntity
    {
        public string Name { get; set; }
        public FunctionalityEnum FunctionalityEnum { get; set; }

        public ICollection<FunctionalityInUserRole> FunctionalitiesInUserRole { get; set; } = new List<FunctionalityInUserRole>();
    }
}