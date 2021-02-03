using System;
using NodaTime;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEvent
    {
        public Guid? Id { get; init; }
        public Guid[]? ContactIds { get; init; }
        public bool? IsActive { get; init; }
        public LocalDate? Date { get; init; }
        public string? Name { get; init; }
        public OfferedCoffee[]? OfferedCoffees { get; init; }
        public Order[]? Orders { get; init; }
    }

    public sealed record OfferedCoffee
    {
        public string? Label { get; init; }
        public Coffee? Coffee { get; init; }
    }
}
