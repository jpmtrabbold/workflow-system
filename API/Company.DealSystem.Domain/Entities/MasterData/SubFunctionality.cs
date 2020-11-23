using InversionRepo.Interfaces;
using System.Collections.Generic;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class SubFunctionality : BaseEntity
    {
        public string Name { get; set; }
        public int FunctionalityId { get; set; }
        public Functionality Functionality { get; set; }
        public int? ParentSubFunctionalityId { get; set; }
        public SubFunctionality ParentSubFunctionality { get; set; }
        public SubFunctionalityEnum SubFunctionalityEnum { get; set; }
        public ICollection<SubFunctionalityInUserRole> SubFunctionalitiesInUserRole { get; set; } = new List<SubFunctionalityInUserRole>();
    }
}