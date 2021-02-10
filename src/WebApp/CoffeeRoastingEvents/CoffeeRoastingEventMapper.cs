using System;
using System.Linq;
using Application.CoffeeRoastingEvents.CreateCoffeeRoastingEvent;
using Domain;
using Domain.Base;
using NodaTime;
using Coffee = Domain.Coffee;
using CoffeeRoastingEvent = Domain.CoffeeRoastingEvent;
using Contact = Domain.Contact;

namespace WebApp.CoffeeRoastingEvents
{
    public interface ICoffeeRoastingEventMapper
    {
        Dto.CoffeeRoastingEvent Map(CoffeeRoastingEvent @event);
        CreateCoffeeRoastingEventCommand Map(Dto.CoffeeRoastingEventPostRequest request);
    }

    public sealed class CoffeeRoastingEventMapper : ICoffeeRoastingEventMapper
    {
        public Dto.CoffeeRoastingEvent Map(CoffeeRoastingEvent @event) => new()
        {
            Id = @event.Id,
            Date = @event.RoastDate,
            Name = @event.Name,
            IsActive = @event.IsActive,
            ContactIds = @event.NotifiedContacts.Select(id => (Guid) id).ToArray(),
            OfferedCoffees = @event.OfferedCoffees.Select(x => new Dto.OfferedCoffee
            {
                Label = x.Key,
                Coffee = new Dto.Coffee
                {
                    Id = x.Value.Id,
                    Name = x.Value.Name,
                    Description = x.Value.Description,
                    PriceInUsd = x.Value.Price,
                    WeightPerBagInOz = x.Value.NetWeightPerBag,
                },
            }).OrderBy(c => c.Label).ToArray(),
            Orders = @event.Orders.Select(order => new Dto.Order
            {
                Id = order.Id,
                Timestamp = order.ReceivedTimestamp,
                IsConfirmed = order.IsConfirmed,
                Invoice = new Dto.Invoice
                {
                    Id = order.Invoice.Id,
                    IsPaid = order.Invoice.IsPaid,
                    PaymentMethod = order.Invoice.PaymentMethod == PaymentMethod.NotSet
                        ? null
                        : order.Invoice.PaymentMethod,
                },
                OrderedCoffees = order.Details.Select(x => new Dto.OrderedCoffee
                {
                    CoffeeId = x.Key,
                    Quantity = x.Value,
                }).ToArray(),
            }).ToArray(),
        };

        public CreateCoffeeRoastingEventCommand Map(Dto.CoffeeRoastingEventPostRequest request) => new()
        {
            Name = Name<CoffeeRoastingEvent>.Create(request.Name!),
            RoastDate = request.RoastDate!.Value,
            OrderByDate = request.OrderByDate ?? request.RoastDate.Value.Minus(Period.FromDays(1)),
            Coffees = request.CoffeeIds?.Select(id => Id<Coffee>.From(id)).ToArray() ?? Array.Empty<Id<Coffee>>(),
            Contacts = request.ContactIds?.Select(id => Id<Contact>.From(id)).ToArray() ?? Array.Empty<Id<Contact>>(),
        };
    }
}
