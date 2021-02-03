using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Data.Models
{
    internal sealed class Coffee
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal OzWeight { get; set; }

        public ICollection<OrderCoffee> OrderCoffees { get; set; }

        public ICollection<CoffeeRoastingEventCoffee> CoffeeRoastingEventCoffees { get; set; }
    }
}
