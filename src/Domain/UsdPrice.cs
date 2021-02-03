using Dawn;

namespace Domain
{
    public sealed record UsdPrice
    {
        private readonly decimal _price;

        public static UsdPrice From(decimal price) => new(price);

        private UsdPrice(decimal price)
        {
            _price = Guard.Argument(price, nameof(price)).Positive().Max(200);
        }

        public override string ToString() => $"USD {_price:N}";

        public static implicit operator decimal(UsdPrice price) => price._price;
    }
}
