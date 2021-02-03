using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.CoffeeRoastingEvents
{
    public sealed record CoffeeRoastingEventCompletedNotification : INotification
    {
        public CoffeeRoastingEvent Event { get; init; }
    }

    internal sealed class CoffeeRoastingEventCompletedNotificationHandler : INotificationHandler<CoffeeRoastingEventCompletedNotification>
    {
        private readonly ISendSms _outboundSms;
        private readonly IContactRepository _contactRepository;

        public CoffeeRoastingEventCompletedNotificationHandler(
            ISendSms outboundSms,
            IContactRepository contactRepository)
        {
            _outboundSms = outboundSms;
            _contactRepository = contactRepository;
        }

        public async Task Handle(CoffeeRoastingEventCompletedNotification noty, CancellationToken cancellationToken)
        {
            var orders = noty.Event.Orders.ToArray();
            var contacts = await _contactRepository.Get(orders.Select(o => o.Contact), cancellationToken);

            var confirmedOrderPhoneNumbers =
                from order in orders
                where order.IsConfirmed
                join contact in contacts on order.Contact equals contact.Id
                select (order, contact.PhoneNumber);

            await Task.WhenAll(confirmedOrderPhoneNumbers.Select(SendReadyForPickupText));
        }

        private async Task SendReadyForPickupText((Order, UsPhoneNumber) orderPhoneNumber)
        {
            var (order, phoneNumber) = orderPhoneNumber;
            var message = BuildReadyForPickupMessage(order);

            await _outboundSms.Send(phoneNumber, message);
        }

        private static SmsMessage BuildReadyForPickupMessage(Order order)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Evan's fresh roast notice - your coffee is ready for pickup!");

            if (!order.Invoice.IsPaid)
            {
                sb.AppendLine($"If you haven't paid yet, make sure to Venmo Evan {order.Invoice.Amount}.");
            }

            return SmsMessage.From(sb.ToString());
        }
    }
}
