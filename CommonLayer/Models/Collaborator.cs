using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class For Collaborator Request
    /// </summary>
    public class Collaborator
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][a-zA-Z0-9]{1,})?@[a-zA-Z0-9]{1,10}[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3})?$", ErrorMessage = "Email Id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
    }
}
