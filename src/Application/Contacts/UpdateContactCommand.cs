using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.Contacts
{
    public sealed record UpdateContactCommand : IRequest<Contact>
    {
        public Id<Contact> ContactId { get; init; }
        public Name<Contact> Name { get; init; }
    }

    internal sealed class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Contact>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Contact> Handle(UpdateContactCommand cmd, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.Get(cmd.ContactId, cancellationToken);

            contact.SetName(cmd.Name);
            await _contactRepository.Save(contact, cancellationToken);

            return contact;
        }
    }
}
