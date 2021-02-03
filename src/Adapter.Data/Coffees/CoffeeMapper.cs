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
            Name<Domain.Coffee>.From(dbCoffee.Name),
            Description.From(dbCoffee.Description),
            UsdPrice.From(dbCoffee.Price),
            Ounces.From(dbCoffee.OzWeight));
    }
}
