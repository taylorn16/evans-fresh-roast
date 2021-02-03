using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Domain;
using MediatR;

namespace Application.Orders
{
    public sealed record OrderConfirmedNotification : INotification
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class OrderConfirmedNotificationHandler : INotificationHandler<OrderConfirmedNotification>
    {
        private readonly ISendSms _outboundSms;

        public OrderConfirmedNotificationHandler(ISendSms outboundSms)
        {
            _outboundSms = outboundSms;
        }

        public async Task Handle(OrderConfirmedNotification noty, CancellationToken _)
        {
            var message = SmsMessage.From("Thank you. Your order is confirmed.");
            await _outboundSms.Send(noty.PhoneNumber, message);
        }
    }
}
