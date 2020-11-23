using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities.Configuration
{
    public class ConfigurationEntry : BaseEntity
    {
        public int ConfigurationGroupId { get; set; }
        public ConfigurationGroup ConfigurationGroup { get; set; }
        public string Name { get; set; }
        public ConfigurationIdentifiersEnum Identifier { get; set; }
        public string Content { get; set; }
        public ConfigurationEntryContentType ContentType { get; set; }
        public FunctionalityEnum? FunctionalityForLookup { get; set; }
    }
}
