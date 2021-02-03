using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adapter.Data.Contacts;
using Adapter.Data.Models;
using Domain;
using Domain.Base;
using NodaTime;
using Coffee = Domain.Coffee;
using CoffeeRoastingEvent = Adapter.Data.Models.CoffeeRoastingEvent;
using Contact = Domain.Contact;
using Invoice = Adapter.Data.Models.Invoice;
using Order = Adapter.Data.Models.Order;

namespace Adapter.Data.CoffeeRoastingEvents
{
    internal interface ICoffeeRoastingEventMapper
    {
        Task<CoffeeRoastingEvent> Map(Domain.CoffeeRoastingEvent domainEvent, CancellationToken cancellationToken);
        Domain.CoffeeRoastingEvent Map(CoffeeRoastingEvent dbEvent);
    }

    internal sealed class CoffeeRoastingEventMapper : ICoffeeRoastingEventMapper
    {
        private readonly IContactDataLayer _contactDataLayer;

        public CoffeeRoastingEventMapper(IContactDataLayer contactDataLayer)
        {
            _contactDataLayer = contactDataLayer;
        }

        public async Task<CoffeeRoastingEvent> Map(Domain.CoffeeRoastingEvent domainEvent, CancellationToken cancellationToken)
        {
            var contacts = await _contactDataLayer.GetContacts(domainEvent.NotifiedContacts.Select(id => (Guid) id), cancellationToken);

            var @event = new CoffeeRoastingEvent
            {
                Id = domainEvent.Id,
                Date = domainEvent.Date.ToDateTimeUnspecified(),
                Name = domainEvent.Name,
                IsActive = domainEvent.IsActive,
                Contacts = contacts,
                Orders = domainEvent.Orders.Select(order => new Order
                {
                    Id = order.Id,
                    ContactId = order.Contact,
                    CreatedTimestamp = order.ReceivedTimestamp.ToDateTimeOffset(),
                    IsConfirmed = order.IsConfirmed,
                    Invoice = new Invoice
                    {
                        Id = order.Invoice.Id,
                        OrderId = order.Id,
                        Amount = order.Invoice.Amount,
                        IsPaid = order.Invoice.IsPaid,
                        PaymentMethod = order.Invoice.PaymentMethod,
                    },
                    OrderCoffees = order.Details.Select(x => new OrderCoffee
                    {
                        Quantity = x.Value,
                        CoffeeId = x.Key,
                        OrderId = order.Id,
                    }).ToList(),
                }).ToList(),
                CoffeeRoastingEventCoffees = domainEvent.OfferedCoffees.Select(x => new CoffeeRoastingEventCoffee
                {
                    Label = x.Key,
                    CoffeeId = x.Value.Id,
                    CoffeeRoastingEventId = domainEvent.Id,
                }).ToList(),
            };

            return @event;
        }

        public Domain.CoffeeRoastingEvent Map(CoffeeRoastingEvent dbEvent)
        {
            return new(
                Id<Domain.CoffeeRoastingEvent>.From(dbEvent.Id),
                dbEvent.Contacts.Select(c => c.Id).Select(id => Id<Contact>.From(id)),
                dbEvent.IsActive,
                LocalDate.FromDateTime(dbEvent.Date),
                Name<Domain.CoffeeRoastingEvent>.From(dbEvent.Name),
                dbEvent.CoffeeRoastingEventCoffees.ToDictionary(
                    ec => OrderReferenceLabel.From(ec.Label),
                    ec => new Coffee(
                        Id<Coffee>.From(ec.Coffee.Id),
                        Name<Coffee>.From(ec.Coffee.Name),
                        Description.From(ec.Coffee.Description),
                        UsdPrice.From(ec.Coffee.Price),
                        Ounces.From(ec.Coffee.OzWeight))),
                dbEvent.Orders.Select(order => new Domain.Order(
                    Id<Domain.Order>.From(order.Id),
                    Id<Contact>.From(order.ContactId),
                    OffsetDateTime.FromDateTimeOffset(order.CreatedTimestamp),
                    order.OrderCoffees.ToDictionary(
                        oc => Id<Coffee>.From(oc.CoffeeId),
                        oc => OrderQuantity.From(oc.Quantity)),
                    new Domain.Invoice(
                        Id<Domain.Invoice>.From(order.Invoice.Id),
                        UsdInvoiceAmount.From(order.Invoice.Amount),
                        order.Invoice.IsPaid,
                        order.Invoice.PaymentMethod),
                    order.IsConfirmed)));
        }
    }
}
