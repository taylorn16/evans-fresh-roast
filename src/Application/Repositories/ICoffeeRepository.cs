using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Base;

namespace Application.Repositories
{
    public interface ICoffeeRepository
    {
        Task<Coffee> Save(Coffee coffee, CancellationToken cancellationToken);
        Task<IEnumerable<Coffee>> Get(IEnumerable<Id<Coffee>> ids, CancellationToken cancellationToken);
        Task<IEnumerable<Coffee>> GetAll(CancellationToken cancellationToken);
    }
}
