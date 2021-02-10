using System.Runtime.CompilerServices;
using Adapter.Data.CoffeeRoastingEvents;
using Adapter.Data.Coffees;
using Adapter.Data.Contacts;
using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("WebApp.RegressionTests")]

namespace Adapter.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAdapter(this IServiceCollection services)
        {
            services.AddTransient<IDbContextFactory, DbContextFactory>();

            services.AddTransient<ICoffeeRoastingEventDataLayer, CoffeeRoastingEventDataLayer>();
            services.AddTransient<ICoffeeRoastingEventMapper, CoffeeRoastingEventMapper>();
            services.AddTransient<ICoffeeRoastingEventRepository, CoffeeRoastingEventRepository>(); // port impl

            services.AddTransient<ICoffeeDataLayer, CoffeeDataLayer>();
            services.AddTransient<ICoffeeMapper, CoffeeMapper>();
            services.AddTransient<ICoffeeRepository, CoffeeRepository>(); // port impl

            services.AddTransient<IContactDataLayer, ContactDataLayer>();
            services.AddTransient<IContactMapper, ContactMapper>();
            services.AddTransient<IContactRepository, ContactRepository>(); // port impl

            return services;
        }
    }
}
