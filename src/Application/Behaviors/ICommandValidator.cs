using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Behaviors
{
    internal interface ICommandValidator<in T> where T : IBaseRequest
    {
        Task<IReadOnlyCollection<ValidationException>> Validate(T command, CancellationToken cancellationToken);
    }
}
