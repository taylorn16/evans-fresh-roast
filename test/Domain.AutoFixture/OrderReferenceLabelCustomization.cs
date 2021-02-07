using System;
using AutoFixture;

namespace Domain.AutoFixture
{
    public sealed class OrderReferenceLabelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var rand = new Random();
                var ch = (char) ('a' + rand.Next(0, 26));

                return OrderReferenceLabel.Create(ch.ToString().ToUpperInvariant());
            });
        }
    }
}
