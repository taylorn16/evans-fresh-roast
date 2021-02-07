using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Application.Repositories;
using Domain;
using Domain.Base;
using Infrastructure;
using MediatR;

namespace Application.CoffeeRoastingEvents.SendTextBlast
{
    public sealed record SendTextBlastCommand : IRequest
    {
        public Id<CoffeeRoastingEvent> Event { get; init; }
    }

    internal sealed class SendTextBlastCommandHandler : AsyncRequestHandler<SendTextBlastCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ILocalDateHumanizer _localDateHumanizer;
        private readonly ISendSms _outboundSms;

        public SendTextBlastCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IContactRepository contactRepository,
            ILocalDateHumanizer localDateHumanizer,
            ISendSms outboundSms)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _contactRepository = contactRepository;
            _localDateHumanizer = localDateHumanizer;
            _outboundSms = outboundSms;
        }

        protected override async Task Handle(SendTextBlastCommand cmd, CancellationToken cancellationToken)
        {
            var roastingEvent = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);

            var contacts = (await _contactRepository.Get(roastingEvent.NotifiedContacts, cancellationToken))
                .Where(c => c.IsConfirmed)
                .Where(c => c.IsActive);

            await SendBlastMessages(roastingEvent, contacts);
        }

        private async Task SendBlastMessages(CoffeeRoastingEvent roastingEvent, IEnumerable<Contact> contacts)
        {
            var message = BuildTextBlastMessage(roastingEvent);
            await Task.WhenAll(contacts.Select(c => _outboundSms.Send(c.PhoneNumber, message)));
        }

        private SmsMessage BuildTextBlastMessage(CoffeeRoastingEvent @event)
        {
            var message = new StringBuilder();

            message.AppendLine($"Evan's fresh roast notice! I'll be roasting on {@event.RoastDate.DayOfWeek} " +
                          $"({@event.RoastDate} - {_localDateHumanizer.Humanize(@event.RoastDate)}). " +
                          "Here are the options:");
            message.AppendLine();
            message.AppendLine(@event.GetCoffeesSummary());
            message.AppendLine();
            message.AppendLine("Please reply to place an order, using this format, with each item on a new line, like this:");
            message.AppendLine("qty 2 B");
            message.AppendLine("qty 1 C");

            return SmsMessage.Create(message.ToString());
        }
    }
}
