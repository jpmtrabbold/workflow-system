using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.Counterparties;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Counterparties
{
    public class CounterpartiesListResponse : ListResponse
    {
        public List<CounterpartyListDto> Counterparties { get; set; }
    }
}
