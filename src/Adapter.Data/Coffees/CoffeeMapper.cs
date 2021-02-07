using Domain;
using Domain.Base;
using Coffee = Adapter.Data.Models.Coffee;

namespace Adapter.Data.Coffees
{
    internal interface ICoffeeMapper
    {
        Coffee Map(Domain.Coffee domainCoffee);
        Domain.Coffee Map(Coffee dbCoffee);
    }

    internal sealed class CoffeeMapper : ICoffeeMapper
    {
        public Coffee Map(Domain.Coffee domainCoffee) => new()
        {
            Id = domainCoffee.Id,
            Description = domainCoffee.Description,
            Name = domainCoffee.Name,
            Price = domainCoffee.Price,
            OzWeight = domainCoffee.NetWeightPerBag,
        };

        public Domain.Coffee Map(Coffee dbCoffee) => new(
            Id<Domain.Coffee>.From(dbCoffee.Id),
            Name<Domain.Coffee>.Create(dbCoffee.Name),
            Description.Create(dbCoffee.Description),
            UsdPrice.Create(dbCoffee.Price),
            Ounces.Create(dbCoffee.OzWeight));
    }
}
