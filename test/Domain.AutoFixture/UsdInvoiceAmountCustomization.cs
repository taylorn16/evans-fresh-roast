using AutoFixture;
using Bogus;

namespace Domain.AutoFixture
{
    public sealed class UsdInvoiceAmountCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var faker = new Faker();
            fixture.Register(() => UsdInvoiceAmount.Create(faker.Random.Number(1, 1000)));
        }
    }
}
