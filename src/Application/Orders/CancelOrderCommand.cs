using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Orders
{
    public sealed record CancelOrderCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class CancelOrderCommandHandler : AsyncRequestHandler<CancelOrderCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMediator _mediator;

        public CancelOrderCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IContactRepository contactRepository,
            IMediator mediator)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _contactRepository = contactRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CancelOrderCommand cmd, CancellationToken cancellationToken)
        {
            var activeEvent = await _coffeeRoastingEventRepository.GetActiveEvent(cancellationToken);
            var contact = await _contactRepository.GetByPhoneNumber(cmd.PhoneNumber, cancellationToken);

            activeEvent.CancelOrder(contact.Id);

            await _coffeeRoastingEventRepository.Save(activeEvent, cancellationToken);

            await _mediator.Publish(new OrderCancelledNotification { PhoneNumber = cmd.PhoneNumber }, cancellationToken);
        }
    }
}
