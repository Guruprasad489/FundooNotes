using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// User ticket class for producer 
    /// </summary>
    public class UserTicket
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Token { get; set; }
        public DateTime IssueAt { get; set; }
    }
}
