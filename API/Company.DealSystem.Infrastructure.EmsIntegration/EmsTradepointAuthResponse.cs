using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Infrastructure.EmsIntegration
{
    public class EmsTradepointAuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public long created_at { get; set; }
    }
}
