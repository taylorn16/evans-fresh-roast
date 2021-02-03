using System;
using NodaTime;

namespace WebApp.Dto
{
    public sealed record Order
    {
        public Guid? Id { get; init; }
        public OffsetDateTime? Timestamp { get; init; }
        public OrderedCoffee[]? OrderedCoffees { get; init; }
        public bool? IsConfirmed { get; init; }
        public Invoice? Invoice { get; init; }
    }

    public sealed record OrderedCoffee
    {
        public Guid? CoffeeId { get; init; }
        public int? Quantity { get; init; }
    }
}
