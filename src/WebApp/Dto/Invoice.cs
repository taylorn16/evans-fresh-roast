using System;
using Domain;

namespace WebApp.Dto
{
    public sealed record Invoice
    {
        public Guid? Id { get; init; }
        public bool? IsPaid { get; init; }
        public PaymentMethod? PaymentMethod { get; init; }
    }
}
