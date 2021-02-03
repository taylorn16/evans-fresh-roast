using Dawn;

namespace Domain
{
    public sealed record NonEmptyString
    {
        private readonly string _s;

        public static NonEmptyString From(string s) => new(s);

        private NonEmptyString(string s)
        {
            _s = Guard.Argument(s, nameof(s)).NotEmpty().NotWhiteSpace();
        }

        public static implicit operator string(NonEmptyString s) => s._s;

        public override string ToString() => _s;
    }
}
