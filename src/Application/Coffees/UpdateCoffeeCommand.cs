using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;

namespace Application.Coffees
{
    public sealed record UpdateCoffeeCommand : IRequest<Coffee>
    {
        public Id<Coffee> CoffeeId { get; init; }
        public Name<Coffee>? Name { get; init; }
        public Description? Description { get; init; }
        public UsdPrice? Price { get; init; }
        public Ounces? WeightPerBag { get; init; }
    }

    internal sealed class UpdateCoffeeCommandHandler : IRequestHandler<UpdateCoffeeCommand, Coffee>
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public UpdateCoffeeCommandHandler(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public async Task<Coffee> Handle(UpdateCoffeeCommand cmd, CancellationToken cancellationToken)
        {
            var coffee = (await _coffeeRepository.Get(new[] { cmd.CoffeeId }, cancellationToken)).Single();

            var updatedCoffee = coffee.With(cmd.Name, cmd.Description, cmd.Price, cmd.WeightPerBag);
            await _coffeeRepository.Save(updatedCoffee, cancellationToken);

            return updatedCoffee;
        }
    }
}
