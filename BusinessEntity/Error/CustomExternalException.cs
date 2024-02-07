using System;

namespace BusinessEntity.Error
{
    public class CustomExternalException : Exception
    {
        public CustomExternalException(string message) : base(message)
        {

        }
    }
}
