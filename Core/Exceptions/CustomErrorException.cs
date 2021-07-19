using System;

namespace Core.Exceptions
{
    public class CustomErrorException : Exception
    {
        public CustomErrorException(string message) : base(message)
        {
        }

        public CustomErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
