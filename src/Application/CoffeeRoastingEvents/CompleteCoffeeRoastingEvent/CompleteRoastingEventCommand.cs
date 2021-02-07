using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using LanguageExt.Common;
using MediatR;

namespace Application.CoffeeRoastingEvents.CompleteCoffeeRoastingEvent
{
    public sealed record CompleteRoastingEventCommand : IRequest<Result<Unit>>
    {
        public Id<CoffeeRoastingEvent> Event { get; init; }
    }

    internal sealed class CompleteRoastingEventCommandHandler : IRequestHandler<CompleteRoastingEventCommand, Result<Unit>>
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

        public async Task<Result<Unit>> Handle(CompleteRoastingEventCommand cmd, CancellationToken cancellationToken)
        {
            var @event = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);

            @event.Deactivate();

            var savedEvent = await _coffeeRoastingEventRepository.Save(@event, cancellationToken);

            _mediator.Publish(new CoffeeRoastingEventCompletedNotification { Event = savedEvent });

            return Unit.Value;
        }
    }
}
