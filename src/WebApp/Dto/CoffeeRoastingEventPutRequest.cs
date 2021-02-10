using NodaTime;

namespace WebApp.Dto
{
    public sealed record CoffeeRoastingEventPutRequest
    {
        public string? Name { get; init; }
        public bool? IsActive { get; init; }
        public OffsetDateTime? RoastDate { get; init; }
        public OffsetDateTime? OrderByDate { get; init; }
    }
}
