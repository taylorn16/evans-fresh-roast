using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Domain.AutoFixture;

namespace Application.IntegrationTests
{
    public static class AutoFixtureFactory
    {
        public static IFixture Create() =>
            new Fixture()
                .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true })
                .AddDomainCustomizations();
    }
}
