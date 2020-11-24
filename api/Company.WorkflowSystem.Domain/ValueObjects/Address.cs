using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Domain.ValueObjects
{
    public class Address : BaseValueObject
    {
        public string Street { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
