using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Application.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) : base(message)
        {

        }
    }
}
