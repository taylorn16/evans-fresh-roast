using System;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventContactsPutRequest
    {
        private Guid[]? ContactIds { get; init; }
    }
}
