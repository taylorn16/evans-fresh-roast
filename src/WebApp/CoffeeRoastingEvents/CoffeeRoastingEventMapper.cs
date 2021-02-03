using System;
using System.Linq;
using Domain;

namespace WebApp.CoffeeRoastingEvents
{
    public interface ICoffeeRoastingEventMapper
    {
        Dto.CoffeeRoastingEvent Map(CoffeeRoastingEvent @event);
    }

    public sealed class CoffeeRoastingEventMapper : ICoffeeRoastingEventMapper
    {
        public Dto.CoffeeRoastingEvent Map(CoffeeRoastingEvent @event) => new()
        {
            Id = @event.Id,
            Date = @event.Date,
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
    }
}
