using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class For User login Response
    /// </summary>
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
