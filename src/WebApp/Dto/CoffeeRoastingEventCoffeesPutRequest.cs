using System;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventCoffeesPutRequest
    {
        private Guid[]? CoffeeIds { get; init; }
    }
}
