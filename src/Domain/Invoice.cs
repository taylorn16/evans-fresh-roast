using System;
using Domain.Base;

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
            if (!IsPaid)
            {
                if (method == PaymentMethod.NotSet)
                {
                    throw new ApplicationException("You must supply a payment method.");
                }

                IsPaid = true;
                PaymentMethod = method;
            }
            else
            {
                throw new ApplicationException("This invoice was already marked as paid.");
            }
        }
    }
}
