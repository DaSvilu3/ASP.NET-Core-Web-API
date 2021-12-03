using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPIDemo.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public string FullAddress { get; set; }
        public string Country { get; set; }
        public string ImagePath { get; set; }
    }
}
