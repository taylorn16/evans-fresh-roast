using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Domain;
using MediatR;

namespace Application.Orders
{
    public sealed record OrderCancelledNotification : INotification
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class OrderCancelledNotificationHandler : INotificationHandler<OrderCancelledNotification>
    {
        private readonly ISendSms _outboundSms;

        public OrderCancelledNotificationHandler(ISendSms outboundSms)
        {
            _outboundSms = outboundSms;
        }

        public async Task Handle(OrderCancelledNotification noty, CancellationToken cancellationToken)
        {
            await _outboundSms.Send(noty.PhoneNumber, SmsMessage.Create("Thank you. Your order has been cancelled."));
        }
    }
}
