using System;
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

namespace Application.CoffeeRoastingEvents
{
    public sealed record SendTextBlastCommand : IRequest
    {
        public Id<CoffeeRoastingEvent> Event { get; init; }
    }

    internal sealed class SendTextBlastCommandHandler : AsyncRequestHandler<SendTextBlastCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentTimeProvider _currentTimeProvider;
        private readonly ILocalDateHumanizer _localDateHumanizer;
        private readonly ISendSms _outboundSms;

        public SendTextBlastCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IContactRepository contactRepository,
            ICurrentTimeProvider currentTimeProvider,
            ILocalDateHumanizer localDateHumanizer,
            ISendSms outboundSms)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _contactRepository = contactRepository;
            _currentTimeProvider = currentTimeProvider;
            _localDateHumanizer = localDateHumanizer;
            _outboundSms = outboundSms;
        }

        protected override async Task Handle(SendTextBlastCommand cmd, CancellationToken cancellationToken)
        {
            var roastingEvent = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);

            if (!roastingEvent.IsActive) // TODO use validation in the pipeline instead?
            {
                throw new ApplicationException("You can't send a text blast for a roasting event that's not active.");
            }

            if (roastingEvent.Date < _currentTimeProvider.Today)
            {
                throw new ApplicationException("You can't send a text blast for a roasting event that already happened!");
            }

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
            var sb = new StringBuilder();

            sb.AppendLine($"Evan's fresh roast notice! I'll be roasting on {@event.Date.DayOfWeek} " +
                          $"({@event.Date} - {_localDateHumanizer.Humanize(@event.Date)}). " +
                          "Here are the options:");
            sb.AppendLine();
            sb.AppendLine(@event.GetOfferingSummary());
            sb.AppendLine();
            sb.AppendLine("Please reply to place an order, using this format, with each item on a new line, like this:");
            sb.AppendLine("qty 2 B");
            sb.AppendLine("qty 1 C");

            return SmsMessage.From(sb.ToString());
        }
    }
}
