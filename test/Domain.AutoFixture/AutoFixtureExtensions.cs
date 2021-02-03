using AutoFixture;

namespace Domain.AutoFixture
{
    public static class AutoFixtureExtensions
    {
        public static IFixture AddDomainCustomizations(this IFixture fixture)
        {
            fixture
                .Customize(new IdCustomization())
                .Customize(new NameCustomization())
                .Customize(new DescriptionCustomization())
                .Customize(new UsdPriceCustomization())
                .Customize(new OuncesCustomization());

            return fixture;
        }
    }
}
