using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.CoffeeRoastingEvents
{
    public sealed record RetrieveCoffeeRoastingEventCommand : IRequest<CoffeeRoastingEvent>
    {
        public Id<CoffeeRoastingEvent> Id { get; init; }
    }

    internal sealed class RetrieveCoffeeRoastingEventCommandHandler : IRequestHandler<RetrieveCoffeeRoastingEventCommand, CoffeeRoastingEvent>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;

        public RetrieveCoffeeRoastingEventCommandHandler(ICoffeeRoastingEventRepository coffeeRoastingEventRepository)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
        }

        public async Task<CoffeeRoastingEvent> Handle(RetrieveCoffeeRoastingEventCommand cmd, CancellationToken cancellationToken) =>
            await _coffeeRoastingEventRepository.Get(cmd.Id, cancellationToken);
    }
}
