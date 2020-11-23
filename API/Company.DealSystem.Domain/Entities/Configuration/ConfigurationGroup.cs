using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Domain.Enum;

namespace Company.DealSystem.Domain.Entities.Configuration
{
    public class ConfigurationGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConfigurationGroupIdentifiersEnum Identifier { get; set; }
        public List<ConfigurationEntry> Entries { get; set; }
    }
}
