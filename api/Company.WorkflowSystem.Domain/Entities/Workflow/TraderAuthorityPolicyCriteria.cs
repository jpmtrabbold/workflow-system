using System.Collections.Generic;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class TraderAuthorityPolicyCriteria : DeactivatableBaseEntity
    {
        public int TraderAuthorityPolicyId { get; set; }
        public TraderAuthorityPolicy TraderAuthorityPolicy { get; set; }
        public int WorkflowRoleId { get; set; }
        public WorkflowRole WorkflowRole { get; set; }
        /// <summary>
        /// this criteria only applies for buying. If it finds a sell dealItem, the criteria is resolved as false
        /// </summary>
        public bool OnlyBuy { get; set; } = false;
        /// <summary>
        /// this criteria only applies for selling. If it finds a buy dealItem, the criteria is resolved as false
        /// </summary>
        public bool OnlySell { get; set; } = false;
        public decimal? MaxBuyVolume { get; set; }
        public decimal? MaxSellVolume { get; set; }
        public decimal? MaxVolume { get; set; }
        public decimal? MaxVolumeForecastPercentage { get; set; }
        /// <summary>
        /// Max Acquisition cost, summing up absolute values for buy and sell deal items. Acquisition cost = power * (amount of half hour periods / 2) * days * price
        /// </summary>
        public decimal? MaxAcquisitionCost { get; set; }
        /// <summary>
        /// Max Acquisition cost for sell deal items. Acquisition cost = power * (amount of half hour periods / 2) * days * price
        /// </summary>
        public decimal? MaxSellAcquisitionCost { get; set; }
        public int? MaxTermInMonths { get; set; }
        public int? MaxDurationInMonths { get; set; }
    }
}