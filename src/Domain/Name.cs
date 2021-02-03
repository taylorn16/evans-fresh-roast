using Dawn;

namespace Domain
{
    public sealed record Name<T>
    {
        private readonly NonEmptyString _name;

        public static Name<T> From(string name) => new(name);

        private Name(string name)
        {
            _name = Guard.Argument(NonEmptyString.From(name), nameof(name))
                .Require(n => ((string) n).Length <= 100);
        }

        public override string ToString() => _name;

        public static implicit operator string(Name<T> name) => name._name;
    }
}
