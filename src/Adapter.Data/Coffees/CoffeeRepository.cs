using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;

namespace Adapter.Data.Coffees
{
    internal sealed class CoffeeRepository : ICoffeeRepository
    {
        private readonly ICoffeeDataLayer _dataLayer;
        private readonly ICoffeeMapper _mapper;

        public CoffeeRepository(
            ICoffeeDataLayer dataLayer,
            ICoffeeMapper mapper)
        {
            _dataLayer = dataLayer;
            _mapper = mapper;
        }

        public async Task<Coffee> Save(Coffee coffee, CancellationToken cancellationToken)
        {
            var existingCoffees = await _dataLayer.GetCoffees(new[] { (Guid) coffee.Id }, cancellationToken);
            var dbCoffee = _mapper.Map(coffee);

            if (!existingCoffees.Any())
            {
                await _dataLayer.CreateCoffee(dbCoffee, cancellationToken);
            }
            else
            {
                await _dataLayer.UpdateCoffee(dbCoffee, cancellationToken);
            }

            var createdOrUpdatedCoffee = (await _dataLayer.GetCoffees(new[] { (Guid) coffee.Id }, cancellationToken))
                .Single();
            var domainCoffee = _mapper.Map(createdOrUpdatedCoffee);

            return domainCoffee;
        }

        public async Task<IEnumerable<Coffee>> Get(IEnumerable<Id<Coffee>> ids, CancellationToken cancellationToken)
        {
            var dbCoffees = await _dataLayer.GetCoffees(ids.Select(id => (Guid) id), cancellationToken);

            return dbCoffees.Select(_mapper.Map);
        }
    }
}
