using System.Collections.Generic;
using Domain.Base;

namespace Domain.Exceptions
{
    public abstract class DomainBehaviorException : DomainException { }

    public sealed class CoffeeRoastingEventAlreadyCompletedException : DomainBehaviorException { }

    public sealed class CoffeeLabelAlreadyUsedInRoastingEventException : DomainBehaviorException
    {
        public OrderReferenceLabel Label { get; }

        public CoffeeLabelAlreadyUsedInRoastingEventException(OrderReferenceLabel label) => Label = label;
    }

    public sealed class CoffeeAlreadyAddedToRoastingEventException : DomainBehaviorException
    {
        public Coffee Coffee { get; }

        public CoffeeAlreadyAddedToRoastingEventException(Coffee coffee) => Coffee = coffee;
    }

    public sealed class ContactNotPartOfRoastingEventException : DomainBehaviorException
    {
        public Id<Contact> Contact { get; }

        public ContactNotPartOfRoastingEventException(Id<Contact> contact) => Contact = contact;
    }

    public sealed class ContactAlreadyPlacedOrderException : DomainBehaviorException
    {
        public Id<Contact> Contact { get; }

        public ContactAlreadyPlacedOrderException(Id<Contact> contact) => Contact = contact;
    }

    public sealed class OrderContainedInvalidCoffeesException : DomainBehaviorException
    {
        public IReadOnlyCollection<Id<Coffee>> Coffees { get; }

        public OrderContainedInvalidCoffeesException(IReadOnlyCollection<Id<Coffee>> coffees) => Coffees = coffees;
    }

    public sealed class OrderCancelledBeforeItWasPlacedException : DomainBehaviorException
    {
        public Id<Contact> Contact { get; }

        public OrderCancelledBeforeItWasPlacedException(Id<Contact> contact) => Contact = contact;
    }

    public sealed class OrderConfirmedBeforeItWasPlacedException : DomainBehaviorException
    {
        public Id<Contact> Contact { get; }

        public OrderConfirmedBeforeItWasPlacedException(Id<Contact> contact) => Contact = contact;
    }

    public sealed class OrderAlreadyConfirmedException : DomainBehaviorException { }

    public sealed class PaymentMethodNotSpecifiedException : DomainBehaviorException { }

    public sealed class InvoiceAlreadyPaidException : DomainBehaviorException { }

    public sealed class ContactAlreadyConfirmedException : DomainBehaviorException { }

    public sealed class ContactAlreadyDeactivatedException : DomainBehaviorException { }

    public sealed class ContactAlreadyActivatedException : DomainBehaviorException { }
}
