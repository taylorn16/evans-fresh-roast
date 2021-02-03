using MediatR;

namespace Application
{
    internal sealed record NoopCommand : IRequest { }

    internal sealed class NoopCommandHandler : RequestHandler<NoopCommand>
    {
        protected override void Handle(NoopCommand _) { }
    }
}
