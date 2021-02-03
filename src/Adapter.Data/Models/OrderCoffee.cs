using System;

namespace Adapter.Data.Models
{
    internal sealed class OrderCoffee
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid CoffeeId { get; set; }
        public Coffee Coffee { get; set; }
    }
}
