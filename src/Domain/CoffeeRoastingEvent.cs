using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Domain.Base;
using Domain.Exceptions;
using NodaTime;

namespace Domain
{
    public sealed class CoffeeRoastingEvent : Entity<CoffeeRoastingEvent>
    {
        public IEnumerable<Id<Contact>> NotifiedContacts { get; private set; }
        public bool IsActive { get; private set; }
        public LocalDate RoastDate { get; }
        public LocalDate OrderByDate { get; }
        public Name<CoffeeRoastingEvent> Name { get; }
        public IReadOnlyDictionary<OrderReferenceLabel, Coffee> OfferedCoffees { get; private set; }
        public IEnumerable<Order> Orders { get; private set; }

        public CoffeeRoastingEvent(
            Id<CoffeeRoastingEvent> id,
            IEnumerable<Id<Contact>> notifiedContacts,
            bool isActive,
            LocalDate roastDate,
            LocalDate orderByDate,
            Name<CoffeeRoastingEvent> name,
            IDictionary<OrderReferenceLabel, Coffee> offeredCoffees,
            IEnumerable<Order> orders) : base(id)
        {
            NotifiedContacts = notifiedContacts.Distinct();
            IsActive = isActive;
            RoastDate = roastDate;
            OrderByDate = orderByDate;
            Name = name;
            OfferedCoffees = new ReadOnlyDictionary<OrderReferenceLabel, Coffee>(offeredCoffees);
            Orders = orders;

            if (orderByDate > roastDate)
            {
                throw new OrderByDateMustBeOnOrBeforeRoastDateException(orderByDate, roastDate);
            }

            if (OfferedCoffees.Values.Count() != OfferedCoffees.Values.Distinct().Count())
            {
                var duplicates = OfferedCoffees.Values.Except(OfferedCoffees.Values.Distinct());
                throw new CoffeesInSameRoastingEventMustBeUniqueException(duplicates.ToArray());
            }
        }

        public void AddNotifiedContact(Id<Contact> contact)
        {
            NotifiedContacts = NotifiedContacts.Concat(new[] { contact }).Distinct();
        }

        public string GetCoffeesSummary()
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
            if (!IsActive) throw new CoffeeRoastingEventAlreadyCompletedException();

            IsActive = false;
        }

        public void AddCoffee(OrderReferenceLabel label, Coffee coffee)
        {
            if (OfferedCoffees.ContainsKey(label)) throw new CoffeeLabelAlreadyUsedInRoastingEventException(label);

            if (OfferedCoffees.Values.Contains(coffee)) throw new CoffeeAlreadyAddedToRoastingEventException(coffee);

            var updatedCoffees = OfferedCoffees.ToDictionary(x => x.Key, x => x.Value);
            updatedCoffees.Add(label, coffee);

            OfferedCoffees = new ReadOnlyDictionary<OrderReferenceLabel, Coffee>(updatedCoffees);
        }

        public Order? GetOrder(Id<Contact> contact) => Orders.FirstOrDefault(o => o.Contact == contact);

        public void PlaceOrder(Id<Contact> contact, IDictionary<Id<Coffee>, OrderQuantity> details)
        {
            if (NotifiedContacts.All(c => c != contact)) throw new ContactNotPartOfRoastingEventException(contact);

            if (Orders.Any(o => o.Contact == contact)) throw new ContactAlreadyPlacedOrderException(contact);

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

            if (invalidCoffees.Any()) throw new OrderContainedInvalidCoffeesException(invalidCoffees);

            var order = CreateOrder(contact, details, totalCost);

            Orders = Orders.Concat(new [] { order });
        }

        private static Order CreateOrder(Id<Contact> contact, IDictionary<Id<Coffee>, OrderQuantity> details, decimal totalCost) => new(
            Id<Order>.New(),
            contact,
            OffsetDateTime.FromDateTimeOffset(DateTimeOffset.Now),
            details.ToDictionary(x => x.Key, x => x.Value),
            new Invoice(
                Id<Invoice>.New(),
                UsdInvoiceAmount.Create(totalCost),
                false,
                PaymentMethod.NotSet),
            false);

        public void CancelOrder(Id<Contact> contact)
        {
            var orderForContact = Orders.FirstOrDefault(o => o.Contact == contact);

            if (orderForContact == null) throw new OrderCancelledBeforeItWasPlacedException(contact);

            Orders = Orders.Where(o => o.Contact != contact);
        }

        public void ConfirmOrder(Id<Contact> contact)
        {
            var order = Orders.FirstOrDefault(o => o.Contact == contact);

            if (order == null) throw new OrderConfirmedBeforeItWasPlacedException(contact);

            order.Confirm();
        }
    }
}
