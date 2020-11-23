using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Application.Models.ViewModels.DealIntegration
{
    public class EmsFetchRequest
    {
        public DateTimeOffset StartCreationDateTime { get; set; }
        public DateTimeOffset EndCreationDateTime { get; set; }
    }
}
