using AutoFixture;
using Bogus;

namespace Domain.AutoFixture
{
    public sealed class OrderQuantityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var faker = new Faker();
            fixture.Register(() => OrderQuantity.Create(faker.Random.Number(1, 15)));
        }
    }
}
