using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Domain.Interfaces.EmsTradepoint
{
    public class DealItemFromEmsTrade
    {
        public double? TradeId { get; set; } //id
        public DateTimeOffset StartDate { get; set; } //Delivery_start
        public DateTimeOffset EndDate { get; set; } //Delivery_end
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public EmsTradePositionEnum Position { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public double? TraderId { get; set; }
        public string TraderName { get; set; }
    }

    public enum EmsTradePositionEnum
    {
        Sell = 0,
        Buy = 1,
    }
}
