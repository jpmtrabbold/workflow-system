using System;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Deals
{
    public class DealsListRequest : ListRequest
    {
        public int? DealId { get; set; }
        public bool OnlyDealsAssignedToMeOrMyRole { get; set; } = false;
        public bool IncludeFinalizedDeals { get; set; } = false;
        /// <summary>
        /// start entered date filter - time will be ignored
        /// </summary>
        public DateTimeOffset? StartCreationDate { get; set; }
        /// <summary>
        /// end entered date filter - time will be ignored
        /// </summary>
        public DateTimeOffset? EndCreationDate { get; set; }
    }
}