using FluentValidation;
using Infrastructure;
using WebApp.Dto;

namespace WebApp.CoffeeRoastingEvents
{
    public sealed class CoffeeRoastingEventPostRequestValidator : AbstractValidator<CoffeeRoastingEventPostRequest>
    {
        public CoffeeRoastingEventPostRequestValidator(ICurrentTimeProvider currentTimeProvider)
        {
            RuleFor(req => req.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(req => req.RoastDate)
                .NotNull()
                .GreaterThan(currentTimeProvider.Today);

            RuleForEach(req => req.CoffeeIds)
                .NotEmpty().When(req => req.CoffeeIds != null);

            RuleForEach(req => req.ContactIds)
                .NotEmpty().When(req => req.CoffeeIds != null);
        }
    }
}
