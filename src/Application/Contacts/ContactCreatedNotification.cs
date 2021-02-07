using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Domain;
using MediatR;

namespace Application.Contacts
{
    public sealed record ContactCreatedNotification : INotification
    {
        public Contact Contact { get; init; }
    }

    internal sealed class ContactCreatedNotificationHandler : INotificationHandler<ContactCreatedNotification>
    {
        private readonly ISendSms _outboundSms;

        public ContactCreatedNotificationHandler(ISendSms outboundSms)
        {
            _outboundSms = outboundSms;
        }

        public async Task Handle(ContactCreatedNotification noty, CancellationToken cancellationToken)
        {
            var newContactMessage = $"Hey there {noty.Contact.Name}! You've been selected to receive coffee notifications" +
                                    " for Evan's Fresh Roast. Nice!\n\nIf that sounds good to you, please reply I'M IN.";

            await _outboundSms.Send(noty.Contact.PhoneNumber, SmsMessage.Create(newContactMessage));
        }
    }
}
