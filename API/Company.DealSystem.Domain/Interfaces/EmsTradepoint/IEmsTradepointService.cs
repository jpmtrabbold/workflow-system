using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.DealSystem.Domain.Interfaces.EmsTradepoint
{
    public interface IEmsTradepointService
    {
        Task<EmsTradepointFetchTradesResponse> FetchTrades(DateTimeOffset createdFrom, DateTimeOffset createdTo);
    }
}
