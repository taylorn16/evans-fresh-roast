using System;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventContactsDeleteRequest
    {
        private Guid[]? ContactIds { get; init; }
    }
}
