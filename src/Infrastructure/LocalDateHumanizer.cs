using Humanizer;
using NodaTime;

namespace Infrastructure
{
    public interface ILocalDateHumanizer
    {
        string Humanize(LocalDate date);
    }

    public sealed class LocalDateHumanizer : ILocalDateHumanizer
    {
        public string Humanize(LocalDate date) => date.ToDateTimeUnspecified().Humanize(false);
    }
}
