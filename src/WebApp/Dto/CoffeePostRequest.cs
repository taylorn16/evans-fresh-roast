namespace WebApp.Dto
{
    public sealed record CoffeePostRequest
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? PriceInUsd { get; init; }
        public decimal? WeightPerBagInOz { get; init; }
    }
}
