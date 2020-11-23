using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Application.Extensions
{
    public static class Base64Extensions
    {
        public static byte[] Base64StringToBinary(this string base64String)
        {
            return Convert.FromBase64String(base64String);
        }
    }
}
