using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.Orders
{
    public sealed record PlaceOrderCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
        public IReadOnlyDictionary<OrderReferenceLabel, OrderQuantity> OrderDetails { get; init; }
    }

    internal sealed class PlaceOrderCommandHandler : AsyncRequestHandler<PlaceOrderCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IMediator _mediator;

        public PlaceOrderCommandHandler(
            IContactRepository contactRepository,
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IMediator mediator)
        {
            _contactRepository = contactRepository;
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(PlaceOrderCommand cmd, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByPhoneNumber(cmd.PhoneNumber, cancellationToken);
            var activeRoastingEvent = await _coffeeRoastingEventRepository.GetActiveEvent(cancellationToken);
            var orderDetails = GenerateOrderDetails(cmd, activeRoastingEvent);

            activeRoastingEvent.PlaceOrder(contact.Id, orderDetails);

            var savedEvent = await _coffeeRoastingEventRepository.Save(activeRoastingEvent, cancellationToken);

            var order = savedEvent.GetOrder(contact.Id)!;

            await _mediator.Publish(new OrderPlacedNotification { Order = order }, cancellationToken);
        }

        private static IDictionary<Id<Coffee>, OrderQuantity> GenerateOrderDetails(PlaceOrderCommand cmd, CoffeeRoastingEvent @event) =>
            (from detail in cmd.OrderDetails
             join coffee in @event.OfferedCoffees on detail.Key equals coffee.Key
             select new { Quantity = detail.Value, Coffee = coffee.Value })
            .ToDictionary(x => x.Coffee.Id, x => x.Quantity);
    }
}
