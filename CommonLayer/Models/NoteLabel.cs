using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class For Notes label Request
    /// </summary>
    public class NoteLabel
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        public string LabelName { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Note Id Should be Positive number")]
        public long NoteId { get; set; }
    }
}
