using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adapter.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Data.Coffees
{
    internal interface ICoffeeDataLayer
    {
        Task<List<Coffee>> GetAllCoffees(CancellationToken cancellationToken);
        Task<List<Coffee>> GetCoffees(IEnumerable<Guid> ids, CancellationToken cancellationToken);
        Task CreateCoffee(Coffee coffee, CancellationToken cancellationToken);
        Task UpdateCoffee(Coffee coffee, CancellationToken cancellationToken);
    }

    internal sealed class CoffeeDataLayer : ICoffeeDataLayer
    {
        private readonly IDbContextFactory _dbContextFactory;

        public CoffeeDataLayer(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Coffee>> GetCoffees(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            return await db.Coffees.Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<Coffee>> GetAllCoffees(CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            return await db.Coffees.ToListAsync(cancellationToken);
        }

        public async Task CreateCoffee(Coffee coffee, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            await db.Coffees.AddAsync(coffee, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCoffee(Coffee coffee, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            db.Coffees.Update(coffee);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
