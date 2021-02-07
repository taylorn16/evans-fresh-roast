using AutoFixture;

namespace Domain.AutoFixture
{
    public static class AutoFixtureExtensions
    {
        public static IFixture AddDomainCustomizations(this IFixture fixture) => fixture
            .Customize(new IdCustomization())
            .Customize(new NameCustomization())
            .Customize(new DescriptionCustomization())
            .Customize(new UsdPriceCustomization())
            .Customize(new OuncesCustomization())
            .Customize(new LocalDateCustomization())
            .Customize(new UsPhoneNumberCustomization())
            .Customize(new OrderReferenceLabelCustomization())
            .Customize(new OffsetDateTimeCustomization())
            .Customize(new OrderQuantityCustomization())
            .Customize(new UsdInvoiceAmountCustomization())
            .Customize(new CoffeeRoastingEventCustomization());
    }
}
