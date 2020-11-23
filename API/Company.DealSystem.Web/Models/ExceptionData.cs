using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DealSystem.Web.Models
{
    public class ExceptionData
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool InvalidLogin { get; set; }
        public string Title { get; set; }
        public string StackTrace { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
