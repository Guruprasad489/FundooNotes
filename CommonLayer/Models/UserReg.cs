using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class For User registration Request
    /// </summary>
    public class UserReg
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name should starts with Cap and should have minimum 3 characters")]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$", ErrorMessage = "First name is not valid")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name should starts with Cap and should have minimum 3 characters")]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$", ErrorMessage = "Last name is not valid")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][a-zA-Z0-9]{1,})?@[a-zA-Z0-9]{1,10}[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3})?$", ErrorMessage = "Email Id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[&%$#@?^*!~]).{8,}$", ErrorMessage = "Passsword is not valid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
