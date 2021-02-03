using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Contacts
{
    public sealed record ConfirmContactCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class ConfirmContactCommandHandler : AsyncRequestHandler<ConfirmContactCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMediator _mediator;

        public ConfirmContactCommandHandler(
            IContactRepository contactRepository,
            IMediator mediator)
        {
            _contactRepository = contactRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(ConfirmContactCommand cmd, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByPhoneNumber(cmd.PhoneNumber, cancellationToken);

            contact.Confirm();

            await _contactRepository.Save(contact, cancellationToken);

            await _mediator.Publish(new ContactConfirmedNotification { Contact = contact }, cancellationToken);
        }
    }
}
