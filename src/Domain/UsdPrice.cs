using Dawn;
using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record UsdPrice
    {
        private readonly decimal _price;

        public static UsdPrice Create(decimal price) => ValueObject.SafeCreate(
            () => new UsdPrice(price),
            ex => new InvalidPriceException(price, ex));

        private UsdPrice(decimal price)
        {
            _price = Guard.Argument(price, nameof(price)).Positive().Max(200);
        }

        public override string ToString() => $"USD {_price:N}";

        public static implicit operator decimal(UsdPrice price) => price._price;
    }
}
