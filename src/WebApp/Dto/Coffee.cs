using System;

namespace WebApp.Dto
{
    public sealed record Coffee
    {
        public Guid? Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? PriceInUsd { get; init; }
        public decimal? WeightPerBagInOz { get; init; }
    }
}
