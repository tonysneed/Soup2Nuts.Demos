using System;

namespace SoupToNuts.Final.Service.Persistence.Exceptions
{
    public class UpdateConcurrencyException : Exception
    {
        public UpdateConcurrencyException()
        {
        }

        public UpdateConcurrencyException(string message)
            : base(message)
        {
        }

        public UpdateConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
