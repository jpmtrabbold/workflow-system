using System.Collections.Generic;

namespace Company.DealSystem.Domain.Entities
{
    public class Product : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public int DealCategoryId { get; set; }
        public DealCategory DealCategory { get; set; }
    }
}