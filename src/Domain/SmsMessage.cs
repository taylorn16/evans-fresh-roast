namespace Domain
{
    public sealed record SmsMessage
    {
        private readonly NonEmptyString _message;

        public static SmsMessage From(string message) => new(message);

        private SmsMessage(string message)
        {
            _message = NonEmptyString.From(message);
        }

        public override string ToString() => _message;

        public static implicit operator string(SmsMessage message) => message._message;
    }
}
