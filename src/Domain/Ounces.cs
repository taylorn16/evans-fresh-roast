using Dawn;
using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record Ounces
    {
        private readonly decimal _oz;

        public static Ounces Create(decimal oz) => ValueObject.SafeCreate(
            () => new Ounces(oz),
            ex => new InvalidOuncesException(oz, ex));

        private Ounces(decimal oz)
        {
            _oz = Guard.Argument(oz, nameof(oz)).Positive().Max(100);
        }

        public override string ToString() => $"{_oz:N} oz";

        public static implicit operator decimal(Ounces oz) => oz._oz;
    }
}
