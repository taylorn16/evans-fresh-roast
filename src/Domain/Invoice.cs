using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed class Invoice : Entity<Invoice>
    {
        public UsdInvoiceAmount Amount { get; }
        public bool IsPaid { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }

        public Invoice(
            Id<Invoice> id,
            UsdInvoiceAmount amount,
            bool isPaid,
            PaymentMethod paymentMethod) : base(id)
        {
            Amount = amount;
            IsPaid = isPaid;
            PaymentMethod = paymentMethod;
        }

        public void MarkAsPaid(PaymentMethod method)
        {
            if (IsPaid) throw new InvoiceAlreadyPaidException();
            if (method == PaymentMethod.NotSet) throw new PaymentMethodNotSpecifiedException();

            IsPaid = true;
            PaymentMethod = method;
        }
    }
}
