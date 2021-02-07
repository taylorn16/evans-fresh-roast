using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.Repositories;
using Infrastructure;

namespace Application.CoffeeRoastingEvents.SendTextBlast
{
    public sealed class SendTextBlastCommandValidator : ICommandValidator<SendTextBlastCommand>
    {
        private readonly ICoffeeRoastingEventRepository _coffeeRoastingEventRepository;
        private readonly ICurrentTimeProvider _currentTimeProvider;

        public SendTextBlastCommandValidator(
            ICoffeeRoastingEventRepository coffeeRoastingEventRepository,
            ICurrentTimeProvider currentTimeProvider)
        {
            _coffeeRoastingEventRepository = coffeeRoastingEventRepository;
            _currentTimeProvider = currentTimeProvider;
        }

        public async Task<IReadOnlyCollection<ValidationException>> Validate(
            SendTextBlastCommand cmd, CancellationToken cancellationToken)
        {
            var roastingEvent = await _coffeeRoastingEventRepository.Get(cmd.Event, cancellationToken);
            var errors = new List<ValidationException>();

            if (!roastingEvent.IsActive)
            {
                errors.Add(new ValidationException("You can't send a text blast for a roasting event that's not active."));
            }

            if (roastingEvent.RoastDate < _currentTimeProvider.Today)
            {
                errors.Add(new ValidationException("You can't send a text blast for a roasting event that already happened."));
            }

            return errors;
        }
    }
}
