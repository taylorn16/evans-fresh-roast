using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Data.Models
{
    internal sealed class Order
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }

        [Required]
        public DateTimeOffset CreatedTimestamp { get; set; }

        public bool IsConfirmed { get; set; }

        public Invoice Invoice { get; set; }

        public ICollection<OrderCoffee> OrderCoffees { get; set; }
    }
}
