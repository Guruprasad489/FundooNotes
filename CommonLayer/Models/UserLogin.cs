using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class UserLogin
    {

        [Required(ErrorMessage = "{0} should not be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
