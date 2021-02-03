using Dawn;

namespace Domain
{
    public sealed record Description
    {
        private readonly NonEmptyString _description;

        public static Description From(string desc) => new(desc);

        private Description(string description)
        {
            _description = Guard.Argument(NonEmptyString.From(description), nameof(description))
                .Require(n => ((string) n).Length <= 100);
        }

        public override string ToString() => _description;

        public static implicit operator string(Description desc) => desc._description;
    }
}
