using Dawn;
using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record OrderQuantity
    {
        private readonly int _qty;

        public static OrderQuantity Create(int qty) => ValueObject.SafeCreate(
            () => new OrderQuantity(qty),
            ex => new InvalidOrderQuantityException(qty, ex));

        private OrderQuantity(int qty)
        {
            _qty = Guard.Argument(qty, nameof(qty)).Positive().Max(15);
        }

        public override string ToString() => $"Qty {_qty}";

        public static implicit operator int(OrderQuantity qty) => qty._qty;
    }
}
