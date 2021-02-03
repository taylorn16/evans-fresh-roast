using System.Linq;
using System.Text.RegularExpressions;
using Dawn;

namespace Domain
{
    public sealed record UsPhoneNumber
    {
        private readonly NonEmptyString _phoneNumber;

        public static UsPhoneNumber From(string phoneNumber) => new(phoneNumber);

        private UsPhoneNumber(string phoneNumber)
        {
            var cleaned = new Regex(@"\D").Replace(phoneNumber, string.Empty).Trim();
            if (cleaned.Length == 11) cleaned = cleaned.Substring(1);

            _phoneNumber = Guard.Argument(NonEmptyString.From(cleaned), nameof(phoneNumber))
                .Require(num => ((string) num).Length == 10)
                .Require(num => ((string) num).Any(c => c != '0'));
        }

        public override string ToString() => ToE164Format();

        public static implicit operator string(UsPhoneNumber phoneNumber) => phoneNumber.ToE164Format();

        private string ToE164Format() => $"1{_phoneNumber}";
    }
}
