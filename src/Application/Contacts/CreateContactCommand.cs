using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.Contacts
{
    public sealed record CreateContactCommand : IRequest<Contact>
    {
        public Name<Contact> Name { get; init; }
        public UsPhoneNumber PhoneNumber { get; init; }
    }

    internal sealed class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Contact>
    {
        private readonly IMediator _mediator;
        private readonly IContactRepository _contactRepository;

        public CreateContactCommandHandler(IMediator mediator, IContactRepository contactRepository)
        {
            _mediator = mediator;
            _contactRepository = contactRepository;
        }

        public async Task<Contact> Handle(CreateContactCommand cmd, CancellationToken cancellationToken)
        {
            var newContact = new Contact(
                Id<Contact>.New(),
                cmd.Name,
                cmd.PhoneNumber,
                false,
                false);

            var savedContact = await _contactRepository.Save(newContact, cancellationToken);

            _mediator.Publish(new ContactCreatedNotification { Contact = savedContact }, cancellationToken);

            return savedContact;
        }
    }
}
