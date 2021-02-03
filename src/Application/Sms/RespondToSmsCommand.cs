using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;

namespace Application.Sms
{
    public sealed record RespondToSmsCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
        public SmsMessage Message { get; set; }
    }

    internal sealed class RespondToSmsCommandHandler : AsyncRequestHandler<RespondToSmsCommand>
    {
        private readonly ISmsParser _smsParser;
        private readonly IMediator _mediator;

        public RespondToSmsCommandHandler(
            ISmsParser smsParser,
            IMediator mediator)
        {
            _smsParser = smsParser;
            _mediator = mediator;
        }

        protected override async Task Handle(RespondToSmsCommand cmd, CancellationToken cancellationToken)
        {
            var resultantCmd = _smsParser.Parse(cmd.PhoneNumber, cmd.Message);

            await _mediator.Send(resultantCmd, cancellationToken);
        }
    }
}
