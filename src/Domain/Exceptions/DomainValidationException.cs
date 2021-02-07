using System;
using System.Collections.Generic;
using NodaTime;

// ReSharper disable SuggestBaseTypeForParameter

namespace Domain.Exceptions
{
    public abstract class DomainValidationException : DomainException { }

    public abstract class DomainValidationException<TValue> : DomainException
    {
        public TValue FailedValue { get; }

        protected DomainValidationException(TValue value, ArgumentException argEx) : base(null, argEx)
        {
            FailedValue = value;
        }
    }

    public sealed class OrderByDateMustBeOnOrBeforeRoastDateException : DomainValidationException
    {
        public LocalDate OrderByDate { get; }
        public LocalDate RoastDate { get; }

        public OrderByDateMustBeOnOrBeforeRoastDateException(LocalDate orderByDate, LocalDate roastDate)
        {
            OrderByDate = orderByDate;
            RoastDate = roastDate;
        }
    }

    public sealed class CoffeesInSameRoastingEventMustBeUniqueException : DomainValidationException
    {
        public IReadOnlyCollection<Coffee> DuplicateCoffees { get; }

        public CoffeesInSameRoastingEventMustBeUniqueException(IReadOnlyCollection<Coffee> duplicateCoffees) => DuplicateCoffees = duplicateCoffees;
    }

    public sealed class InvalidDescriptionException : DomainValidationException<string>
    {
        public InvalidDescriptionException(string value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidNameException : DomainValidationException<string>
    {
        public InvalidNameException(string value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidOrderQuantityException : DomainValidationException<int>
    {
        public InvalidOrderQuantityException(int value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidOrderReferenceLabelException : DomainValidationException<string>
    {
        public InvalidOrderReferenceLabelException(string value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidOuncesException : DomainValidationException<decimal>
    {
        public InvalidOuncesException(decimal value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidSmsMessageException : DomainValidationException<string>
    {
        public InvalidSmsMessageException(string value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidInvoiceAmountException : DomainValidationException<decimal>
    {
        public InvalidInvoiceAmountException(decimal value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidPriceException : DomainValidationException<decimal>
    {
        public InvalidPriceException(decimal value, ArgumentException argEx) : base(value, argEx) { }
    }

    public sealed class InvalidPhoneNumberException : DomainValidationException<string>
    {
        public InvalidPhoneNumberException(string value, ArgumentException argEx) : base(value, argEx) { }
    }
}
