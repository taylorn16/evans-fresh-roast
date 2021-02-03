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
    public sealed record SendFollowUpTextBlastCommand : IRequest
    {
        public Id<CoffeeRoastingEvent> Event { get; init; }
    }

    internal sealed class SendFollowUpTextBlastCommandHandler : AsyncRequestHandler<SendFollowUpTextBlastCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentTimeProvider _currentTimeProvider;
        private readonly ISendSms _outboundSms;

        public SendFollowUpTextBlastCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            IContactRepository contactRepository,
            ICurrentTimeProvider currentTimeProvider,
            ISendSms outboundSms)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _contactRepository = contactRepository;
            _currentTimeProvider = currentTimeProvider;
            _outboundSms = outboundSms;
        }

        protected override async Task Handle(SendFollowUpTextBlastCommand cmd, CancellationToken cancellationToken)
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

            var orders = roastingEvent.Orders.ToArray();
            var contacts = (await _contactRepository.Get(roastingEvent.NotifiedContacts, cancellationToken))
                .Where(c => c.IsConfirmed)
                .Where(c => c.IsActive)
                .ToArray();

            var unconfirmedOrderPhoneNumbers =
                from order in orders
                where !order.IsConfirmed
                join contact in contacts on order.Contact equals contact.Id
                select contact.PhoneNumber;

            var notOrderedPhoneNumbers =
                from contactId in roastingEvent.NotifiedContacts.Except(orders.Select(o => o.Contact))
                join contact in contacts on contactId equals contact.Id
                select contact.PhoneNumber;

            await Task.WhenAll(
                SendOrderConfirmationReminders(unconfirmedOrderPhoneNumbers)
                    .Concat(SendNotYetOrderedReminders(notOrderedPhoneNumbers)));
        }

        private IEnumerable<Task> SendOrderConfirmationReminders(IEnumerable<UsPhoneNumber> phoneNumbers)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Evan's fresh roast here! Looks like you haven't confirmed your order yet.");
            sb.AppendLine();
            sb.AppendLine("Please reply CONFIRM to confirm your order or CANCEL to cancel it.");

            var message = SmsMessage.From(sb.ToString());

            return phoneNumbers.Select(phn => _outboundSms.Send(phn, message));
        }

        private IEnumerable<Task> SendNotYetOrderedReminders(IEnumerable<UsPhoneNumber> phoneNumbers)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Evan's fresh roast here! Looks like you haven't placed an order yet.");
            sb.AppendLine();
            sb.AppendLine("If you'd like to place an order, please reply."); // TODO should include options again, maybe? Or the order format?

            var message = SmsMessage.From(sb.ToString());

            return phoneNumbers.Select(phn => _outboundSms.Send(phn, message));
        }
    }
}
