using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace Domain.AutoFixture
{
    public sealed class NameCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new NameSpecimenBuilder());
        }

        private sealed class NameSpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var fixture = new Fixture();

                var type = request as Type;

                if (type == null ||
                    !type.IsGenericType ||
                    type.GetGenericTypeDefinition() != typeof(Name<>))
                {
                    return new NoSpecimen();
                }

                var staticFrom = type.GetMethod("From", BindingFlags.Static | BindingFlags.Public);

                return staticFrom == null ?
                    new NoSpecimen() :
                    staticFrom.Invoke(null, new object[] { fixture.Create<string>() }) ?? new NoSpecimen();
            }
        }
    }
}
