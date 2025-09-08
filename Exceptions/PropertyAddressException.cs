using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyUI.Exceptions
{
     public class PropertyAddressException: Exception
    {
        static string[] AllowedAddress = { "New York","San Francisco"};
        public PropertyAddressException(string address) : base($"Address '{address}' is not allowed. Only allowed address are: {string.Join(", ", AllowedAddress)}")
        { }
    }
}