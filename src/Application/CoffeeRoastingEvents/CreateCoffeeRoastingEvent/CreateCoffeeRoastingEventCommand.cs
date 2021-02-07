using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;
using MediatR;
using NodaTime;

namespace Application.CoffeeRoastingEvents.CreateCoffeeRoastingEvent
{
    public sealed record CreateCoffeeRoastingEventCommand : IRequest<CoffeeRoastingEvent>
    {
        public Name<CoffeeRoastingEvent> Name { get; init; }
        public IReadOnlyCollection<Id<Contact>> Contacts { get; init; }
        public LocalDate RoastDate { get; init; }
        public LocalDate OrderByDate { get; init; }
        public IReadOnlyCollection<Id<Coffee>> Coffees { get; init; }
    }

    internal sealed class CreateCoffeeRoastingEventCommandHandler : IRequestHandler<CreateCoffeeRoastingEventCommand, CoffeeRoastingEvent>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ILabelCoffees _coffeeLabeler;

        public CreateCoffeeRoastingEventCommandHandler(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            ICoffeeRepository coffeeRepository,
            ILabelCoffees coffeeLabeler)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _coffeeRepository = coffeeRepository;
            _coffeeLabeler = coffeeLabeler;
        }

        public async Task<CoffeeRoastingEvent> Handle(CreateCoffeeRoastingEventCommand cmd, CancellationToken cancellationToken)
        {
            var coffees = (await _coffeeRepository.Get(cmd.Coffees, cancellationToken)).ToArray();
            var labeledCoffees = _coffeeLabeler.Label(coffees);

            var @event = new CoffeeRoastingEvent(
                Id<CoffeeRoastingEvent>.New(),
                cmd.Contacts,
                true,
                cmd.RoastDate,
                cmd.OrderByDate,
                cmd.Name,
                labeledCoffees,
                Enumerable.Empty<Order>());

            return await _coffeeRoastingEventRepository.Save(@event, cancellationToken);
        }
    }
}
