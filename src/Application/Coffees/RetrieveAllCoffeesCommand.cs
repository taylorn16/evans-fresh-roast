using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using MediatR;

namespace Application.Coffees
{
    public sealed record RetrieveAllCoffeesCommand : IRequest<IEnumerable<Coffee>> { }

    internal sealed class RetrieveAllCoffeesCommandHandler : IRequestHandler<RetrieveAllCoffeesCommand, IEnumerable<Coffee>>
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public RetrieveAllCoffeesCommandHandler(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public async Task<IEnumerable<Coffee>> Handle(RetrieveAllCoffeesCommand request, CancellationToken cancellationToken) =>
            await _coffeeRepository.GetAll(cancellationToken);
    }
}
