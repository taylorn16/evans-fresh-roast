using AutoFixture;
using Bogus;

namespace Domain.AutoFixture
{
    public sealed class OuncesCustomization : ICustomization
    {
        private readonly Faker _faker = new();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => Ounces.Create(_faker.Random.Decimal(1, 100)));
        }
    }
}
