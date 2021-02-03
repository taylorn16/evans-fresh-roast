using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Base;

namespace Application.Repositories
{
    public interface ICoffeeRoastingEventRepository
    {
        Task<CoffeeRoastingEvent> Save(CoffeeRoastingEvent @event, CancellationToken cancellationToken);
        Task<CoffeeRoastingEvent> GetActiveEvent(CancellationToken cancellationToken);
        Task<CoffeeRoastingEvent> Get(Id<CoffeeRoastingEvent> id, CancellationToken cancellationToken);
    }
}
