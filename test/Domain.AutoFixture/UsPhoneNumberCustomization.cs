using AutoFixture;
using Bogus;

namespace Domain.AutoFixture
{
    public sealed class UsPhoneNumberCustomization : ICustomization
    {
        private readonly Faker _faker = new();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var number = _faker.Phone.PhoneNumber("##########");
                return UsPhoneNumber.Create($"+1{number}");
            });
        }
    }
}
