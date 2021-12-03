using System;
using RestAPIDemo.Models;
using RestAPIDemo.Models.Extra;

namespace RestAPIDemo.Extensions
{
    public class UserValidatorExtenstion
    {
        public static Boolean IsValid(UserDto user)
        {

            if(!String.IsNullOrEmpty(user.Name) &&
                user.Designation != null &&
                user.JoiningDate != null &&
                !String.IsNullOrEmpty(user.FullAddress.Street) &&
                !String.IsNullOrEmpty(user.FullAddress.State) &&
                !String.IsNullOrEmpty(user.FullAddress.Country) &&
                !String.IsNullOrEmpty(user.FullAddress.PinCode)
                )
            {
                return true;
            }
            return false;
        }
    }
}
