using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.CoffeeRoastingEvents
{
    public sealed record CompleteRoastingEventCommand : IRequest
    {
        public Id<CoffeeRoastingEvent> Event { get; init; }
    }

    internal sealed class CompleteRoastingEventCommandHandler : AsyncRequestHandler<CompleteRoastingEventCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IMediator _mediator;

        public CompleteRoastingEventCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IMediator mediator)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CompleteRoastingEventCommand cmd, CancellationToken cancellationToken)
        {
            var @event = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);

            @event.Deactivate();

            var savedEvent = await _coffeeRoastingEventRepository.Save(@event, cancellationToken);

            await _mediator.Publish(new CoffeeRoastingEventCompletedNotification { Event = savedEvent }, cancellationToken);
        }
    }
}
