using System;
using Application.Repositories;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public sealed class CompositionRoot
    {
        private readonly IServiceProvider _container;

        public CompositionRoot()
        {
            var fixture = AutoFixtureFactory.Create();
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddScoped(_ => fixture);
            services.AddScoped(_ => fixture.Freeze<ICoffeeRepository>());

            _container = services.BuildServiceProvider();
        }

        public T Get<T>() => _container.GetRequiredService<T>();
    }
}
