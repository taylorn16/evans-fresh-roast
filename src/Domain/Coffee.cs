﻿using Domain.Base;

namespace Domain
{
    public sealed class Coffee : Entity<Coffee>
    {
        public Name<Coffee> Name { get; }
        public Description Description { get; }
        public UsdPrice Price { get; }
        public Ounces NetWeightPerBag { get; }

        public Coffee(
            Id<Coffee> id,
            Name<Coffee> name,
            Description description,
            UsdPrice price,
            Ounces netWeightPerBag) : base(id)
        {
            Name = name;
            Description = description;
            Price = price;
            NetWeightPerBag = netWeightPerBag;
        }

        public Coffee With(
            Name<Coffee>? name = null,
            Description? description = null,
            UsdPrice? price = null,
            Ounces? netWeightPerBag = null) => new(
            Id,
            name ?? Name,
            description ?? Description,
            price ?? Price,
            netWeightPerBag ?? NetWeightPerBag);
    }
}
