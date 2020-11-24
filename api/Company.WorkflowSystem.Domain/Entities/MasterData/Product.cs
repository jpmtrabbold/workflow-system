using System.Collections.Generic;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class Product : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public int DealCategoryId { get; set; }
        public DealCategory DealCategory { get; set; }
    }
}