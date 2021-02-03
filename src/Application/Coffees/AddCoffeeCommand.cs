using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.Coffees
{
    public sealed record AddCoffeeCommand : IRequest<Coffee>
    {
        public Name<Coffee> Name { get; init; }
        public Description Description { get; init; }
        public UsdPrice Price { get; init; }
        public Ounces NetWeightPerBag { get; init; }
    }

    internal sealed class AddCoffeeCommandHandler : IRequestHandler<AddCoffeeCommand, Coffee>
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public AddCoffeeCommandHandler(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public async Task<Coffee> Handle(AddCoffeeCommand cmd, CancellationToken cancellationToken)
        {
            var coffee = new Coffee(
                Id<Coffee>.New(),
                cmd.Name,
                cmd.Description,
                cmd.Price,
                cmd.NetWeightPerBag);

            var savedCoffee = await _coffeeRepository.Save(coffee, cancellationToken);

            return savedCoffee;
        }
    }
}
