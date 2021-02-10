namespace WebApp.Dto
{
    public sealed record ContactPostRequest
    {
        public string? Name { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
