using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Domain.ExtensionMethods
{
    public static class IConfigurationExtensions
    {
        public static int GetIntValue(this IConfiguration configuration, string key)
        {
            var value = configuration[key];
            return int.Parse(value);
        }
    }
}
