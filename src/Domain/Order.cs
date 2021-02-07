using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.Base;
using Domain.Exceptions;
using NodaTime;

namespace Domain
{
    public sealed class Order : Entity<Order>
    {
        public Id<Contact> Contact { get; }
        public OffsetDateTime ReceivedTimestamp { get; }
        public IReadOnlyDictionary<Id<Coffee>, OrderQuantity> Details { get; }
        public Invoice Invoice { get; }
        public bool IsConfirmed { get; private set; }

        public Order(
            Id<Order> id,
            Id<Contact> contact,
            OffsetDateTime receivedTimestamp,
            IDictionary<Id<Coffee>, OrderQuantity> details,
            Invoice invoice,
            bool isConfirmed) : base(id)
        {
            Contact = contact;
            ReceivedTimestamp = receivedTimestamp;
            Details = new ReadOnlyDictionary<Id<Coffee>, OrderQuantity>(details);
            Invoice = invoice;
            IsConfirmed = isConfirmed;
        }

        public void Confirm()
        {
            if (IsConfirmed) throw new OrderAlreadyConfirmedException();

            IsConfirmed = true;
        }

        public void MarkAsPaid(PaymentMethod method)
        {
            Invoice.MarkAsPaid(method);
        }
    }
}
