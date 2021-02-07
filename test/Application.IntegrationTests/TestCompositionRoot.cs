using System;
using Application.Ports;
using Application.Repositories;
using AutoFixture;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public sealed class TestCompositionRoot
    {
        private readonly IServiceProvider _container;

        public TestCompositionRoot()
        {
            var fixture = AutoFixtureFactory.Create();
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddScoped(_ => fixture);
            services.AddScoped(_ => fixture.Freeze<ICoffeeRepository>());
            services.AddScoped(_ => fixture.Freeze<ICoffeeRoastingEventRepository>());
            services.AddScoped(_ => fixture.Freeze<ICurrentTimeProvider>());
            services.AddScoped(_ => fixture.Freeze<IContactRepository>());
            services.AddScoped(_ => fixture.Freeze<ISendSms>());

            _container = services.BuildServiceProvider();
        }

        public T Get<T>() => _container.GetRequiredService<T>();
    }
}
