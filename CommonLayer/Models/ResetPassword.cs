using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [DataType(DataType.Password)]
        //[Compare("NewPassword", ErrorMessage = "The NewPassword and ConfirmPassword do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
