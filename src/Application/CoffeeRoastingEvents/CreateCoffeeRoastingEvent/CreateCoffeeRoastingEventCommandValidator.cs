using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Infrastructure;

namespace Application.CoffeeRoastingEvents.CreateCoffeeRoastingEvent
{
    public sealed class CreateCoffeeRoastingEventCommandValidator : ICommandValidator<CreateCoffeeRoastingEventCommand>
    {
        private readonly ICurrentTimeProvider _currentTimeProvider;

        public CreateCoffeeRoastingEventCommandValidator(ICurrentTimeProvider currentTimeProvider)
        {
            _currentTimeProvider = currentTimeProvider;
        }

        public Task<IReadOnlyCollection<ValidationException>> Validate(
            CreateCoffeeRoastingEventCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.RoastDate >= _currentTimeProvider.Today)
            {
                IReadOnlyCollection<ValidationException> emptyErrors = Array.Empty<ValidationException>();
                return Task.FromResult(emptyErrors);
            }

            IReadOnlyCollection<ValidationException> errors = new List<ValidationException>
            {
                new("You cannot create a roasting event in the past.")
            };

            return Task.FromResult(errors);
        }
    }
}
