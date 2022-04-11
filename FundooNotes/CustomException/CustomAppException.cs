using System;

namespace FundooNotes.CustomException
{
    /// <summary>
    /// Custom exception class for throwing application specific exceptions that can be caught and handled within the application
    /// </summary>
    public class CustomAppException : Exception
    {
        public CustomAppException() : base() { }

        public CustomAppException(string message) : base(message) { }
    }
}
