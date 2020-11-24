using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Application.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public string Title { get; set; }
        public BusinessRuleException(string message, string title = null) : base(message)
        {
            Title = title;
        }
    }
}
