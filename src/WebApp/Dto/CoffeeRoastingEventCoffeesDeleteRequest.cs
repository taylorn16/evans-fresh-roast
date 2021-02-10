using System;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventCoffeesDeleteRequest
    {
        private Guid[]? CoffeeIds { get; init; }
    }
}
