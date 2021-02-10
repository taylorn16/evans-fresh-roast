using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Contacts
{
    public sealed record RetrieveAllContactsCommand : IRequest<IEnumerable<Contact>> { }

    internal sealed class RetrieveAllContactsCommandHandler : IRequestHandler<RetrieveAllContactsCommand, IEnumerable<Contact>>
    {
        private readonly IContactRepository _contactRepository;

        public RetrieveAllContactsCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IEnumerable<Contact>> Handle(RetrieveAllContactsCommand cmd, CancellationToken cancellationToken) =>
            await _contactRepository.GetAll(cancellationToken);
    }
}
