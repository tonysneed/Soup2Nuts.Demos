using System;

namespace SoupToNuts.Final.Service.Persistence.Exceptions
{
    public class UpdateException : Exception
    {
        public UpdateException()
        {
        }

        public UpdateException(string message)
            : base(message)
        {
        }

        public UpdateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
