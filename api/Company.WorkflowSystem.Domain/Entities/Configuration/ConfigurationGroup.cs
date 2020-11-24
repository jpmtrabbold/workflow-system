using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Domain.Enum;

namespace Company.WorkflowSystem.Domain.Entities.Configuration
{
    public class ConfigurationGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConfigurationGroupIdentifiersEnum Identifier { get; set; }
        public List<ConfigurationEntry> Entries { get; set; }
    }
}
