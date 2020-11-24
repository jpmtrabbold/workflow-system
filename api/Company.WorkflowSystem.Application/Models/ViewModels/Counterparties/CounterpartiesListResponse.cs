using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.Counterparties;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Counterparties
{
    public class CounterpartiesListResponse : ListResponse
    {
        public List<CounterpartyListDto> Counterparties { get; set; }
    }
}
