using System;
using AutoFixture;
using NodaTime;

namespace Domain.AutoFixture
{
    public sealed class OffsetDateTimeCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => OffsetDateTime.FromDateTimeOffset(fixture.Create<DateTimeOffset>()));
        }
    }
}
