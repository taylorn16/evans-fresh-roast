using System;
using System.Linq;
using AutoFixture;
using Domain.Base;
using NodaTime;

namespace Domain.AutoFixture
{
    public sealed class CoffeeRoastingEventCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var roastDate = fixture.Create<LocalDate>();
                var offeredCoffees = new[] { fixture.Create<(OrderReferenceLabel, Coffee)>() };

                return new CoffeeRoastingEvent(
                    fixture.Create<Id<CoffeeRoastingEvent>>(),
                    fixture.CreateMany<Id<Contact>>(),
                    fixture.Create<bool>(),
                    roastDate,
                    roastDate - Period.FromDays(1),
                    fixture.Create<Name<CoffeeRoastingEvent>>(),
                    offeredCoffees.ToDictionary(t => t.Item1, t => t.Item2),
                    Array.Empty<Order>());
            });
        }
    }
}
