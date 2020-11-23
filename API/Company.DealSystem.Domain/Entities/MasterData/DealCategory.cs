using System.Collections.Generic;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class DealCategory : DeactivatableBaseEntity
    {
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }

   
        public ICollection<CounterpartyInDealCategory> CounterpartiesInDealCategory { get; set; } = new List<CounterpartyInDealCategory>();
        public ICollection<DealTypeInDealCategory> DealTypesInDealCategory { get; set; } = new List<DealTypeInDealCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}