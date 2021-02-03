using System;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Data.Models
{
    internal sealed class CoffeeRoastingEventCoffee
    {
        public int Id { get; set; }

        [Required]
        public string Label { get; set; }

        public Coffee Coffee { get; set; }
        public Guid CoffeeId { get; set; }

        public CoffeeRoastingEvent CoffeeRoastingEvent { get; set; }
        public Guid CoffeeRoastingEventId { get; set; }
    }
}
