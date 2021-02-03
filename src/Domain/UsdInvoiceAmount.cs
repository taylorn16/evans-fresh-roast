using Dawn;

namespace Domain
{
    public sealed record UsdInvoiceAmount
    {
        private readonly decimal _amount;

        public static UsdInvoiceAmount From(decimal amount) => new(amount);

        private UsdInvoiceAmount(decimal amount)
        {
            _amount = Guard.Argument(amount, nameof(amount)).Positive().Max(1000);
        }

        public override string ToString() => $"USD {_amount:N}";

        public static implicit operator decimal(UsdInvoiceAmount amount) => amount._amount;
    }
}
