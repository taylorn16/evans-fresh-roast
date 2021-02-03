using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Base;

namespace Application.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> Save(Contact contact, CancellationToken cancellationToken);
        Task<Contact> Get(Id<Contact> id, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> Get(IEnumerable<Id<Contact>> ids, CancellationToken cancellationToken);
        Task<Contact> GetByPhoneNumber(UsPhoneNumber phoneNumber, CancellationToken cancellationToken);
    }
}
