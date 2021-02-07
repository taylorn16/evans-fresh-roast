using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.Repositories;
using Infrastructure;

namespace Application.CoffeeRoastingEvents.SendTextBlast
{
    internal sealed class SendFollowUpTextBlastCommandValidator : ICommandValidator<SendFollowUpTextBlastCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly ICurrentTimeProvider _currentTimeProvider;

        public SendFollowUpTextBlastCommandValidator(ICoffeeRoastingEventRepository coffeeRoastingEventRepository, ICurrentTimeProvider currentTimeProvider)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _currentTimeProvider = currentTimeProvider;
        }

        public async Task<IReadOnlyCollection<ValidationException>> Validate(SendFollowUpTextBlastCommand cmd, CancellationToken cancellationToken)
        {
            var @event = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);

            var errors = new List<ValidationException>();

            if (!@event.IsActive)
            {
                errors.Add(new ValidationException("This event is not active. You can't send a text blast reminder out for it."));
            }

            if (_currentTimeProvider.Today > @event.OrderByDate)
            {
                errors.Add(new ValidationException("The order-by date on this event has already passed. You can't send a text blast reminder out for it."));
            }

            IReadOnlyCollection<ValidationException> validationErrors = errors.AsReadOnly();
            return validationErrors;
        }
    }
}
