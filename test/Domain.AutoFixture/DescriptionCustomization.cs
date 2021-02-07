using AutoFixture;

namespace Domain.AutoFixture
{
    public sealed class DescriptionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => Description.Create(fixture.Create<string>()));
        }
    }
}
