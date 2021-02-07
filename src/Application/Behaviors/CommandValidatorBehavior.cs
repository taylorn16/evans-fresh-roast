using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Behaviors
{
    internal sealed class CommandValidatorBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes>
        where TReq : IRequest<TRes>
    {
        private readonly IEnumerable<ICommandValidator<TReq>> _validators;

        public CommandValidatorBehavior(IEnumerable<ICommandValidator<TReq>> validators)
        {
            _validators = validators;
        }

        public async Task<TRes> Handle(TReq cmd, CancellationToken cancellationToken, RequestHandlerDelegate<TRes> next)
        {
            if (!_validators.Any()) return await next();

            var validationErrors = new ConcurrentBag<ValidationException>();

            foreach (var validator in _validators)
            {
                foreach (var error in await validator.Validate(cmd, cancellationToken))
                {
                    validationErrors.Add(error);
                }
            }

            if (!validationErrors.Any()) return await next();

            var combinedMessage = validationErrors.Select(err => err.Message).Aggregate((a, b) => $"{a} {b}");
            throw new ValidationException(combinedMessage);
        }
    }
}
