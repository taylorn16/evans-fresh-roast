using System.Text.RegularExpressions;
using Dawn;

namespace Domain
{
    public sealed record OrderReferenceLabel
    {
        private static readonly Regex LabelPattern = new(@"^[A-Z]{1,2}$");

        private readonly NonEmptyString _label;

        public static OrderReferenceLabel From(string label) => new(label);

        private OrderReferenceLabel(string label)
        {
            _label = Guard.Argument(NonEmptyString.From(label), nameof(label))
                .Require(l => LabelPattern.IsMatch(l));
        }

        public override string ToString() => _label;

        public static implicit operator string(OrderReferenceLabel label) => label._label;
    }
}
