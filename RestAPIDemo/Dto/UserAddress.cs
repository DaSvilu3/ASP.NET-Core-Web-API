using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPIDemo.Models.Extra
{
    public class UserAddress
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PinCode { get; set; }
    }
}
