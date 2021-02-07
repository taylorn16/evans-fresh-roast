using System;
using AutoFixture;
using NodaTime;

namespace Domain.AutoFixture
{
    public sealed class LocalDateCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => LocalDate.FromDateTime(fixture.Create<DateTime>()));
        }
    }
}
