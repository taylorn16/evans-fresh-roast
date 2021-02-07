using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Data.Models
{
    internal sealed class CoffeeRoastingEvent
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime OrderByDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Contact> Contacts { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<CoffeeRoastingEventCoffee> CoffeeRoastingEventCoffees { get; set; }
    }
}
