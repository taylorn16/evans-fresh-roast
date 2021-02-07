using System;
using Domain.Exceptions;

namespace Domain.Base
{
    public static class ValueObject
    {
        public static TValueObject SafeCreate<TValueObject, TException>(Func<TValueObject> createFn, Func<ArgumentException, TException> handleArgExFn)
            where TException : DomainException
        {
            try
            {
                return createFn();
            }
            catch (ArgumentException argEx)
            {
                var domainValidationException = handleArgExFn(argEx);
                throw domainValidationException;
            }
        }
    }
}
