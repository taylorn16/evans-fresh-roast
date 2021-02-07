using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Application
{
    internal interface ILabelCoffees
    {
        IDictionary<OrderReferenceLabel, Coffee> Label(IReadOnlyCollection<Coffee> coffees);
    }

    internal sealed class CoffeeLabeler : ILabelCoffees
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public IDictionary<OrderReferenceLabel, Coffee> Label(IReadOnlyCollection<Coffee> coffees)
        {
            if (coffees.Count > Alphabet.Length)
            {
                throw new ApplicationException("Sorry, we can't apply a unique label to more than 26 coffees.");
            }

            return Alphabet
                .Take(coffees.Count)
                .Select(ch => OrderReferenceLabel.Create(ch.ToString()))
                .Zip(coffees)
                .ToDictionary(x => x.Item1, x => x.Item2);
        }
    }
}
