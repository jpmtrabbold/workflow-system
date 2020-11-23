using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DealSystem.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}