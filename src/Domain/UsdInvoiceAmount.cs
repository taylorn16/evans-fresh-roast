using Dawn;
using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record UsdInvoiceAmount
    {
        private readonly decimal _amount;

        public static UsdInvoiceAmount Create(decimal amount) => ValueObject.SafeCreate(
            () => new UsdInvoiceAmount(amount),
            ex => new InvalidInvoiceAmountException(amount, ex));

        private UsdInvoiceAmount(decimal amount)
        {
            _amount = Guard.Argument(amount, nameof(amount)).Positive().Max(1000);
        }

        public override string ToString() => $"USD {_amount:N}";

        public static implicit operator decimal(UsdInvoiceAmount amount) => amount._amount;
    }
}
