using System;

namespace Domain.Exceptions
{
    public abstract class DomainException : ApplicationException
    {
        protected DomainException() : base(null) { }

        protected DomainException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
