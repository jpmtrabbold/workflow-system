using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;

namespace Company.DealSystem.Application.Models.ViewModels.Shared
{
    public class StringLookupRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
