using System.Collections.Generic;

namespace Company.DealSystem.Domain.Entities
{
    public class FunctionalityInUserRole : BaseEntity
    {
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int FunctionalityId { get; set; }
        public Functionality Functionality { get; set; }
        
        public ICollection<SubFunctionalityInUserRole> SubFunctionalitiesInUserRole { get; set; } = new List<SubFunctionalityInUserRole>();
    }
}
