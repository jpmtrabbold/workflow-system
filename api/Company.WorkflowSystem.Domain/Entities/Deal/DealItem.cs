using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealItem : BaseEntity
    {
        /// <summary>
        /// when this is an executed item, this field will have the original dealItem that is being executed
        /// </summary>
        public int? OriginalItemId { get; set; }
        /// <summary>
        /// when this is an executed dealItem, this field will have the original dealItem that is being executed
        /// </summary>
        public DealItem OriginalItem { get; set; }

        public int Order { get; set; }

        public int DealId { get; set; }
        public Deal Deal { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public PositionEnum? Position { get; set; }
        public DayTypeEnum? DayType { get; set; }
        /// <summary>
        /// from which date this dealItem applies
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }
        /// <summary>
        /// from which half hour period this dealItem applies
        /// </summary>
        public int? HalfHourTradingPeriodStart { get; set; }
        /// <summary>
        /// until which date this dealItem applies
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }
        /// <summary>
        /// until which half hour period this dealItem applies
        /// </summary>
        public int? HalfHourTradingPeriodEnd { get; set; }

        public decimal? Quantity { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }

        public decimal? Price { get; set; }
        public string Criteria { get; set; }

        public DealItemSourceData SourceData { get; set; }
    }
}
