using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Data.Models
{
    internal sealed class Contact
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool IsConfirmed { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<CoffeeRoastingEvent> CoffeeRoastingEvents { get; set; }
    }
}
