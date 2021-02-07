using System;

namespace Application.Behaviors
{
    public sealed class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }
    }
}
