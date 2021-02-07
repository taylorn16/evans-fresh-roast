using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Application.Repositories;
using Domain;
using Domain.Exceptions;
using MediatR;
using MediatR.Pipeline;

namespace Application.Orders
{
    internal sealed class PlaceOrderDomainBehaviorExceptionHandler
        : IRequestExceptionHandler<PlaceOrderCommand, Unit, DomainBehaviorException>
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ISendSms _outboundSms;

        public PlaceOrderDomainBehaviorExceptionHandler(
            ICoffeeRepository coffeeRepository,
            ISendSms outboundSms)
        {
            _coffeeRepository = coffeeRepository;
            _outboundSms = outboundSms;
        }

        public async Task Handle(
            PlaceOrderCommand cmd,
            DomainBehaviorException exception,
            RequestExceptionHandlerState<Unit> state,
            CancellationToken cancellationToken)
        {
            var msg = exception switch
            {
                ContactNotPartOfRoastingEventException =>
                    "Sorry, you cannot place an order at this time. Keep an eye out for the next text blast!",
                ContactAlreadyPlacedOrderException =>
                    "Woops! Looks like you already placed an order. Reply CANCEL to cancel your order first if you want to make a change.",
                OrderContainedInvalidCoffeesException ex =>
                    await GetInvalidCoffeesMessage(ex, cancellationToken),
                _ =>
                    throw new ArgumentOutOfRangeException(nameof(exception)),
            };

            await _outboundSms.Send(cmd.PhoneNumber, SmsMessage.Create(msg));

            state.SetHandled(Unit.Value);
        }

        private async Task<string> GetInvalidCoffeesMessage(
            OrderContainedInvalidCoffeesException ex, CancellationToken cancellationToken)
        {
            var joinedInvalidCoffeeNames = (await _coffeeRepository.Get(ex.Coffees, cancellationToken))
                .Select(c => c.Name.ToString())
                .Aggregate((a, b) => $"{a}, {b}");

            var message = $"Sorry, the following coffees aren't being offered in this roasting event: {joinedInvalidCoffeeNames}. " +
                          "Please include only the listed coffees when placing an order.";

            return message;
        }
    }
}
