using System;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Adapter.Data.Models
{
    internal sealed class Invoice
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
