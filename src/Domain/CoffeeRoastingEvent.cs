using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dawn;
using Domain.Base;
using NodaTime;

namespace Domain
{
    public sealed class CoffeeRoastingEvent : Entity<CoffeeRoastingEvent>
    {
        public IEnumerable<Id<Contact>> NotifiedContacts { get; }
        public bool IsActive { get; private set; }
        public LocalDate Date { get; }
        public Name<CoffeeRoastingEvent> Name { get; }
        public IReadOnlyDictionary<OrderReferenceLabel, Coffee> OfferedCoffees { get; private set; }
        public IEnumerable<Order> Orders { get; private set; }

        public CoffeeRoastingEvent(
            Id<CoffeeRoastingEvent> id,
            IEnumerable<Id<Contact>> notifiedContacts,
            bool isActive,
            LocalDate date,
            Name<CoffeeRoastingEvent> name,
            IDictionary<OrderReferenceLabel, Coffee> offeredCoffees,
            IEnumerable<Order> orders) : base(id)
        {
            NotifiedContacts = notifiedContacts.Distinct();
            IsActive = isActive;
            Date = Guard.Argument(date, nameof(date))
                .Require(dt => dt >= LocalDate.FromDateTime(DateTime.Today));
            Name = name;
            OfferedCoffees = new ReadOnlyDictionary<OrderReferenceLabel, Coffee>(offeredCoffees);
            Orders = orders;

            if (OfferedCoffees.Values.Count() != OfferedCoffees.Values.Distinct().Count())
            {
                throw new ApplicationException("The same coffee cannot be added multiple times.");
            }
        }

        public string GetOfferingSummary()
        {
            var lines = OfferedCoffees
                .OrderBy(x => (string) x.Key)
                .Select(x => $"{x.Key}: {x.Value.Name} ({x.Value.Price} per {x.Value.NetWeightPerBag} bag) - {x.Value.Description}");

            var sb = new StringBuilder();
            foreach (var line in lines) sb.AppendLine(line);

            return sb.ToString();
        }

        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
            }
            else
            {
                throw new ApplicationException("This roasting event was already completed.");
            }
        }

        public void AddCoffeeOffering(OrderReferenceLabel label, Coffee coffee)
        {
            if (OfferedCoffees.ContainsKey(label))
            {
                throw new ApplicationException($"The label {label} is already taken and can't be repeated.");
            }

            if (OfferedCoffees.Values.Contains(coffee))
            {
                throw new ApplicationException("The same coffee has already been added to this coffee roasting event.");
            }

            var updatedCoffees = OfferedCoffees.ToDictionary(x => x.Key, x => x.Value);
            updatedCoffees.Add(label, coffee);

            OfferedCoffees = new ReadOnlyDictionary<OrderReferenceLabel, Coffee>(updatedCoffees);
        }

        public Order? GetOrder(Id<Contact> contact) => Orders.FirstOrDefault(o => o.Contact == contact);

        public void PlaceOrder(Id<Contact> contact, IDictionary<Id<Coffee>, OrderQuantity> details)
        {
            if (NotifiedContacts.All(c => c != contact))
            {
                throw new ApplicationException("Woops - you may not place a coffee order at this time. " +
                                               "Please wait for the next text blast.");
            }

            if (Orders.Any(o => o.Contact == contact))
            {
                throw new ApplicationException("Woops - you already placed an order. " +
                                               "If you are trying to make a change, reply CANCEL to cancel your order first.");
            }

            var invalidCoffees = new List<Id<Coffee>>();
            var totalCost = 0m;

            foreach (var detail in details)
            {
                var (coffee, qty) = detail;
                var offeredCoffee = OfferedCoffees.Values.FirstOrDefault(c => c.Id == coffee);

                if (offeredCoffee == null)
                {
                    invalidCoffees.Add(coffee);
                    continue;
                }

                var subTotal = offeredCoffee.Price * qty;
                totalCost += subTotal;
            }

            if (invalidCoffees.Any())
            {
                throw new ApplicationException("You cannot place an order for a coffee that was not offered. Errant coffee ids: " +
                                               $"{invalidCoffees.Aggregate(string.Empty, (a, b) => $"{a}, {b}")}");
            }

            var invoice = new Invoice(
                Id<Invoice>.New(),
                UsdInvoiceAmount.From(totalCost),
                false,
                PaymentMethod.NotSet);
            var order = new Order(
                Id<Order>.New(),
                contact,
                OffsetDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                details.ToDictionary(x => x.Key, x => x.Value),
                invoice,
                false);

            Orders = Orders.Concat(new [] { order });
        }

        public void ConfirmOrder(Id<Contact> contact)
        {
            var order = Orders.FirstOrDefault(o => o.Contact == contact);

            if (order == null)
            {
                throw new ApplicationException("Woops - looks like you haven't placed an order yet. You can place one now, though!");
            }

            order.Confirm();
        }
    }
}
