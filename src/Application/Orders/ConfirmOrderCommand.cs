using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Orders
{
    public sealed record ConfirmOrderCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class ConfirmOrderCommandHandler : AsyncRequestHandler<ConfirmOrderCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IMediator _mediator;

        public ConfirmOrderCommandHandler(
            IContactRepository contactRepository,
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IMediator mediator)
        {
            _contactRepository = contactRepository;
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(ConfirmOrderCommand cmd, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByPhoneNumber(cmd.PhoneNumber, cancellationToken);
            var activeRoastingEvent = await _coffeeRoastingEventRepository.GetActiveEvent(cancellationToken);

            activeRoastingEvent.ConfirmOrder(contact.Id);
            await _coffeeRoastingEventRepository.Save(activeRoastingEvent, cancellationToken);

            await _mediator.Publish(new OrderConfirmedNotification { PhoneNumber = contact.PhoneNumber }, cancellationToken);
        }
    }
}
