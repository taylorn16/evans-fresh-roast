﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adapter.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Data.CoffeeRoastingEvents
{
    internal interface ICoffeeRoastingEventDataLayer
    {
        Task<CoffeeRoastingEvent> GetCoffeeRoastingEvent(Guid coffeeRoastingEventId, CancellationToken cancellationToken);
        Task<bool> DoesCoffeeRoastingEventExist(Guid coffeeRoastingEventId, CancellationToken cancellationToken);
        Task CreateCoffeeRoastingEvent(CoffeeRoastingEvent @event, CancellationToken cancellationToken);
        Task UpdateCoffeeRoastingEvent(CoffeeRoastingEvent @event, CancellationToken cancellationToken);
        Task<Guid> GetActiveEventId(CancellationToken cancellationToken);
    }

    internal sealed class CoffeeRoastingEventDataLayer : ICoffeeRoastingEventDataLayer
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CoffeeRoastingEventDataLayer(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Guid> GetActiveEventId(CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            return (await db.CoffeeRoastingEvents.SingleAsync(ev => ev.IsActive, cancellationToken)).Id;
        }

        public async Task<bool> DoesCoffeeRoastingEventExist(Guid coffeeRoastingEventId, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            var existingEvent = await db.CoffeeRoastingEvents.SingleOrDefaultAsync(ev => ev.Id == coffeeRoastingEventId, cancellationToken);

            return existingEvent != null;
        }

        public async Task<CoffeeRoastingEvent> GetCoffeeRoastingEvent(Guid coffeeRoastingEventId, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            return await db.CoffeeRoastingEvents
                .Where(ev => ev.Id == coffeeRoastingEventId)
                .Include(ev => ev.Contacts)
                .Include(ev => ev.Orders)
                    .ThenInclude(o => o.Invoice)
                .Include(ev => ev.Orders)
                    .ThenInclude(o => o.OrderCoffees)
                        .ThenInclude(oc => oc.Coffee)
                .Include(ev => ev.CoffeeRoastingEventCoffees)
                    .ThenInclude(ec => ec.Coffee)
                .SingleAsync(cancellationToken);
        }

        public async Task CreateCoffeeRoastingEvent(CoffeeRoastingEvent @event, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            await db.CoffeeRoastingEvents.AddAsync(@event, cancellationToken);

            foreach (var contact in @event.Contacts)
            {
                db.Entry(contact).State = EntityState.Modified;
            }

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCoffeeRoastingEvent(CoffeeRoastingEvent @event, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            db.CoffeeRoastingEvents.Update(@event);

            var existingEvent = await db.CoffeeRoastingEvents
                .Where(ev => ev.Id == @event.Id)
                .Include(ev => ev.Orders)
                    .ThenInclude(o => o.OrderCoffees)
                .Include(ev => ev.CoffeeRoastingEventCoffees)
                .Include(ev => ev.Contacts)
                .SingleAsync(cancellationToken);

            var existingContactIds = existingEvent.Contacts.Select(c => c.Id);

            foreach (var existingContact in @event.Contacts.Where(c => existingContactIds.Contains(c.Id)))
            {
                db.Entry(existingContact).State = EntityState.Unchanged;
            }

            var existingOrderIds = existingEvent.Orders.Select(o => o.Id).ToArray();

            foreach (var existingOrder in @event.Orders.Where(o => existingOrderIds.Contains(o.Id)))
            {
                db.Entry(existingOrder).State = EntityState.Modified;
                db.Entry(existingOrder.Invoice).State = EntityState.Modified;
            }

            foreach (var newOrder in @event.Orders.Where(o => !existingOrderIds.Contains(o.Id)))
            {
                db.Entry(newOrder).State = EntityState.Added;
                db.Entry(newOrder.Invoice).State = EntityState.Added;
            }

            db.RemoveRange(existingEvent.CoffeeRoastingEventCoffees);
            db.RemoveRange(existingEvent.Orders.SelectMany(o => o.OrderCoffees));

            await db.AddRangeAsync(@event.CoffeeRoastingEventCoffees, cancellationToken);
            await db.AddRangeAsync(@event.Orders.SelectMany(o => o.OrderCoffees), cancellationToken);


            await db.SaveChangesAsync(cancellationToken);
        }
    }
}