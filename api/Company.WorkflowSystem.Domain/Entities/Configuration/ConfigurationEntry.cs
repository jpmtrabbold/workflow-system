using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities.Configuration
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
