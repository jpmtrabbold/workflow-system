using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Domain.Entities.Integrations
{
    public class IntegrationRunEntry : BaseEntity
    {
        public int IntegrationRunId { get; set; }
        [JsonIgnore]
        public IntegrationRun IntegrationRun { get; set; }
        public IntegrationRunEntryTypeEnum Type { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        /// <summary>
        /// json payload
        /// </summary>
        public string Payload { get; set; }
        
        /// <summary>
        /// the id of the entity affected in the system by this integration run entry
        /// </summary>
        public int? AffectedId { get; set; }
        /// <summary>
        /// the functionality of the id (ex: if it's the Deals functionality, then AffectedId will have a Deal's Id)
        /// </summary>
        public FunctionalityEnum? FunctionalityOfAffectedId { get; set; }
        public DateTimeOffset? DateTime { get; set; }
    }
}
