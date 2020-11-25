using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.Counterparties;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Counterparties
{
    public class CounterpartiesListResponse : ListResponse
    {
        public List<CounterpartyListDto> Counterparties { get; set; }
    }
}
