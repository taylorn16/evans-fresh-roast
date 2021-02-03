using System;
using NodaTime;

namespace Infrastructure
{
    public interface ICurrentTimeProvider
    {
        LocalDate Today { get; }
    }

    public sealed class CurrentTimeProvider : ICurrentTimeProvider
    {
        public LocalDate Today => LocalDate.FromDateTime(DateTime.Today);
    }
}
