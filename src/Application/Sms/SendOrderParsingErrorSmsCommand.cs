using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Ports;
using Domain;
using MediatR;

namespace Application.Sms
{
    public sealed record SendOrderParsingErrorSmsCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
        public SmsMessage Message { get; init; }
    }

    internal sealed class SendOrderParsingErrorSmsCommandHandler : AsyncRequestHandler<SendOrderParsingErrorSmsCommand>
    {
        private readonly ISendSms _outboundSms;

        public SendOrderParsingErrorSmsCommandHandler(ISendSms outboundSms)
        {
            _outboundSms = outboundSms;
        }

        protected override async Task Handle(SendOrderParsingErrorSmsCommand cmd, CancellationToken cancellationToken)
        {
            var message = new StringBuilder();

            message.Append("Sorry, we didn't understand your order. ");
            message.AppendLine($"Here's some things we noticed about it: {cmd.Message}");
            message.AppendLine();
            message.AppendLine("If you are trying to place an order, use this format, with each item on a new line, like this:");
            message.AppendLine("qty 2 B");
            message.AppendLine("qty 1 C");

            await _outboundSms.Send(cmd.PhoneNumber, SmsMessage.Create(message.ToString()));
        }
    }
}
