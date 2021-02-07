using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Orders
{
    public sealed record OrderPlacedNotification : INotification
    {
        public Order Order { get; init; }
    }

    internal sealed class OrderPlacedNotificationHandler : INotificationHandler<OrderPlacedNotification>
    {
        private readonly ISendSms _outboundSms;
        private readonly IContactRepository _contactRepository;
        private readonly ICoffeeRepository _coffeeRepository;

        public OrderPlacedNotificationHandler(
            ISendSms outboundSms,
            IContactRepository contactRepository,
            ICoffeeRepository coffeeRepository)
        {
            _outboundSms = outboundSms;
            _contactRepository = contactRepository;
            _coffeeRepository = coffeeRepository;
        }

        public async Task Handle(OrderPlacedNotification noty, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.Get(noty.Order.Contact, cancellationToken);

            await SendConfirmOrderMessage(contact.PhoneNumber, noty.Order, cancellationToken);
        }

        private async Task SendConfirmOrderMessage(UsPhoneNumber phoneNumber, Order order, CancellationToken cancellationToken)
        {
            var coffees = await _coffeeRepository.Get(order.Details.Select(x => x.Key), cancellationToken);
            var message = BuildConfirmOrderMessage(order, coffees);

            await _outboundSms.Send(phoneNumber, message);
        }

        private static SmsMessage BuildConfirmOrderMessage(Order order, IEnumerable<Coffee> coffees)
        {
            var coffeeQuantities =
                from coffee in coffees
                join detail in order.Details on coffee.Id equals detail.Key
                select new { Quantity = detail.Key, Coffee = coffee };

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Thank you for your order! You ordered");
            messageBuilder.AppendLine();

            foreach (var cq in coffeeQuantities)
            {
                messageBuilder.AppendLine($"{cq.Quantity} {cq.Coffee.Name}");
            }

            messageBuilder.AppendLine();
            messageBuilder.AppendLine($"The total for your order is {order.Invoice.Amount}. " +
                                      "If this is not correct, please reply CANCEL to cancel your order. " +
                                      "If this is correct, please reply CONFIRM to confirm your order.");

            return SmsMessage.Create(messageBuilder.ToString());
        }
    }
}
