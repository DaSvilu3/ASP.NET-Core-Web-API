using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPIDemo.Models.Extra
{
    public class UserDto
    {

        public Guid UserId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        internal bool IsValid()
        {
            throw new NotImplementedException();
        }

        [Required]
        [Display(Name = "Designation Date")]
        public DateTime Designation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joinging Date")]
        public DateTime JoiningDate { get; set; }
        [Required]
        public UserAddress FullAddress { get; set; }
        public string ImagePath { get; set; }
    }
}
