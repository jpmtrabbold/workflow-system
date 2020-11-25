using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class Counterparty : DeactivatableBaseEntity
    {
        public Counterparty() : base()
        {
        }
        /// <summary>
        /// Counterparty's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Counterparty's code. It is used to generate the Deal Number prefix
        /// </summary>
        public string Code { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public string CompanyNumber { get; set; }
        public string BusinessNumber { get; set; }
        public bool? NzemParticipant { get; set; }
        public string NzemParticipantId { get; set; }
        public string Conditions { get; set; }
        public decimal ExposureLimit { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public DateTimeOffset? ApprovalDate { get; set; }
        public string SecurityHeld { get; set; }

        /// <summary>
        /// Deal Categories in which this Counterparty can be used
        /// </summary>
        public ICollection<CounterpartyInDealCategory> DealCategories { get; set; } = new List<CounterpartyInDealCategory>();
    }
}