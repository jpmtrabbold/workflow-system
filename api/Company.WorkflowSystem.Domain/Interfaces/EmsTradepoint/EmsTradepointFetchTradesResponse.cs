using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Domain.Interfaces.EmsTradepoint
{
    public class EmsTradepointFetchTradesResponse
    {
        public ICollection<DealItemFromEmsTrade> Trades { get; set; }
        public string Payload { get; set; }
    }
}
