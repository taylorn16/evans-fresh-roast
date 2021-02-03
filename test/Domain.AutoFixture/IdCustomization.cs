using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Domain.Base;

namespace Domain.AutoFixture
{
    public sealed class IdCustomization: ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IdSpecimenBuilder());
        }

        private sealed class IdSpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var type = request as Type;

                if (type == null ||
                    !type.IsGenericType ||
                    type.GetGenericTypeDefinition() != typeof(Id<>))
                {
                    return new NoSpecimen();
                }

                var staticNew = type.GetMethod("New", BindingFlags.Static | BindingFlags.Public);

                return staticNew == null ?
                    new NoSpecimen() :
                    staticNew.Invoke(null, null) ?? new NoSpecimen();
            }
        }
    }
}
