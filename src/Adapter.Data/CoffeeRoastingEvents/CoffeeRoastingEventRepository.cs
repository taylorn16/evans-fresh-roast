using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;

namespace Adapter.Data.CoffeeRoastingEvents
{
    internal sealed class CoffeeRoastingEventRepository : ICoffeeRoastingEventRepository
    {
        private readonly ICoffeeRoastingEventMapper _mapper;
        private readonly ICoffeeRoastingEventDataLayer _dataLayer;

        public CoffeeRoastingEventRepository(
            ICoffeeRoastingEventMapper mapper,
            ICoffeeRoastingEventDataLayer dataLayer)
        {
            _mapper = mapper;
            _dataLayer = dataLayer;
        }

        public async Task<CoffeeRoastingEvent> Save(CoffeeRoastingEvent @event, CancellationToken cancellationToken)
        {
            var dbEvent = await _mapper.Map(@event, cancellationToken);

            if (await _dataLayer.DoesCoffeeRoastingEventExist(@event.Id, cancellationToken))
            {
                await _dataLayer.UpdateCoffeeRoastingEvent(dbEvent, cancellationToken);
            }
            else
            {
                await _dataLayer.CreateCoffeeRoastingEvent(dbEvent, cancellationToken);
            }

            var createdOrUpdatedEvent = await _dataLayer.GetCoffeeRoastingEvent(@event.Id, cancellationToken);

            return _mapper.Map(createdOrUpdatedEvent);
        }

        public async Task<CoffeeRoastingEvent> GetActiveEvent(CancellationToken cancellationToken)
        {
            var activeEventId = await _dataLayer.GetActiveEventId(cancellationToken);
            var activeDbEvent = await _dataLayer.GetCoffeeRoastingEvent(activeEventId, cancellationToken);
            var domainEvent = _mapper.Map(activeDbEvent);

            return domainEvent;
        }

        public async Task<CoffeeRoastingEvent> Get(Id<CoffeeRoastingEvent> id, CancellationToken cancellationToken)
        {
            var dbEvent = await _dataLayer.GetCoffeeRoastingEvent(id, cancellationToken);
            var domainEvent = _mapper.Map(dbEvent);

            return domainEvent;
        }
    }
}
