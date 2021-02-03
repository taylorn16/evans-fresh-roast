using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Domain;
using MediatR;

namespace Application.Contacts
{
    public sealed record ContactConfirmedNotification : INotification
    {
        public Contact Contact { get; init; }
    }

    internal sealed class ContactConfirmedNotificationHandler : INotificationHandler<ContactConfirmedNotification>
    {
        private readonly ISendSms _outboundSms;

        public ContactConfirmedNotificationHandler(ISendSms outboundSms)
        {
            _outboundSms = outboundSms;
        }

        public async Task Handle(ContactConfirmedNotification noty, CancellationToken cancellationToken)
        {
            const string message = "Right on! You're in. Thank you, and be on the look out for the next text blast.";

            await _outboundSms.Send(noty.Contact.PhoneNumber, SmsMessage.From(message));
        }
    }
}
