using AutoFixture;
using Bogus;

namespace Domain.AutoFixture
{
    public sealed class UsdPriceCustomization : ICustomization
    {
        private readonly Faker _faker = new();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => UsdPrice.Create(_faker.Random.Decimal(1, 20)));
        }
    }
}
