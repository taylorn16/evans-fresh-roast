using Dawn;
using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed record Description
    {
        private readonly NonEmptyString _description;

        public static Description Create(string desc) => ValueObject.SafeCreate(
            () => new Description(desc),
            ex => new InvalidDescriptionException(desc, ex));

        private Description(string description)
        {
            _description = Guard.Argument(NonEmptyString.From(description), nameof(description))
                .Require(n => ((string) n).Length <= 100);
        }

        public override string ToString() => _description;

        public static implicit operator string(Description desc) => desc._description;
    }
}
