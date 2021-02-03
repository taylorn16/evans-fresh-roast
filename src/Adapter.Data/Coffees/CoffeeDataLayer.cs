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
        Task<List<Coffee>> GetCoffees(IEnumerable<Guid> ids, CancellationToken cancellationToken);
        Task CreateCoffee(Coffee coffee, CancellationToken cancellationToken);
        Task UpdateCoffee(Coffee coffee, CancellationToken cancellationToken);
    }

    internal sealed class CoffeeDataLayer : ICoffeeDataLayer
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CoffeeDataLayer(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Coffee>> GetCoffees(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            return await db.Coffees.Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
        }

        public async Task CreateCoffee(Coffee coffee, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            await db.Coffees.AddAsync(coffee, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCoffee(Coffee coffee, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            db.Coffees.Update(coffee);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
