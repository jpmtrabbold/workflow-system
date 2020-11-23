using System.Collections.Generic;

namespace Company.DealSystem.Domain.Entities
{
    public class TraderAuthorityPolicy : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Deal types that use this policy to determine their approvers
        /// </summary>
        public ICollection<DealType> DealTypes { get; private set; } = new List<DealType>();
        /// <summary>
        /// Collection of criterias for this policy
        /// </summary>
        public ICollection<TraderAuthorityPolicyCriteria> Criteria { get; private set; } = new List<TraderAuthorityPolicyCriteria>();
        
    }
}