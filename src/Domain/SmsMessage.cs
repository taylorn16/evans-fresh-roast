using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record SmsMessage
    {
        private readonly NonEmptyString _message;

        public static SmsMessage Create(string message) => ValueObject.SafeCreate(
            () => new SmsMessage(message),
            ex => new InvalidSmsMessageException(message, ex));

        private SmsMessage(string message)
        {
            _message = NonEmptyString.From(message);
        }

        public override string ToString() => _message;

        public static implicit operator string(SmsMessage message) => message._message;
    }
}
