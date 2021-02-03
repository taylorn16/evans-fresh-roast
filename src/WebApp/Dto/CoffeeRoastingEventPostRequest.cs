using System;
using NodaTime;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventPostRequest
    {
        public string? Name { get; init; }
        public Guid[]? ContactIds { get; init; }
        public LocalDate? Date { get; init; }
        public Guid[]? CoffeeIds { get; init; }
    }
}
