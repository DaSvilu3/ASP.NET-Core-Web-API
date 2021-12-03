using System;
using System.Text.Json;
using RestAPIDemo.Models.Extra;

namespace RestAPIDemo.Extensions
{
    public class UserAddressFormatter
    {
        public static string FormatUserAddress(UserAddress address)
        {
            return JsonSerializer.Serialize(address); 
        }
    }
}
