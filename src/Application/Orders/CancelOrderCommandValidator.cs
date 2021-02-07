using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.Repositories;
using Infrastructure;

namespace Application.Orders
{
    internal sealed class CancelOrderCommandValidator : ICommandValidator<CancelOrderCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly ICurrentTimeProvider _currentTimeProvider;

        public CancelOrderCommandValidator(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            ICurrentTimeProvider currentTimeProvider)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _currentTimeProvider = currentTimeProvider;
        }

        public async Task<IReadOnlyCollection<ValidationException>> Validate(CancelOrderCommand cmd, CancellationToken cancellationToken)
        {
            var activeEvent = await _coffeeRoastingEventRepository.GetActiveEvent(cancellationToken);

            if (_currentTimeProvider.Today > activeEvent.OrderByDate)
            {
                IReadOnlyCollection<ValidationException> errors = new List<ValidationException>
                {
                    new("It's too late to place an order for this event.")
                };
                return errors;
            }

            IReadOnlyCollection<ValidationException> emptyErrors = Array.Empty<ValidationException>();
            return emptyErrors;
        }
    }
}
